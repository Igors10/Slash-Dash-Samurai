using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerSimple : MonoBehaviour
    {


    // Start is called before the first frame update
    void Start()
        {

        }

    

    // Update is called once per frame
    void FixedUpdate()
        {
        float speed = 5f;
        //transform.position += transform.up * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(transform.up * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        transform.Translate(transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        //Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        //rigidbody2D.AddForce(Vector2.up * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        //rigidbody2D.AddForce(Vector2.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        //rigidbody2D.
        }
    }
