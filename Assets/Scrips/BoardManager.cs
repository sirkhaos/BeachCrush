using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance;
    public const int minCoinsToMatch = 2;

    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentCoin;
    public int xSize, ySize;
    public bool isShifting { get; set; }

    private GameObject[,] coins;
    private Coin selectedCoin;


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

        int idX = -1;
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                GameObject newCoin = Instantiate(currentCoin, new Vector3(startX + (offset.x * i), startY + (offset.y * j), 0), currentCoin.transform.rotation);
                newCoin.name = string.Format("Coin[{0}],[{1}]", i, j);
                do
                {
                    idX = Random.Range(0, prefabs.Count);
                } while ((i > 0 && idX == coins[i - 1, j].GetComponent<Coin>().id) || (j > 0 && idX == coins[i, j - 1].GetComponent<Coin>().id));
                Sprite sprite = prefabs[idX];
                newCoin.GetComponent<SpriteRenderer>().sprite = sprite;
                newCoin.GetComponent<Coin>().id = idX;
                newCoin.transform.parent = this.transform;
                coins[i, j] = newCoin;
            }
        }
    }

    public IEnumerator FindNullCoins()
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                if (coins[i, j].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(MakeCoinsFall(i, j));
                    break;
                }
            }
        }
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                coins[i, j].GetComponent<Coin>().FindAllMatches();
            }
        }
    }

    private IEnumerator MakeCoinsFall(int x, int yStart, float shiftDelay=0.05f)
    {
        isShifting = true;

        List<SpriteRenderer> renderes = new List<SpriteRenderer>();
        int nullcoins = 0;

        for (int y = yStart; y < ySize; y++)
        {
            SpriteRenderer spriteRenderer = coins[x, y].GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == null)
            {
                nullcoins++;
            }
            renderes.Add(spriteRenderer);
        }
        for(int i = 0; i < nullcoins; i++)
        {
            yield return new WaitForSeconds(shiftDelay);
            for(int j = 0; j < renderes.Count - 1; j++)
            {
                renderes[j].sprite = renderes[j + 1].sprite;
                renderes[j + 1].sprite = GetNewCoin(x,ySize-1);
            }
        }
        isShifting = false;
    }

    private Sprite GetNewCoin(int x, int y)
    {
        List<Sprite> possibleCoins = new List<Sprite>();
        possibleCoins.AddRange(prefabs);
        if (x > 0)
        {
            possibleCoins.Remove(coins[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        /*else
        {*/
            if (x < xSize - 1)
            {
                possibleCoins.Remove(coins[x + 1, y].GetComponent<SpriteRenderer>().sprite);
            }
            /*else
            {*/
                if (y > 0)
                {
                    possibleCoins.Remove(coins[x, y - 1].GetComponent<SpriteRenderer>().sprite);
                }
            //}
        //}
        return possibleCoins[Random.Range(0, possibleCoins.Count)];
    }
}
