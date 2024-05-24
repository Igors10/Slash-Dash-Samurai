using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreMultiplierJuice : MonoBehaviour
{
    RectTransform rectTransform;
    TextMeshProUGUI textMeshProUGUI;
    ScoreManagerScriptMAIN scoreManagerScript;
    [SerializeField] float BecomeRedAtThisPoint = 5.0f;
    [SerializeField] float ReboundHasteT = 0.1f;
    [SerializeField] float ReboundHaste = 0.1f;
    [SerializeField] float SizeIncreaseOnScore = 2.0f;

    float SizeIncreaseCounter = 1.0f;

    private void Start()
        {
        rectTransform = GetComponent<RectTransform>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        scoreManagerScript = MainContextMAIN.ScoreManager;
        }

    // Update is called once per frame
    void Update()
    {
        //textMeshProUGUI.colorGradient
        textMeshProUGUI.color = Color.Lerp(Color.white, Color.red, scoreManagerScript.CurrentScoreMultiplier / BecomeRedAtThisPoint); //Color.white;
        rectTransform.rotation = Quaternion.AngleAxis(Mathf.Sin(Time.realtimeSinceStartup) * 10.0f, Vector3.forward);
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, new Vector3 (1.0f, 1.0f, 0.0f) * Mathf.Clamp(((Mathf.Sin(Time.realtimeSinceStartup)/8.0f) + 1.0f), 0.0f, 10.0f) * SizeIncreaseCounter, ReboundHasteT);

        if (SizeIncreaseCounter - ReboundHaste < 1.0f) { SizeIncreaseCounter = 1.0f; } 
        else { SizeIncreaseCounter -= ReboundHaste; }
    }


    public void HasScored()
        {
        SizeIncreaseCounter = SizeIncreaseOnScore;
        }
}
