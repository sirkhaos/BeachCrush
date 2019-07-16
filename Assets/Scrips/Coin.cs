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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
