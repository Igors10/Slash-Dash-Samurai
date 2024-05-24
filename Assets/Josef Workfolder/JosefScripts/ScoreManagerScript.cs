using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreDisplay;
    [SerializeField] GameObject UpdateSystem;
    [SerializeField] GameObject TrianglePointer;
    [SerializeField] LevelHandler2 LC;

    public static int playerScore = 0; 
    public static int highscore = 0;

    public int roomsCleared;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = playerScore.ToString();
    }

    public void UpdateScore(int scoreValue)
    {
        playerScore = scoreValue + playerScore;

        //Debug.Log(playerScore);

        // Room cleared
        if (playerScore % 10 == 0)
        {
            UpdateSystem.GetComponent<Upgrade_manager>().GetNewUpgrades();
            TrianglePointer.SetActive(true);
            LC.CurrentLevel.LastWall.SetActive(false);
            roomsCleared++;
        }
    }
}
