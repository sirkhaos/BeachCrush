using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public static GUIManager sharedInstance;

    private int movesCounter;
    private int score;

    public Text moveText, scoreText;
    //variable auto computada
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = "Score: " + score;
        }
    }

    public int MovesCounter
    {
        get
        {
            return movesCounter;
        }
        set
        {
            movesCounter = value;
            moveText.text = "Moves: " + movesCounter;
            if (movesCounter <= 0)
            {
                movesCounter = 0;
                StartCoroutine(GameOver());
            }
        }
    }

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
        movesCounter = 30;
        moveText.text = "Moves: " + movesCounter;
        scoreText.text = "Score: " + score;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);
        yield return new WaitForSeconds(0.25f);
        //TODO: llamado de la pantalla de gameoverS

    }
}
