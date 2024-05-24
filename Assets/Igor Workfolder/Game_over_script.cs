using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_over_script : MonoBehaviour
{
    [SerializeField] GameObject game_over_text;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (UI.IsTouchOverUI(touch.position, game_over_text))
            {
                SceneManager.LoadScene("StartingScene");

                ScoreManagerScriptMAIN.playerScore = 0;
                MainContextMAIN.LevelCount = 0;
            }
        }
    }
}
