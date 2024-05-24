using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_NPC_script : MonoBehaviour
{
    [SerializeField] GameObject dialog_window;
    [SerializeField] TMPro.TextMeshProUGUI demon_text;
   
    int current_dialog_id = 0;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && dialog_window.activeSelf == true)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ProgressDialog();
            }
        }
    }

    void ProgressDialog()
    {
        string new_demon_text = GetComponent<DM>().ProgressDialog(current_dialog_id);
        if (new_demon_text == "stop dialog") EndDialog();
        else demon_text.text = new_demon_text;
    }

    void ChooseDialog()
    {
        switch (MainContextMAIN.played_games_count)
        {
            case 0:
                current_dialog_id = 1;
                break;
            case 1:
                current_dialog_id = 2;
                break;
            case 2:
                current_dialog_id = 3;
                break;
            default:
                current_dialog_id = 4;
                break;

        }

        
    }
    void StartDialog()
    {
        dialog_window.SetActive(true);

        MainContextMAIN.player.GetComponent<Samurai_state_scriptMAIN>().dead = true;

        ProgressDialog();
    }
    
    void EndDialog()
    {
        this.gameObject.SetActive(false);
        dialog_window.SetActive(false);
        MainContextMAIN.player.GetComponent<Samurai_state_scriptMAIN>().dead = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartDialog();
        }
    }

   
}
