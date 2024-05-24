using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_bullets_script : MonoBehaviour
{

    [SerializeField] GameObject bullet;
    int bullet_timer = 0;
    public int bullet_rate;

    void FixedUpdate()
    {
        bullet_timer++;
        transform.Rotate(0, 0, 1);
        if (bullet_timer % bullet_rate == 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }

        transform.position = MainContextMAIN.player.transform.position;
    }
}
