using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramovement : MonoBehaviour
    {

    public Transform PlayerTransform;

    // Start is called before the first frame update
    void Start()
        {

        }

    // Update is called once per frame
    void Update()
        {
        transform.position = PlayerTransform.position;

        //float speed = 5f;
        //transform.position += transform.up * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        }
    }
