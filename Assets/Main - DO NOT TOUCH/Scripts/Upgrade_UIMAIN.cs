using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;

public class Upgrade_UIMAIN : MonoBehaviour
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

    public TMPro.TextMeshProUGUI description_text;
    public TMPro.TextMeshProUGUI title_text;


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
        upgrades_manager.GetComponent<Upgrade_managerMAIN>().PickUpgrade(this.gameObject);
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 

            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch position intersects with the target UI element
                isTouchingTargetUI = IsTouchOverUI(touch.position);
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

    bool IsTouchOverUI(Vector2 touchPosition)
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
            if (result.gameObject == targetUIElement)
            {
                //Debug.Log("touched a card");
                card_sprite.sprite = yellow_card;
                return true;
            }
        }

        return false;
    }
}
