using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;

    public bool isPlayerBullet = true;
    
    public GameObject gunFirePrefab;

    public GameObject explosionPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        gunFirePrefab.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * (speed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Tank":
                if (!isPlayerBullet)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                break;
            case "Heart":
                collision.SendMessage("Hit");
                Destroy(gameObject);
                break;
            case "Enemy":
                if (isPlayerBullet)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                break;
            case "Wall":
                // destroy wall
                Destroy(collision.gameObject);
                //destroy bullet
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(gameObject, 0.1f);
                break;
            case "Barrier":
                collision.SendMessage("Hit");
                //destroy bullet
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(gameObject, 0.1f);
                break;
            case "Border":
                Destroy(gameObject);
                break;
            case "Grass":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
    
}
