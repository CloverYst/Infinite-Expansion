﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    public float speed = 10;
    public float hp = 150;
    public int attackDistance = 2;
    public int attackPower = 10;
    public float attackRate = 1;
    private float totalHp;
    private Slider hpSlider;
    private GameObject target;
    private string status = "forward";
    private Transform[] positions;
    private int index = 0;
    private Animator anim;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        positions = Waypoints.positions;
        hpSlider = GetComponentInChildren<Slider>();
        totalHp = hp;
        anim = GetComponent<Animator>();
        timer = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (status == "forward")
        {
            Move();
        }
        else if (status == "fight")
        {
            if (target == null)
            {
                this.status = "forward";
                return;
            }
            if (Vector3.Distance(target.transform.position, transform.position) > attackDistance)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                //TODO 调用奔跑动画
                anim.SetBool("run",true);
                transform.forward = target.transform.position - transform.position;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= attackRate)
                {
                    timer = 0;
                    Fight();
                }
            }
        }
    }

    private void Move()
    {
        if (index > positions.Length - 1)
        {
            return;
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        anim.SetBool("walkf",true);
        transform.forward = positions[index].position - transform.position;
        if(Time.deltaTime * speed >= Vector3.Distance (transform.position, positions[index].position)){
            index++;
        }
        else if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }

    void ReachDestination()
    {
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;    
    }

    public void TakeDamage(float damage, GameObject source)
    {
        if (hp <= 0)
        {
            return;
        }
        hp -= damage;
        hpSlider.value = hp / totalHp;
        if (hp <= 0)
        {
            Die();
        }
        if (source != null)
        {
            if (source.tag == "Turret")
            {
                this.status = "fight";
                target = source;
            }
        }
    }
    void Die()
    {
        anim.SetTrigger("death");
        float dieTime = 1.8f;
        Destroy(this.gameObject, dieTime);
    }
    void Fight()
    {
        //TODO 调用攻击动画
        
        anim.SetBool("attack0",true);

        anim.SetBool("attack1",true);
       
        transform.Translate(new Vector3(0, 0, 0));
        target.GetComponent<Turret>().TakeDamage(attackPower);
    }
}