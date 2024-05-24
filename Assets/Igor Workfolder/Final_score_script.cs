using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Final_score_script : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMPro.TextMeshProUGUI score;
    [SerializeField] TMPro.TextMeshProUGUI highscore;
    bool new_highscore = false;
    [SerializeField] GameObject NEW;
    float score_to_show;
    int blinking_timer = 0;
    [SerializeField] TMPro.TextMeshProUGUI demon_message;
    [SerializeField] int fake_score;
    

    void Start()
    {
        new_highscore = false;
        if (ScoreManagerScriptMAIN.playerScore > ScoreManagerScriptMAIN.highscore) NewHighScore();
        highscore.text = ScoreManagerScriptMAIN.highscore.ToString();

        score_to_show = (fake_score == 0) ? ScoreManagerScriptMAIN.playerScore : fake_score;
        score.text = score_to_show.ToString();

        GameOverMessage();



    }
    
    void GameOverMessage()
    {
        string message = "";

        // Default game over screen messages

        switch (score_to_show)
        {

            case 0:
                message = "0?? Really!?? Swipe on the screen to launch samurai to dash, its not that hard duh";
                break;

            case < 10:
                message = "I'm not supposed to say it, but you suck at this. Practice a little more to get better!";
                break;

            case < 30:
                message = "Is that all you got?, only " + score_to_show.ToString("F1") + " ?? My cat can do better";
                break;

            case < 60:
                message = "Not bad, not bad, but I'm sure you can do better";
                break;

            case 69:
                message = "Nice!";
                break;

            case < 90:
                message = "heyy that was entertaining to watch, keep it up!";
                break;

            default:

                message = "Nice try!";

                break;
        }

        // New Highscore messages

        if (new_highscore)
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    message = "Look at that! You got a new highscore! Ma boy!";
                    break;
                case 1:
                    message = "Now this is what I call a respectable result";
                    break;
                case 2:
                    message = "Wow, you did manage to surprise me after all, samurai. This new highscore is really impressive";
                    break;
                case 3:
                    message = "After so many attempts you did it!! New Highscooooore!";
                    break;
                case 4:
                    message = "I'm impressed, despite those horrible skills you did manage to get a new highcore.";
                    break;
            }

            
        }

        demon_message.text = message;
    }
    void NewHighScore()
    {
        ScoreManagerScriptMAIN.highscore = ScoreManagerScriptMAIN.playerScore;
        new_highscore = true;
    }

    void FixedUpdate()
    {
        if (new_highscore)
        {
            blinking_timer++;
            if (blinking_timer % 20 == 0)
            {
                bool active = (NEW.activeSelf) ? false : true;
                NEW.SetActive(active);
            }
        }
       
    }
}
