using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selector : MonoBehaviour
{

    private int choice = 1;

    public Transform pos1;
    public Transform pos2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            choice = 1;
            transform.position = pos1.position;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            choice = 2;
            transform.position = pos2.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            switch (choice)
            {
                case 1:
                    SceneManager.LoadScene("Scenes/Game");
                    break;
                
            }
        }
    }
}
