using System.Collections;
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
    }

    private void CreateInitialBoard(Vector2 offset)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
