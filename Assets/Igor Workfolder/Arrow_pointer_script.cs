using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow_pointer_script : MonoBehaviour
{
    public Transform target;
    public Transform PlayerT;

    private void Awake()
        {
        this.gameObject.SetActive(false);   
        }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        transform.position = PlayerT.position;
    }
}
