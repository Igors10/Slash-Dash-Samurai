using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow_pointer_scriptMAIN : MonoBehaviour
{
    public Transform target;
    public Transform PlayerT;
    public SpriteRenderer ArrowRenderer;
    const float DISTANCETHERESHOLD = 10.0f;

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

        if (((Vector2) PlayerT.position - (Vector2) target.position).magnitude < DISTANCETHERESHOLD) { ArrowRenderer.enabled = false; }
        else { ArrowRenderer.enabled = true; }
    }
}
