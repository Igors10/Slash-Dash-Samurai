using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM : MonoBehaviour
{
    int current_m_in_list = 0;
    string[] greeting =
    { 
       "MESSAGE THAT PLAYS WHEN INTERACTING WITH DEMON AFTER LAUNCING THE GAME FOR THE FIRST TIME",
       //-----------------------------------------------------------------------------------------
       "Welcome to hell, Sweetie.",
       "You are here because you did something REAL bad in your life.",
       "What is it, let's see...",
       "Daaayum, you did slauthered half your village, didnt you?",
       "Well now you are bound to slay ghosts in here...",
       "FOREVER!!!",
       "Now go and slay some for me, lets see what you are made from."


       // -- end dialog --
    };

    string[] after_first_time =
    {
       //"MESSAGE THAT PLAYS AFTER YOU FINISH YOUR FIRST RUN",
       //------------------------
       "Hey good job, I wasnt completely bored",
       "Though you'd need to work on that slashing and dashing skill ngl",
       "You will have time to do that because thats what you will be doing...",
       "FOREVER!!!",
       "Did I already said that?, well anyway",
       "Go and do it again!! Yeeehaaawww!"
        // -- end dialog --
    };

    string[] third_time =
    {
       //"MESSAGE FOR COMING BACK 3RD TIME",
       //------------------------
       "You have probably noticed that I am enhancing you with magical abilities with each room cleared",
       "Dont even think of trying to find ultimate strategy and all that fuzz.",
       "Just have fun combining cool magic powers!",
       "Now go! Show me your inner artist, ",
       "the whole world is a canvas and katana is your brush!"
       
        // -- end dialog --
    };

    string[] random_advice1 =
    {
       //"RANDOM ADVICE MESSAGE 1",
       //------------------------
       "Dash! Slash! Slash and Dash!"
        // -- end dialog --
    };

    public string ProgressDialog(int id)
    {
        string m_to_return = "stop dialog";
        switch (MainContextMAIN.played_games_count)
        {
            case 0:
                if (greeting.Length > current_m_in_list) m_to_return = greeting[current_m_in_list];
                else current_m_in_list = -1;
                break;
            case 1:
                if (after_first_time.Length > current_m_in_list) m_to_return = after_first_time[current_m_in_list];
                else current_m_in_list = -1;
                break;
            case 2:
                if (third_time.Length > current_m_in_list) m_to_return = third_time[current_m_in_list];
                else current_m_in_list = -1;
                break;
            case 3:
                if (random_advice1.Length > current_m_in_list) m_to_return = random_advice1[current_m_in_list];
                else current_m_in_list = -1;
                break;


        }

        current_m_in_list++;
        
        return m_to_return;
    }
}
