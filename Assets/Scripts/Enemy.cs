using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3;

    // up: 0, right: 1, down: 2, left: 3
    public Sprite[] tankSprites = new Sprite[4];

    public GameObject bulletPrefab;
    
    public float attackCD = 2f;

    public GameObject explosionPrefab;
    
    public AudioClip hitSound;

    private SpriteRenderer sr;

    private Vector3 bulletEulerAngles;
    private float v = -1;
    private float h = 0;
    
    // counters
    private float timeOfTurning = 4f;
    private float timeOfChangeDirectionCounter;
    private float attackTimeCd = 0;
    
    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        timeOfChangeDirectionCounter = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }
        
        if (attackTimeCd >= Random.Range(attackCD, 7))
        {
            Attack();
        }
        else
        {
            attackTimeCd += Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }
        Move();
    }

    private void Attack()
    {
        Instantiate(bulletPrefab, 
            transform.position, 
            Quaternion.Euler(transform.eulerAngles +
                             bulletEulerAngles));
        attackTimeCd = 0;
    }

    private void Move()
    {
        Turn();

        if (h < 0)
        {
            sr.sprite = tankSprites[3];
            bulletEulerAngles = new Vector3(0,0,90);
        } 
        else if (h > 0)
        {
            sr.sprite = tankSprites[1];
            bulletEulerAngles = new Vector3(0,0,-90);
        }
        
        transform.Translate(Vector3.right * (h * moveSpeed * Time.deltaTime), Space.World);

        if (h != 0)
        {
            return;
        }

        if (v < 0)
        {
            sr.sprite = tankSprites[2];
            bulletEulerAngles = new Vector3(0,0,-180);
        } 
        else if (v > 0)
        {
            sr.sprite = tankSprites[0];
            bulletEulerAngles = new Vector3(0,0,0);
        }
        
        transform.Translate(Vector3.up * (v * moveSpeed * Time.deltaTime), Space.World);
    }
    
    // tank has been destroyed by a bullet
    private void Die()
    {
        // score plus
        PlayerManager.Instance.playerScore++;
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        // explode
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        // dead
        Destroy(gameObject);
    }

    private void Turn()
    {
        if (timeOfChangeDirectionCounter >= timeOfTurning)
        {
            int num = Random.Range(0, 8);
            if (num > 4)
            {
                v = -1;
                h = 0;
            }
            else if (num == 0)
            {
                v = 1;
                h = 0;
            }
            else if (num > 0 && num <= 2)
            {
                h = -1;
                v = 0;
            }
            else
            {
                h = 1;
                v = 0;
            }

            timeOfChangeDirectionCounter = 0;
            timeOfTurning = Random.Range(1f, 3f);
        }
        else
        {
            timeOfChangeDirectionCounter += Time.fixedDeltaTime;
        }
    }
    
    // when hit the Grass, crush it
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Grass":
                Destroy(other.gameObject);
                break;
            case "Enemy":
                timeOfChangeDirectionCounter = timeOfTurning;
                break;
            default:
                break;
        }
    }
    
}
