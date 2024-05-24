using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_button_script : MonoBehaviour
{

    [SerializeField] GameObject RevertControlsB;
    [SerializeField] GameObject SelfDestructB;
    [SerializeField] GameObject ClearEnemiesB;
    [SerializeField] GameObject GetUpgradeB;
    [SerializeField] GameObject HomingMissleB;
    [SerializeField] GameObject HomingMissleUpgrade;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Revert Controls button
                if (UI.IsTouchOverUI(touch.position, RevertControlsB))
                {

                    MainContextMAIN.DashScript.HasReversedControls = MainContextMAIN.DashScript.HasReversedControls ? false : true;

                }

                // Kill the samurai button
                else if(UI.IsTouchOverUI(touch.position, SelfDestructB)) 
                {
                    MainContextMAIN.player.GetComponent<Samurai_state_scriptMAIN>().Death();
                }

                // Clear all the enemies button
                else if(UI.IsTouchOverUI(touch.position, ClearEnemiesB))
                {
                    GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

                    foreach (GameObject enemy in AllEnemies)
                    {
                        enemy.GetComponent<EnemyStatusMAIN>().EnemyDeath();
                    }
                }

                // Get a new upgrade button
                else if(UI.IsTouchOverUI(touch.position, GetUpgradeB))
                {
                    MainContextMAIN.upgrade_manager.GetComponent<Upgrade_managerMAIN>().GetNewUpgrades();
                }

                else if (UI.IsTouchOverUI(touch.position, HomingMissleB))
                {
                    HomingMissleUpgrade.GetComponent<HomingMissle_script>().ActivateSlot();
                }
            }
        }
    }
}
