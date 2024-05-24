using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_script : MonoBehaviour
{
    [SerializeField] float bullet_speed;
    void FixedUpdate()
    {
        transform.Translate(0, bullet_speed, 0);

        if (Vector3.Distance(transform.position, MainContextMAIN.player.transform.position) > 30.0f) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyStatusMAIN>().EnemyDeath();
            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "COLLISION")
        {
            Destroy(this.gameObject);
        }
    }
}
