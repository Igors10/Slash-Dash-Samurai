using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerScriptMAIN : MonoBehaviour
    {
    public TMPro.TextMeshProUGUI scoreDisplay;
    public TMPro.TextMeshProUGUI scoreMultiplierDisplay;
    [SerializeField] GameObject UpdateSystem;
    [SerializeField] GameObject TrianglePointer;
    [SerializeField] LevelHandlerMAIN LC;
    [SerializeField] float ScoreDecayRate = 1.0f;
    [SerializeField] float ScoreMultAfterKill = 1.0f;
    [SerializeField] float ExtraDecayRelatedToCurrentScore = 0.0f;
    [SerializeField] ScoreMultiplierJuice SMJ;

    float ScoreDecayTime = 0;
    public float CurrentScoreMultiplier = 1;

    public static float playerScore = 0;
    public static float highscore = 0;


    private void Awake()
        {
        MainContextMAIN.ScoreManager = this;
        }


    void Update()
        {
        //Debug.Log(CurrentScoreMultiplier + " | " + Time.realtimeSinceStartup);

        if (CurrentScoreMultiplier <= 1) { ScoreDecayTime = 0.0f; }
        else if (ScoreDecayTime > ScoreDecayRate) { CurrentScoreMultiplier -= ScoreMultAfterKill; ScoreDecayTime = 0.0f; }

        ScoreDecayTime += Time.deltaTime + CurrentScoreMultiplier*ExtraDecayRelatedToCurrentScore;

        scoreDisplay.text = playerScore.ToString();
        if (CurrentScoreMultiplier > 1) { scoreMultiplierDisplay.enabled = true; scoreMultiplierDisplay.text = CurrentScoreMultiplier.ToString() + "X"; }
        else { scoreMultiplierDisplay.enabled = false; }

        if ((MainContextMAIN.LevelHandler.CurrentLevel.SpawnerScript.SpawnedEnemyTracker == 0) && (MainContextMAIN.LevelHandler.CurrentLevel.SpawnerScript.SpawningStopped))
            {
            Debug.Log("RCCL: " + MainContextMAIN.LevelHandler.CurrentLevel + " | " + Time.realtimeSinceStartup);
            Debug.Log("---ROOM CLEARED | " + Time.realtimeSinceStartup + " ---");
            MainContextMAIN.player.GetComponent<Samurai_state_scriptMAIN>().Heal();
            UpdateSystem.GetComponent<Upgrade_managerMAIN>().GetNewUpgrades();
            TrianglePointer.SetActive(true);
            LC.CurrentLevel.LevelHasBeenCleared();


            MainContextMAIN.LevelCount++;
            }
        }

    public void UpdateScore(int scoreValue)
        {
        CurrentScoreMultiplier += ScoreMultAfterKill;
        ScoreDecayTime = 0.0f;
        playerScore = (scoreValue * CurrentScoreMultiplier) + playerScore;
        SMJ.HasScored();
        }
    }
