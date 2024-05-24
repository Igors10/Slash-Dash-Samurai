using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Samurai_upgrade_script : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float speed_buff;
    [SerializeField] float distance_buff;

    [SerializeField] GameObject FireSlash;
    [SerializeField] DashScript2 Dashscript;

    int[] upgrade_lvl = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public void Upgrade(int upgrade_id)
    {
        upgrade_lvl[upgrade_id]++;

        switch (upgrade_id)
        {
                // + HP
            case 0:
                GetComponent<Samurai_state_script>().max_hp++;
                GetComponent<Samurai_state_script>().current_hp++;
                GetComponent<Samurai_state_script>().hp_bar.GetComponent<Health_bar_script>().PlusHeart();
                // Healing animation
                break;

                // Increased movement speed
            case 1:
                //GetComponent<Dash_script>().NonTimeSpeed += speed_buff;
                Dashscript.SpeedCoefficient += speed_buff;
                break;

            // Fire Slash
            case 2:
                //GetComponent<Dash_script>().slash_effect = FireSlash;
                Dashscript.FireSlashUpgrade();
                break;

                // Increased movement distance
            case 3:
                //GetComponent<Dash_script>().UpdateDistance(distance_buff);
                Dashscript.DistanceModifier += distance_buff;
                break;

                // Ammo capacity
            case 4:
                
                // add code that makes the collisions bigger
                break;

                // Snow Trail
            case 5:
                break;

                // Triple Shot
            case 6:
                break;

                // ** (Upgrades for later) **
            case 7:
                break;

            case 8:
                break;

            case 9:
                break;

            case 10:
                break;

            case 11:
                break;

            case 12:
                break;

        }

    }

}
