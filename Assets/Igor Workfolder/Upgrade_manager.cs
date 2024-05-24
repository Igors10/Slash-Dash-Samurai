using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_manager : MonoBehaviour
{
    [SerializeField] Sprite[] icons;
    [SerializeField] GameObject[] upgrade_cards; // 0- left, 1- center, 2- right
    [SerializeField] float filling_speed;
    [SerializeField] bool new_upgrade;
    [SerializeField] GameObject player;

    /*================* 
     * Upgrade list
    0) +1 HP
    1) +movement speed
    2) +ammo
    3) +movement_distance
    4) fire slash
    5) snow trail
    6) triple shot
     */

    // Start is called before the first frame update
    void Start()
    {
        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            upgrade_cards[a].GetComponent<Upgrade_UI>().upgrades_manager = this.gameObject;
            upgrade_cards[a].GetComponent<Upgrade_UI>().filling_speed = filling_speed;
            upgrade_cards[a].SetActive(false);
        }

        //GetNewUpgrades();
    }



    void NewUpgradeChoice(GameObject card, int upgrade_id)
    {
        card.SetActive(true);
        card.GetComponent<Upgrade_UI>().upgrade_type = upgrade_id;
        card.GetComponent<Upgrade_UI>().icon.GetComponent<Image>().sprite = icons[upgrade_id];
        Debug.Log(card.GetComponent<Upgrade_UI>().upgrade_type);
    }

    public void PickUpgrade(GameObject picked_upgrade)
    {
        Debug.Log(picked_upgrade.GetComponent<Upgrade_UI>().upgrade_type);
        player.GetComponent<Samurai_upgrade_script>().Upgrade(picked_upgrade.GetComponent<Upgrade_UI>().upgrade_type);
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

            else if (upgrade_id == upgrade_cards[a].GetComponent<Upgrade_UI>().upgrade_type) return true;
        }

        return false;
    }

    public void GetNewUpgrades()
    {
        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            upgrade_cards[a].GetComponent<Upgrade_UI>().upgrade_type = -1;
        }

        for (int a = 0; a < upgrade_cards.Length; a++)
        {
            int generated_upgrade_id;
            int bug_protection = 0;

            // Choosing an upgarde 
            do
            {
                
                bug_protection++;
               
                generated_upgrade_id = Random.Range(0, icons.Length - 3);
               
               
                
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
