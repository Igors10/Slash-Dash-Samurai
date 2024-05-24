using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_radar_script : MonoBehaviour
{
    public List<GameObject> nearby_foes = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            nearby_foes.Add(col.gameObject);
            //col.gameObject.GetComponent<EnemyStatusMAIN>().EnemyDeath();
        }
    }
}
