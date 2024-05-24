using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Samurai_state_scriptMAIN>().TakeDamage(1);
        }
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(.3f);

        Destroy(gameObject);
    }
}
