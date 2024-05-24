using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverse_control_button_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch position intersects with the target UI element
                if (UI.IsTouchOverUI(touch.position, this.gameObject)) Debug.Log("button is pressed");
            }
        }

    }

}
