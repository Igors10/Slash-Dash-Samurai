using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Homing_script : MonoBehaviour
{
    GameObject target;
    [SerializeField] float rotation_speed;
    [SerializeField] GameObject Bullet;
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = target.transform.position - transform.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Calculate the desired rotation using lerp to gradually rotate towards the target
            Quaternion desiredRotation = Quaternion.Euler(0, 0, angle);
            Bullet.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotation_speed * Time.deltaTime);
        }
       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && target == null)
        {
            target = col.gameObject;
        }
    }
}
