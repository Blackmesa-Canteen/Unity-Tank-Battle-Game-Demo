using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3;

    // up: 0, right: 1, down: 2, left: 3
    public Sprite[] tankSprites = new Sprite[4];

    public GameObject bulletPrefab;

    private SpriteRenderer sr;

    private Vector3 bulletEulerAngles;

    private float attackTimeCd = 0;

    private bool isDefended = true;

    public float attackCD = 0.4f;

    public GameObject explosionPrefab;
    
    public GameObject defendPrefab;

    // private AudioSource moveAudio;
    // private AudioSource stopAudio;
    //
    // public AudioClip moveSound;
    // public AudioClip stopSound;

    public AudioSource moveAudio;
    public AudioClip[] tankAudio;

    public float defendTimeVal = 3;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // defending time counter
        if (isDefended)
        {
            defendPrefab.SetActive(true);
            defendTimeVal -= Time.deltaTime;
            if (defendTimeVal <= 0)
            {
                isDefended = false;
                defendPrefab.SetActive(false);
            }
        }
        
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }
        
        // attack CD
        if (attackTimeCd >= attackCD)
        {
            Attack();
        }
        else
        {
            attackTimeCd += Time.deltaTime;
        }
    }
    
    // physic frame
    private void FixedUpdate()
    {
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }
        Move();
    }
    
    // attack with bullets
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, 
                transform.position, 
                Quaternion.Euler(transform.eulerAngles +
                                 bulletEulerAngles));
            attackTimeCd = 0;
        }
    }
    
    // moves tank
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

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
        
        if (Mathf.Abs(h) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        
        transform.Translate(Vector3.right * (h * moveSpeed * Time.deltaTime), Space.World);

        if (h != 0)
        {
            return;
        }
        
        float v = Input.GetAxisRaw("Vertical");
        
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
        
        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
    }
    
    // tank has been destroyed by a bullet
    private void Die()
    {
        if (isDefended)
        {
            return;
        }
        // die
        PlayerManager.Instance.isDead = true;
        
        // explode
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        // dead
        Destroy(gameObject);
    }

    // when hit the Grass, crush it
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Grass":
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

}
