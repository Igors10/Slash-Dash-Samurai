using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{

    [SerializeField] Rigidbody2D rbArrow;
    [SerializeField] float arrowSpeed;
    [SerializeField] float arrowLifeTime
        ;
    // Start is called before the first frame update
    void Start()
    {
        rbArrow = GetComponent<Rigidbody2D>();
        Destroy(gameObject, arrowLifeTime);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rbArrow.velocity = transform.up * arrowSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Samurai_state_script>().TakeDamage(1);
        }
    }




}
