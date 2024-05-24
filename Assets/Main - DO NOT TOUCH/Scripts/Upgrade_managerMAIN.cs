using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_managerMAIN : MonoBehaviour
{
    [SerializeField] Sprite[] icons;
    public GameObject[] upgrade_cards; // 0- left, 1- center, 2- right
    [SerializeField] float filling_speed;
    [SerializeField] bool new_upgrade;
    [SerializeField] GameObject player;
    


    private int SafetySwitch = 0;
    private const int SAFETYLIMIT = 10000;

    /*================* 
     * Upgrade list
    0) +1 HP
    1) +movement speed
    2) fire slash
    3) +movement_distance
    4) ice trail
    5) 360 bullets
 
     */

    // Start is called before the first frame update
    void Start()
    {
        MainContextMAIN.upgrade_manager = this.gameObject;

        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            upgrade_cards[a].GetComponent<Upgrade_UIMAIN>().upgrades_manager = this.gameObject;
            upgrade_cards[a].GetComponent<Upgrade_UIMAIN>().filling_speed = filling_speed;
            upgrade_cards[a].SetActive(false);
        }

        //GetNewUpgrades();
    }

    void UpgradeText(GameObject card, int upgrade_id)
    {
        string desc_text = "";
        string t_text = "";
        switch (upgrade_id)
        {                           // Make them say different things on higher levels
            case 0:
                desc_text = "Get a life";
                t_text = "1 UP";
                //t_text = "1 UP (" + MainContextMAIN.player.GetComponent<Samurai_upgrade_scriptMAIN>().upgrade_lvl[upgrade_id] + 1 + " lvl)";
                break;
            case 1:
                desc_text = "Move faster then wind";
                t_text = "Speed up";
                break;
            case 2:
                desc_text = "Enhance slash with fire";
                t_text = "Fire Slash";
                break;
            case 3:
                desc_text = "Dash reaches further";
                t_text = "Movement Distance";
                break;
            case 4:
                desc_text = "Create frost trail that freezes enemies on contact";
                t_text = "Frostfeet";
                break;
            case 5:
                desc_text = "Passivly shoot projectiles";
                t_text = "Kunai";
                break;
            case 6:
                desc_text = "Summon winter upon taking damage";
                t_text = "Ice ring";
                break;
            case 7:
                desc_text = "Gather enemy souls to shoot homing missles (soul are hard to hold)";
                t_text = "Soul missles";
                break;
            case 8:
                desc_text = "Small zap zap nearby enemies when dashing";
                t_text = "Energy surge";
                break;
            case 9:
                desc_text = "Perform 3 successful attacks in a row to create big zap zap";
                t_text = "Lightning strike";
                break;

        }
        int upgrade_level = MainContextMAIN.player.GetComponent<Samurai_upgrade_scriptMAIN>().upgrade_lvl[upgrade_id] + 1;
        t_text += " (" + upgrade_level + " lvl)";
        card.GetComponent<Upgrade_UIMAIN>().title_text.text = t_text;
        card.GetComponent<Upgrade_UIMAIN>().description_text.text = desc_text;
    }

    public static bool IsUpgradePicking()
    {
        bool picking = (MainContextMAIN.upgrade_manager.GetComponent<Upgrade_managerMAIN>().upgrade_cards[0].activeSelf) ? true : false;

        return picking;
    }
    void NewUpgradeChoice(GameObject card, int upgrade_id)
    {
        card.SetActive(true);
        card.GetComponent<Upgrade_UIMAIN>().upgrade_type = upgrade_id;
        card.GetComponent<Upgrade_UIMAIN>().icon.GetComponent<Image>().sprite = icons[upgrade_id];
        UpgradeText(card, upgrade_id);

        //Debug.Log(card.GetComponent<Upgrade_UIMAIN>().upgrade_type);
    }

    public void PickUpgrade(GameObject picked_upgrade)
    {
        //Debug.Log(picked_upgrade.GetComponent<Upgrade_UIMAIN>().upgrade_type);
        player.GetComponent<Samurai_upgrade_scriptMAIN>().Upgrade(picked_upgrade.GetComponent<Upgrade_UIMAIN>().upgrade_type);
        // Removing upgrade cards from the screen
        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            upgrade_cards[a].SetActive(false);
        }
        // ======================================
    }

    bool DublicateUpgradeCheck(int card_id, int upgrade_id)
    {
        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            // upgrade ban fix, remove it later
            if (upgrade_cards[card_id] == upgrade_cards[a]) continue;

            else if (upgrade_id == upgrade_cards[a].GetComponent<Upgrade_UIMAIN>().upgrade_type) return true;
        }

        return false;
    }

    public void GetNewUpgrades()
    {
        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            upgrade_cards[a].GetComponent<Upgrade_UIMAIN>().upgrade_type = -1;
        }

        for (int a = 0; a < upgrade_cards.Length - 1; a++)
        {
            int generated_upgrade_id;
            int bug_protection = 0;

            SafetySwitch = 0;
            // Choosing an upgarde 
            do
            {
                SafetySwitch++;
                bug_protection++;
               
                generated_upgrade_id = Random.Range(0, icons.Length); // upgrade count

                if (SafetySwitch > SAFETYLIMIT) { Debug.Log("UNSAFEWHILELOOP 242"); break; }
                }
            while (DublicateUpgradeCheck(a, generated_upgrade_id) || bug_protection > 5); // Making sure it doesnt repeat

            NewUpgradeChoice(upgrade_cards[a], generated_upgrade_id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (new_upgrade)
        {
            new_upgrade = false;

            if (upgrade_cards[0].activeSelf == false) GetNewUpgrades(); 
        }

        
        
    }
}
