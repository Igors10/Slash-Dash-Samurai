using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle_script : MonoBehaviour
{
    [SerializeField] GameObject[] soul_slots;
    [SerializeField] GameObject homing_bullet;
    public int missle_number;
    int soul_timer = 0;
    [SerializeField] int soul_remove_time;

    private void Start()
    {
        MainContextMAIN.homing_missles = this.gameObject;
    }
    void FixedUpdate()
    {
        transform.position = MainContextMAIN.player.transform.position;

        soul_timer++;
        if (soul_timer % soul_remove_time == 0) DeactivateSlot();
    }
    void ShootMissles()
    { 
        while (soul_slots[0].activeSelf)
        {
            DeactivateSlot();
        }



        for (int a = 0; a < missle_number; a++)
        {
            GameObject missle = Instantiate(homing_bullet, transform.position, Quaternion.identity);
            missle.transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
        }

    }

    void DeactivateSlot()
    {
        for (int a = soul_slots.Length - 1; a > -1; a--)
        {
            if (soul_slots[a].activeSelf)
            {
                soul_slots[a].SetActive(false);
                break;
            }
        }
    }
    public void ActivateSlot()
    {
        soul_timer = 0;
        for (int a = 0; a < soul_slots.Length; a++)
        {
            if (soul_slots[a].activeSelf == false)
            {
                
                if (a == soul_slots.Length - 1) ShootMissles();
                else
                {
                    soul_slots[a].SetActive(true);
                }
                break;
            }
            
        }
    }
}
