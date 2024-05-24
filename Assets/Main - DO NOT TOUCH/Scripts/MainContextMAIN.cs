using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainContextMAIN : MonoBehaviour
{
    public static GameObject player;
    public static GameObject arrow;
    public static GameObject upgrade_manager;
    public static GameObject homing_missles;
    public static DashScriptMAIN DashScript;
    public static LevelHandlerMAIN LevelHandler;
    public static int LevelCount;

    public static int played_games_count = 0;

    public static ScoreManagerScriptMAIN ScoreManager;


    public static int timer;

    private void FixedUpdate()
    {
        timer++;
    }


}
