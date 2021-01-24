using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int HP = 3;

    public int playerScore = 0;

    public bool isDead;

    public bool isDefeat = false;

    public bool isWin = false;

    public GameObject born;

    public AudioClip winAudio;

    public Text playerScoreText;
    public Text playerHPText;
    public GameObject defeatUI;

    private static PlayerManager instance;
    private bool returnHasBeenInvoked = false;

    public static PlayerManager Instance
    {
        get => instance;
        set => instance = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckWin())
        {
            if (!returnHasBeenInvoked)
            {
                Invoke("ReturnToMenu", 4f);
                AudioSource.PlayClipAtPoint(winAudio, transform.position);
                returnHasBeenInvoked = true;
            }

            return;
        }
        
        if (CheckDefeat())
        {
            if (!returnHasBeenInvoked)
            {
                Invoke("ReturnToMenu", 3f);
                returnHasBeenInvoked = true;
            }

            return;
        }
        
        if (isDead)
        {
            Recover();
        }

        playerScoreText.text = playerScore.ToString();
        playerHPText.text = HP.ToString();
    }

    private void Recover()
    {
        if (HP == 0)
        {
            // failed
            isDefeat = true;
            Invoke("ReturnToMenu", 3f);
        }
        else
        {
            HP--;
            GameObject go = Instantiate(born, new Vector3(-2, -8, 0), Quaternion.identity);
            go.GetComponent<Born>().isPlayerCreator = true;
            isDead = false;
        }
    }

    private bool CheckDefeat()
    {
        if (isDefeat)
        {
            defeatUI.SetActive(true);
            return true;
        }

        return false;
    }

    private bool CheckWin()
    {
        if (playerScore >= 8)
        {
            isWin = true;
            return true;
        }

        return false;
    }

    private void ReturnToMenu()
    {
        
        SceneManager.LoadScene("Scenes/menu");
        
    }
}
