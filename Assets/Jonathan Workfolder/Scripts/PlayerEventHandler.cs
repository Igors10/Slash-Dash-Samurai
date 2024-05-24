using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

//public enum PlayerEventId
//    {
//    START,
//    PREDICTEDMOVEMENT,
//    STARTEDMOVEMENT,
//    ENDEDMOVEMENT,

//    }

//public struct EventInformation
//    {
//    public Vector2 TargetPosition;
//    public Vector2 MovementOrigin;
//    public bool IsCurved;
//    public FunctionType CurrentFunctType;
//    public Vector2 ModifiedLineStart;

//    public Vector2 FinalPosition;
//    public bool HasFireSlash;
//    }


public class PlayerEventHandler : MonoBehaviour
    {
    [SerializeField] GameObject ForwardSlashPrefab;
    [SerializeField] GameObject CurveSlashPrefab;
    [SerializeField] GameObject ForwardPredictSquare;
    [SerializeField] GameObject CurvePredictSquare;
    [SerializeField] GameObject FireSlashPrefab;

    public DashScript2 PlayerReference;
    GameObject ForwardPredictSquareInstance;
    GameObject CurvePredictSquareInstance;

    private void Awake()
        {
        ForwardPredictSquareInstance = Instantiate(ForwardPredictSquare);
        CurvePredictSquareInstance = Instantiate(CurvePredictSquare);
        }


    public void HandleEvent(PlayerEventId ID, EventInformation EINF)
        {
        switch (ID)
            {
            case PlayerEventId.START:
                CurvePredictSquareInstance.SetActive(false);
                ForwardPredictSquareInstance.SetActive(false);
                break;


            case PlayerEventId.PREDICTEDMOVEMENT:
                if (EINF.CurrentFunctType == FunctionType.MODIFIEDLINE)
                    {
                    ForwardPredictSquareInstance.SetActive(true);
                    CurvePredictSquareInstance.SetActive(false);


                    Vector2 MVector = EINF.FinalPosition - EINF.ModifiedLineStart;

                    float Ax = (EINF.FinalPosition - EINF.ModifiedLineStart).x;
                    if (Ax == 0) { Ax = 0.1f; }

                    float LinePredictAngle = 90 + Mathf.Rad2Deg * (Mathf.Atan((EINF.FinalPosition - EINF.ModifiedLineStart).y / Ax));
                    Vector2 BoxPoint = EINF.FinalPosition + (MVector.normalized * PlayerReference.ForwardSlashDistance);
                    ForwardPredictSquareInstance.transform.position = BoxPoint;
                    if (LinePredictAngle == float.NaN) { LinePredictAngle = 0.0f; }
                    ForwardPredictSquareInstance.transform.eulerAngles = Vector3.forward * LinePredictAngle;
                    ForwardPredictSquareInstance.transform.localScale = PlayerReference.ForwardSlashSize / 2.0f;

                    break;
                    }


                if (EINF.IsCurved) 
                    {
                    CurvePredictSquareInstance.SetActive(true);
                    ForwardPredictSquareInstance.SetActive(false);

                    Vector2 Destination = EINF.TargetPosition + EINF.MovementOrigin;

                    Vector2 MVector = Destination - EINF.MovementOrigin;
                    Vector2 PMiddle = MVector / 2.0f;
                    float CurvePredictAngle = 90 + Mathf.Rad2Deg * (Mathf.Atan((Destination - EINF.MovementOrigin).y / (Destination - EINF.MovementOrigin).x));
                    CurvePredictSquareInstance.transform.position =  Destination - PMiddle;
                    CurvePredictSquareInstance.transform.eulerAngles = Vector3.forward * CurvePredictAngle;
                    CurvePredictSquareInstance.transform.localScale = new Vector3(PMiddle.magnitude/2.0f, MVector.magnitude, -5.0f);
                    }
                else
                    {
                    ForwardPredictSquareInstance.SetActive(true);
                    CurvePredictSquareInstance.SetActive(false);


                    Vector2 Destination = EINF.TargetPosition + EINF.MovementOrigin;

                    float LinePredictAngle = 90 + Mathf.Rad2Deg * (Mathf.Atan((Destination - EINF.MovementOrigin).y / (Destination - EINF.MovementOrigin).x));
                    Vector2 BoxPoint = Destination + (EINF.TargetPosition.normalized * PlayerReference.ForwardSlashDistance); 
                    ForwardPredictSquareInstance.transform.position = BoxPoint;
                    ForwardPredictSquareInstance.transform.eulerAngles = Vector3.forward * LinePredictAngle;
                    ForwardPredictSquareInstance.transform.localScale = PlayerReference.ForwardSlashSize / 2.0f;
                    }
                break;


            case PlayerEventId.STARTEDMOVEMENT:
                AudioManager.instance.PlaySFX("PlayerSlash");

                CurvePredictSquareInstance.SetActive(false);
                ForwardPredictSquareInstance.SetActive(false);
                break;


            case PlayerEventId.ENDEDMOVEMENT:
                if (EINF.CurrentFunctType == FunctionType.BEZIER)
                    {
                    Vector2 MHalfVector = EINF.TargetPosition / 2.0f;
                    GameObject SlashInstance;
                    if (EINF.HasFireSlash == false) { SlashInstance = Instantiate(CurveSlashPrefab); }
                    else { SlashInstance = Instantiate(FireSlashPrefab); }
                    SlashInstance.transform.position = MHalfVector + EINF.MovementOrigin;
                    SlashInstance.transform.rotation = PlayerReference.transform.rotation * Quaternion.AngleAxis(90, Vector3.forward);
                    SlashInstance.transform.localScale = new Vector3(6.0f, 3.0f, 1.0f);
                    }
                else if (EINF.CurrentFunctType == FunctionType.MODIFIEDLINE)
                    {
                    Vector2 MMovement = EINF.FinalPosition - EINF.MovementOrigin;
                    Vector2 BoxPoint = EINF.FinalPosition + (MMovement.normalized * PlayerReference.ForwardSlashDistance);
                    GameObject SlashInstance;
                    if (EINF.HasFireSlash == false) { SlashInstance = Instantiate(CurveSlashPrefab); }
                    else { SlashInstance = Instantiate(FireSlashPrefab); }
                    SlashInstance.transform.position = BoxPoint;
                    SlashInstance.transform.rotation = PlayerReference.transform.rotation;
                    }
                else
                    {
                    Vector2 Destination = EINF.TargetPosition + EINF.MovementOrigin;
                    Vector2 BoxPoint = Destination + (EINF.TargetPosition * PlayerReference.ForwardSlashDistance);
                    GameObject SlashInstance;
                    if (EINF.HasFireSlash == false) { SlashInstance = Instantiate(CurveSlashPrefab); }
                    else { SlashInstance = Instantiate(FireSlashPrefab); }
                    SlashInstance.transform.position = BoxPoint;
                    SlashInstance.transform.rotation = PlayerReference.transform.rotation;
                    }
                break;
            }


        }
    }
