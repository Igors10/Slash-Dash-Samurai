using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    

    public static bool IsTouchOverUI(Vector2 touchPosition, GameObject targetElement)
    {
        // Create a pointer event for the current touch position
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the pointer event
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Check if the raycast hits the target UI element
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == targetElement)
            {
                //Debug.Log("touched a card");

                return true;
            }
        }

        return false;
    }
}
