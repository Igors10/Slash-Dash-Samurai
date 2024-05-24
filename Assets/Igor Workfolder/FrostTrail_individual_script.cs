using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FrostTrail_individual_script : MonoBehaviour
{
    Vector3 starting_scale;
    float melt_speed;
    float max_lifetime;
    float lifetime;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        transform.localScale *= FrostTrail.frosttrail_manager.GetComponent<FrostTrail>().size_modifier;
        starting_scale = transform.localScale;
        melt_speed = FrostTrail.frosttrail_manager.GetComponent<FrostTrail>().melting_speed;
        max_lifetime = FrostTrail.frosttrail_manager.GetComponent<FrostTrail>().lifetime;
        lifetime = max_lifetime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lifetime > 0) lifetime--;
        else if (transform.localScale.x > 0.3f)
        {
            transform.localScale -= new Vector3(melt_speed, melt_speed, 0);
        }
        else
        {
            FrostTrail.frosttrail_manager.GetComponent<FrostTrail>().DeactivatedFrostTrails.Enqueue(this.gameObject);
            this.gameObject.SetActive(false);
        }
            
    }

    public void Reuse(Transform spawn)
    {
        transform.position = spawn.position;
        transform.Translate(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        transform.localScale = starting_scale;
        lifetime = max_lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<NavMeshAgent>().speed = FrostTrail.frosttrail_manager.GetComponent<FrostTrail>().slowdown_effect;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<NavMeshAgent>().speed = 5;
        }
    }
}
