﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float moveSpeed = 100f;

    private ParticleSystem bulletSFX;

    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        bulletSFX = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);

        // 检测碰撞发生
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemy)
        {
            // SFX播放
            bulletSFX.Play();
            // 碰撞到物体
            Destroy(gameObject);
            Debug.Log("hit enemy");
        }
    }
}