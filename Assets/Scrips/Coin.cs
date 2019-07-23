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
                    SwapSprite(previousSelected);
                    previousSelected.DeselectCoin();
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
}
