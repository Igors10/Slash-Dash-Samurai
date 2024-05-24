using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Samurai_upgrade_scriptMAIN : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float speed_buff;
    [SerializeField] float distance_buff;
    [SerializeField] float ice_trail_size_buff;
    [SerializeField] float speed_trail_buff;
    [SerializeField] int passive_bullet_rate_buff;
    [SerializeField] float frost_ring_size_buff;
    [SerializeField] int homing_missles_count_buff;
    [SerializeField] int energy_surge_size_buff;
    [SerializeField] int lightning_number_buff;

    [SerializeField] GameObject FireSlash;
    [SerializeField] GameObject LightningManager;
    [SerializeField] GameObject IceTrail;
    [SerializeField] GameObject BulletAround;
    [SerializeField] GameObject FrostRing;
    [SerializeField] GameObject HomingMissles;
    public bool energy_surge_on = false;
    public GameObject Energy_surge;
    GameObject IceTrailInstance;
    GameObject HomingMisslesInstance;
    GameObject BulletAroundInstance;
    public GameObject LightningManagerInstance;
    [SerializeField] DashScriptMAIN Dashscript;

    public int[] upgrade_lvl = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public void Upgrade(int upgrade_id)
    {
        upgrade_lvl[upgrade_id]++;

        switch (upgrade_id)
        {
                // + HP
            case 0:
                GetComponent<Samurai_state_scriptMAIN>().max_hp++;
                GetComponent<Samurai_state_scriptMAIN>().current_hp++;
                GetComponent<Samurai_state_scriptMAIN>().hp_bar.GetComponent<Health_bar_script>().PlusHeart();
                // Healing animation
                break;

                // Increased movement speed
            case 1:
                //GetComponent<Dash_script>().NonTimeSpeed += speed_buff;
                Dashscript.SpeedCoefficient += speed_buff;
                //MainContextMAIN.player.GetComponent<TrailRenderer>().time += speed_trail_buff;
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

                // Snow trail
            case 4:
                if (upgrade_lvl[upgrade_id] == 1) IceTrailInstance = Instantiate(IceTrail, MainContextMAIN.player.transform);
                else
                {
                    IceTrailInstance.GetComponent<FrostTrail>().size_modifier += ice_trail_size_buff;
                    //IceTrailInstance.GetComponent<FrostTrail>().lifetime += ice_trail_size_modifier;
                }
                // add code that makes the collisions bigger
                break;

                // 360 bullets
            case 5:
                if (upgrade_lvl[upgrade_id] == 1) BulletAroundInstance = Instantiate(BulletAround, MainContextMAIN.player.transform);
                else
                {
                    BulletAroundInstance.GetComponent<Passive_bullets_script>().bullet_rate -= passive_bullet_rate_buff;
                    //IceTrailInstance.GetComponent<FrostTrail>().lifetime += ice_trail_size_modifier;
                }
                break;

                // Frost ring
            case 6:
                if (upgrade_lvl[upgrade_id] == 1) MainContextMAIN.player.GetComponent<Samurai_state_scriptMAIN>().Frostring = FrostRing;
                else
                {
                    MainContextMAIN.player.GetComponent<Samurai_state_scriptMAIN>().Frostring.GetComponent<Frostring_script>().scale_modifier += frost_ring_size_buff;
                }
                break;

                // Homing missles
            case 7:
                if (upgrade_lvl[upgrade_id] == 1) HomingMisslesInstance = Instantiate(HomingMissles);
                else
                {
                    HomingMisslesInstance.GetComponent<HomingMissle_script>().missle_number += homing_missles_count_buff;
                }
                break;

            case 8:
                if (upgrade_lvl[upgrade_id] == 1)
                
                    {
                        energy_surge_on = true;
                    Energy_surge = Instantiate(Energy_surge, transform.position, Quaternion.identity);
                    }
                else
                    {
                        Energy_surge.GetComponent<Energy_surge_script>().scale_modifier = energy_surge_size_buff;
                        Energy_surge.GetComponent<Energy_surge_script>().IncreaseSize();
                    }
                
                break;

            case 9:
                if (upgrade_lvl[upgrade_id] == 1)
                {
                    LightningManagerInstance = Instantiate(LightningManager, transform.position, Quaternion.identity);
                }
                else
                {
                    LightningManagerInstance.GetComponent<Lightning_manager_script>().lightning_count += lightning_number_buff;
                }
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
