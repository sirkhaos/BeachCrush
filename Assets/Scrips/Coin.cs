using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Coin previousSelected = null;

    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;
    private Vector2[] direcciones = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    public int id;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SelectCoin()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected = gameObject.GetComponent<Coin>();
    }

    private void DeselectCoin()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelected = null;
    }

    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == null || BoardManager.sharedInstance.isShifting)
        {
            return;
        }
        else
        {
            if (isSelected)
            {
                DeselectCoin();
            }
            else
            {
                if (previousSelected == null)
                {
                    SelectCoin();
                }
                else
                {
                    if (CanSwipe())
                    {
                        SwapSprite(previousSelected);
                        previousSelected.FindAllMatches();
                        previousSelected.DeselectCoin();
                        FindAllMatches();
                    }
                    else
                    {
                        previousSelected.DeselectCoin();
                        SelectCoin();
                    }
                }
            }
        }
    }

    public void SwapSprite(Coin newCoin)
    {
        if (spriteRenderer.sprite == newCoin.GetComponent<SpriteRenderer>().sprite) 
        {
            return;
        }

            Sprite temp = this.spriteRenderer.sprite;
            this.spriteRenderer.sprite = newCoin.spriteRenderer.sprite;
            newCoin.spriteRenderer.sprite = temp;
            int nid = this.id;
            this.id = newCoin.id;
            newCoin.id = nid;

    }

    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    private List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();

        foreach(Vector2 direction in direcciones)
        {
            neighbors.Add(GetNeighbor(direction));

        }

        return neighbors;
    }

    private bool CanSwipe()
    {
        return GetAllNeighbors().Contains(previousSelected.gameObject);
    }

    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingCoins = new List<GameObject>();

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite)
        {
            matchingCoins.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }
        return matchingCoins;
    }

    private bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchingCoins = new List<GameObject>();
        foreach(Vector2 direction in directions)
        {
            matchingCoins.AddRange(FindMatch(direction));
        }
        if (matchingCoins.Count >= BoardManager.minCoinsToMatch)
        {
            foreach(GameObject coin in matchingCoins)
            {
                coin.GetComponent<SpriteRenderer>().sprite = null;
            }
            return true;
        }
        else
        {
            return false;
        }

    }

    public void FindAllMatches()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }

        bool hMatch = ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        bool vMatch = ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });

        if (hMatch || vMatch)
        {
            spriteRenderer.sprite = null;
            StopCoroutine(BoardManager.sharedInstance.FindNullCoins());
            StartCoroutine(BoardManager.sharedInstance.FindNullCoins());
        }
    }


}