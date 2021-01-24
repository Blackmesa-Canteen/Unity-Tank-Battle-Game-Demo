using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapCreator : MonoBehaviour
{

    public int enermySalvo = 5;
        
    // init NPC objects on the map
    // 0: Home, 1: wall, 2: Barrier, 3: Born, 4: river
    // 5: grass, 6: boundary of the map
    public GameObject[] items;
    
    //private List<Vector3> itemsCreated = new List<Vector3>();
    private Hashtable itemsCreated = new Hashtable();

    private void Awake()
    {
        initMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Not secure, can create item at the same position
    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        
        if (!itemsCreated.ContainsKey(createPosition))
        {
            itemsCreated.Add(createPosition, itemGo);
        }
    }

    private Vector3 CreateRandomPosition()
    {
        // does not generate x = -10, 10 && y = -8, 8
        // this is to guarantee that there is always
        // a way to the other side of the map
        for (int securityCounter = 999; securityCounter >= 0; securityCounter--)
        {
            Vector3 createPosition = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            if (!itemsCreated.ContainsKey(createPosition))
            {
                return createPosition;
            }
            
        }

        return new Vector3(0, 0, 0);
    }

    private void generateAnEnemy()
    {
        if (PlayerManager.Instance.isDefeat || enermySalvo <= 0)
        {
            return;
        }

        enermySalvo--;
        
        int num = Random.Range(0, 3);
        Vector3 enemyPos;
        if (num == 0)
        {
            enemyPos = new Vector3(-10, 8, 0);
        }
        else if (num == 1)
        {
            enemyPos = new Vector3(0, 8, 0);
        }
        else
        {
            enemyPos = new Vector3(10, 8, 0);
        }
        
        CreateItem(items[3], enemyPos, Quaternion.identity);
    }

    private void initMap()
    {
        InitHome();

        InitBorder();

        InitPlayer();

        InitEnemies();

        InitNPCObjects();

        // then use the API to generate enemy periodically
        InvokeRepeating("generateAnEnemy", 4, 5);
    }

    private void InitNPCObjects()
    {
        // init objects    
        for (int i = 0; i < 20; i++)
        {
            CreateItem(items[1], CreateRandomPosition(), Quaternion.identity);
            CreateItem(items[2], CreateRandomPosition(), Quaternion.identity);
            CreateItem(items[4], CreateRandomPosition(), Quaternion.identity);
            CreateItem(items[1], CreateRandomPosition(), Quaternion.identity);
            CreateItem(items[5], CreateRandomPosition(), Quaternion.identity);
            CreateItem(items[1], CreateRandomPosition(), Quaternion.identity);
        }
    }

    private void InitEnemies()
    {
        // init enermies
        CreateItem(items[3], new Vector3(-10, 8, 0), Quaternion.identity);
        CreateItem(items[3], new Vector3(0, 8, 0), Quaternion.identity);
        CreateItem(items[3], new Vector3(10, 8, 0), Quaternion.identity);
    }

    private void InitPlayer()
    {
        // init player
        GameObject go = Instantiate(items[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<Born>().isPlayerCreator = true;
        itemsCreated.Add(go.transform.position, go);
    }

    private void InitBorder()
    {
        // init map border
        for (int x = -11; x <= 11; x++)
        {
            CreateItem(items[6], new Vector3(x, 9, 0), Quaternion.identity);
            CreateItem(items[6], new Vector3(x, -9, 0), Quaternion.identity);
        }

        for (int y = -8; y <= 8; y++)
        {
            CreateItem(items[6], new Vector3(-11, y, 0), Quaternion.identity);
            CreateItem(items[6], new Vector3(11, y, 0), Quaternion.identity);
        }
    }

    private void InitHome()
    {
        // init home
        CreateItem(items[0], new Vector3(0, -8, 0), Quaternion.identity);
        CreateItem(items[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItem(items[1], new Vector3(1, -8, 0), Quaternion.identity);
        for (int i = -1; i < 2; i++)
        {
            CreateItem(items[1], new Vector3(i, -7, 0), Quaternion.identity);
        }
    }
}
