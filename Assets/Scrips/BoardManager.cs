﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance;

    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentCoin;
    public int xSize, ySize;
    public bool isShifting { get; set; }

    private GameObject[,] coins;


    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Vector2 offset = currentCoin.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }

    private void CreateInitialBoard(Vector2 offset)
    {
        coins = new GameObject[xSize, ySize];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        for(int i = 0; i<xSize; i++)
        {
            for(int j=0; j<ySize; j++)
            {
                GameObject newCoin = Instantiate(currentCoin, new Vector3(startX + (offset.x * i), startY + (offset.y * j), 0),currentCoin.transform.rotation);
                newCoin.name = string.Format("Coin[{0}],[{1}]", i, j);
                Sprite sprite = prefabs[Random.Range(0, prefabs.Count)];
                newCoin.GetComponent<SpriteRenderer>().sprite = sprite;
                newCoin.GetComponent<Coin>().id=-1;
                newCoin.transform.parent = this.transform;
                coins[i, j] = newCoin;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}