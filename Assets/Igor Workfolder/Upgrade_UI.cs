using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;

public class Upgrade_UI : MonoBehaviour
{
    public GameObject targetUIElement; // Reference to the UI element to be touched
    [SerializeField] Slider slider;
    [SerializeField] Image card_sprite;
    [SerializeField] Sprite red_card;
    [SerializeField] Sprite yellow_card;
    public GameObject icon;
    public float filling_speed;
    public int upgrade_type;
    public GameObject upgrades_manager;
    

    private bool isTouchingTargetUI = false;

    private void Start()
    {
        slider = GetComponent<Slider>();
        targetUIElement = this.gameObject;
        slider.value = 0;
        
    }

    void SelectUpgrade()
    {
        card_sprite.sprite = red_card;
        isTouchingTargetUI = false;
        slider.value = 0;
        upgrades_manager.GetComponent<Upgrade_manager>().PickUpgrade(this.gameObject);
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 

            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch position intersects with the target UI element
                isTouchingTargetUI = UI.IsTouchOverUI(touch.position, targetUIElement);
                if (isTouchingTargetUI)
                {
                    slider.value += 0.35f;
                    card_sprite.sprite = yellow_card;
                }
                   
            }

            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && (isTouchingTargetUI))
            {
                isTouchingTargetUI = false;
                slider.value = 0;
                card_sprite.sprite = red_card;
            }
        }

        if (isTouchingTargetUI)
        {
            slider.value += 0.001f * filling_speed;
            if (slider.value >= 1.0f) SelectUpgrade();
        }
    }

    
}
