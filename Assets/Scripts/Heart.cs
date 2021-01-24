using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private SpriteRenderer sr;

    //private bool isDestroyed = false;

    public Sprite brokenSprite;
    
    public GameObject explosionPrefab;

    public AudioClip destroiedSound;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Hit()
    {
        PlayerManager.Instance.isDefeat = true;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        sr.sprite = brokenSprite;
        AudioSource.PlayClipAtPoint(destroiedSound, transform.position);
    }
}
