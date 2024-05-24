using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Orientation_script : MonoBehaviour
{
    // Element order
    // 0) HP
    // 1) Score
    // 2) Upgrade card 1 
    // 3) Upgrade card 2
    // 4) score multiplier

    [SerializeField] GameObject[] UI_elements;
    [SerializeField] GameObject[] UI_portrait_positions;
    [SerializeField] GameObject[] UI_landscape_positions;

    private ScreenOrientation currentOrientation;
    
    void Start()
    {
        currentOrientation = Screen.orientation;
        OrientationChange(currentOrientation);
    }
   
    void Update()
    {
        if (Screen.orientation != currentOrientation) // doesnt recognize when you change orientation
        {
            currentOrientation = Screen.orientation;
            OrientationChange(currentOrientation);
        }
    }
    void OrientationChange(ScreenOrientation new_orientation)
    {
        Debug.Log("orientation chaaaaaaange");
        if (new_orientation == ScreenOrientation.Portrait || new_orientation == ScreenOrientation.PortraitUpsideDown)
        {
            AdjustUI(UI_portrait_positions);
        }
        else
        {
            AdjustUI(UI_landscape_positions);
        }
    }
    void AdjustUI(GameObject[] new_positions)
    {
        for (int a = 0; a < UI_elements.Length; a++)
        {
            UI_elements[a].transform.position = new_positions[a].transform.position;
        }
    }
}
