﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 500 * Time.deltaTime);
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit detected");
            if (collision.gameObject.GetComponent<PlayerController>().isDashing)
            {
                // Dashing player destroys an enemy
                Destroy(gameObject);
            }
            else
            {
                // Non-dashing player gets hurt
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Destroy")
        {
            Debug.Log("Object destroy");
            Destroy(this.gameObject);
        }
    }
}