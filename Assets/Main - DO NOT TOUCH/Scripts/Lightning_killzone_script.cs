using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_killzone_script : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyStatusMAIN>().EnemyDeath();
        }
    }
}
