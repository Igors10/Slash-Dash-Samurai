using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starting_scene_script : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMPro.TextMeshProUGUI highscore;
    [SerializeField] GameObject starting_zone;
    void Start()
    {
        highscore.text = ScoreManagerScriptMAIN.highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainContextMAIN.player.transform.position.y > starting_zone.transform.position.y)
        {
            MainContextMAIN.played_games_count++;
            SceneManager.LoadScene("Main Scene");
        }
    }
}
