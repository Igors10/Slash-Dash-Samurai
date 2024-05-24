using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

/*public enum FunctionType
    {
    BEZIER,
    LINE,
    MODIFIEDLINE
    }*/


public class DashScript2 : MonoBehaviour
    {
    [SerializeField] float ShortDashCap;
    [SerializeField] float MinimumTouchTime;
    [SerializeField] GameObject TargetPointPrefab;
    [SerializeField] int TargetPointCount;
    [SerializeField] public float SpeedCoefficient = 1.0f;
    [SerializeField] PlayerEventHandler PEH;
    [SerializeField] public float ForwardSlashDistance = 0.5f;
    [SerializeField] public Vector2 ForwardSlashSize = new Vector2(3.0f, 1.5f);
    [SerializeField] Samurai_state_script SSS;
    [SerializeField] float MaxDashMagnitude = 10.0f;

    public float DistanceModifier = 1.0f;

    bool IsDashing = false;
    bool StartedInput = false;
    Samurai_state_script SamuraiState;
    int TouchTime = 0;


    FunctionType CurrentFuncType;
    Vector2 TouchStartPosition;
    Vector2 TargetPosition;
    Vector2 MovementOrigin;

    Vector2 ModifiedLineStart;
    Vector2 ModifiedLineEnd;
    float ModifiedA = 0.0f;

    float MovementLength = 0.0f;
    float CurrentTime = 0.0f;
    float OldT = 0.0f;
    bool IsCurved = false;
    bool HasTriggeredCollision = false;
    Vector2 CurvedDashCenter;
    bool WillCollide = false;
    //bool HasFireSlash = false;
    float HitboxSizeModifier = 1.0f;

    List<GameObject> TargetPoints = new List<GameObject>();
    EventInformation PlayerEINF;

    void UpdateEventInformation()
        {
        PlayerEINF.TargetPosition = this.TargetPosition;
        PlayerEINF.MovementOrigin = this.MovementOrigin;
        PlayerEINF.IsCurved = this.IsCurved;
        PlayerEINF.ModifiedLineStart = this.ModifiedLineStart;

        PlayerEINF.CurrentFunctType = this.CurrentFuncType;
        }


    public void FireSlashUpgrade()
        {
        //HasFireSlash = true;
        HitboxSizeModifier += 0.2f;
        }

    private void Start()
        {
        //HasFireSlash = false;
        SamuraiState = GetComponent<Samurai_state_script>();
        MovementOrigin = transform.position;

        for (int i = 0; i < TargetPointCount; i++) 
            {
            GameObject TargetPInstance = Instantiate(TargetPointPrefab);
            TargetPInstance.SetActive(false);
            TargetPoints.Add(TargetPInstance);
            }

        PEH.PlayerReference = this;
        PEH.HandleEvent(PlayerEventId.START, PlayerEINF);
        }


    void ActivatePath(bool IsActive)
        {
        foreach (GameObject Point in TargetPoints)
            {
            Point.SetActive(IsActive);
            }
        }


#region //Collision & Movement
    Vector2 CheckCollision(Vector2 CurrentPosition, Vector2 NextPosition, float a) //TODO, account for player size
        {
        Vector2 MovementVector = NextPosition - CurrentPosition;

        RaycastHit2D RayHit = Physics2D.Raycast(CurrentPosition, MovementVector.normalized, MovementVector.magnitude);
        if (RayHit.collider == null) { return CurrentPosition; } //If nothing was hit procceed as normal 
        else if (RayHit.collider.tag != "COLLISION") { return CurrentPosition; }

        //Else mirror the movement vector by the collision normal and return the lineposition from those parameters
        CurrentFuncType = FunctionType.MODIFIEDLINE;
        ModifiedLineStart = RayHit.point;

        Vector2 UnitNormal = RayHit.normal / RayHit.normal.magnitude;
        Vector2 ProjectionOntoUnitNormal = UnitNormal * (2 * (Vector2.Dot(MovementVector, UnitNormal)));
        Vector2 FuturePosition = RayHit.point + (MovementVector - ProjectionOntoUnitNormal);

        Vector2 FutureMovement = FuturePosition - RayHit.point; 
        Vector2 UnitFuture = FutureMovement.normalized;
        ModifiedLineEnd = ModifiedLineStart + UnitFuture * MovementLength;
        ModifiedA = a + ModifiedA;
        WillCollide = true;

        return CurrentPosition; 
        }


    Vector2 BezierCurvePosition(float a, Vector2 Start, Vector2 End, Vector2 Control) //Can get rid of control
        {
        Vector2 MovementVector = (-TargetPosition) * 0.5f;
        Vector2 C = new Vector2(-MovementVector.y, MovementVector.x) - MovementVector;

        C += MovementOrigin;
        End += MovementOrigin;

        Vector2 Result = (Mathf.Pow(1 - a, 2) * Start) + (2 * (1 - a) * a * C) + (Mathf.Pow(a, 2) * End);
        return (Result);
        }


    Vector2 BezierCurveExtended(float a, float a2, Vector2 Start, Vector2 End, Vector2 Control)
        {
        if (a2 == 0.0f) { return BezierCurvePosition(a, Start, End, Control); }

        Vector2 CurrentPosition = BezierCurvePosition(a, Start, End, Control);
        Vector2 NextPosition = BezierCurvePosition(a2, Start, End, Control);

        float rotation_angle = Mathf.Atan2(NextPosition.x - CurrentPosition.x, NextPosition.y - CurrentPosition.y) * Mathf.Rad2Deg * -1;
        transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);

        return CheckCollision(CurrentPosition, NextPosition, a);
        }


    Vector2 LinePosition(float a, Vector2 Start, Vector2 End)
        {
        return Vector2.Lerp(Start, End, a);
        }


    Vector2 LinePositionExtended(float a, float a2, Vector2 Start, Vector2 End)
        {
        if (a2 == 0.0f) { return LinePosition(a, Start, End); }
        if (a >= 1.0f) { return LinePosition(1.0f, Start, End); }

        Vector2 CurrentPosition = LinePosition(a, Start, End);
        Vector2 NextPosition = LinePosition(a2, Start, End);

        float rotation_angle = Mathf.Atan2(NextPosition.x - CurrentPosition.x, NextPosition.y - CurrentPosition.y) * Mathf.Rad2Deg * -1;
        transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);

        return CheckCollision(CurrentPosition, NextPosition, a);
        }


    Vector2 GetNextPosition(float a, float a2)
        {
        switch (CurrentFuncType)
            {
            case FunctionType.LINE:
                return LinePositionExtended(a, a2, MovementOrigin, TargetPosition + MovementOrigin);

            case FunctionType.BEZIER:
                return BezierCurveExtended(a, a2, MovementOrigin, TargetPosition, CurvedDashCenter);

            case FunctionType.MODIFIEDLINE:
                return LinePositionExtended(a - ModifiedA, a2 - ModifiedA, ModifiedLineStart, ModifiedLineEnd);

            default:
                return Vector2.zero;
            }
        }
#endregion


    void DashTrajectory()
        {
        CurrentFuncType = IsCurved ? FunctionType.BEZIER : FunctionType.LINE;   
        
        for (int i = 0; i < TargetPointCount; i++) 
            {
            float t2 = 0.0f;
            if ((i + 1) < TargetPointCount) { t2 = ((1.0f / TargetPointCount) * (i + 1)); }

            float t = ((1.0f / TargetPointCount) * i);
            Vector2 PointPositon = GetNextPosition(t, t2);

            TargetPoints[i].transform.position = PointPositon;
            }

        PlayerEINF.FinalPosition = TargetPoints[TargetPointCount - 1].transform.position;
        }


    private void Update()
        {
        if (Input.touchCount > 0 && IsDashing == false && SamuraiState.dead == false) //Tapping on screen
            {
            Touch TouchReference = Input.GetTouch(0);
            Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(TouchReference.position);


            if (TouchReference.phase == TouchPhase.Began) //have started touching the screen
                {
                StartedInput = true;
                MovementOrigin = transform.position;
                TouchTime = 0;
                TouchStartPosition = TouchPosition;

                ActivatePath(true);
                WillCollide = false;
                }


            if (TouchReference.phase == TouchPhase.Moved)
                {
                if (StartedInput == false) { return; }
                TargetPosition = Vector2.ClampMagnitude((TouchStartPosition - TouchPosition) * DistanceModifier, MaxDashMagnitude);
                ModifiedA = 0.0f;

                //Debug.DrawLine(MovementOrigin, MovementOrigin + TargetPosition);

                float DistanceCalc = Vector2.Distance(TouchStartPosition, new Vector2(TargetPosition.x + TouchStartPosition.x, TargetPosition.y + TouchStartPosition.y));
                MovementLength = Vector2.Distance(MovementOrigin, TargetPosition + MovementOrigin);
                IsCurved = DistanceCalc > ShortDashCap;

                if (IsCurved) 
                    {
                    Vector2 MovementVector = (-TargetPosition)*0.5f;
                    CurvedDashCenter = new Vector2(-MovementVector.y, MovementVector.x) - MovementVector;
                    }

                DashTrajectory(); //Updates line trajectory

                UpdateEventInformation();
                PEH.HandleEvent(PlayerEventId.PREDICTEDMOVEMENT, PlayerEINF);

                float rotation_angle = Mathf.Atan2(TargetPosition.x, TargetPosition.y) * Mathf.Rad2Deg * -1;
                transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);
                }

            TouchTime++;
            }
        else if (StartedInput) //Start dash
            {
            StartedInput = false;
            ActivatePath(false);
            if (TouchTime > MinimumTouchTime)
                {
                CurrentFuncType = IsCurved ? FunctionType.BEZIER : FunctionType.LINE;
                IsDashing = true;
                CurrentTime = 0.0f;
                HasTriggeredCollision = false;
                OldT = 0.0f;
                ModifiedA = 0.0f;

                PEH.HandleEvent(PlayerEventId.STARTEDMOVEMENT, PlayerEINF);
                }
            }
        }


    private void FixedUpdate()
        {
        if (IsDashing)
            {
            CurrentTime += Time.deltaTime;
            
            float Progress = CurrentTime * SpeedCoefficient / MovementLength;

            float CoolT = Mathf.Pow(Progress, 2);


            if (IsCurved == false)
                {
                if (CoolT >= 1.0f)
                    {
                    CoolT = 1.0f;
                    IsDashing = false;

                    if (CurrentFuncType == FunctionType.MODIFIEDLINE) 
                        {
                        HelperFunctions.CheckCollisionLine(ModifiedLineStart, PlayerEINF.FinalPosition, ForwardSlashDistance, ForwardSlashSize * HitboxSizeModifier, PlayerEINF.FinalPosition - ModifiedLineStart);
                        }
                    else 
                        {
                        HelperFunctions.CheckCollisionLine(MovementOrigin, MovementOrigin + TargetPosition, ForwardSlashDistance, ForwardSlashSize * HitboxSizeModifier, TargetPosition);
                        }

                    UpdateEventInformation();
                    PEH.HandleEvent(PlayerEventId.ENDEDMOVEMENT, PlayerEINF);
                    }
                }
            else
                {
                if (CoolT >= 1.0f)  
                    {
                    CoolT = 1.0f;
                    IsDashing = false;

                    if (WillCollide) 
                        {
                        HelperFunctions.CheckCollisionLine(ModifiedLineStart, PlayerEINF.FinalPosition, ForwardSlashDistance, ForwardSlashSize * HitboxSizeModifier, PlayerEINF.FinalPosition - ModifiedLineStart);
                        UpdateEventInformation();
                        PEH.HandleEvent(PlayerEventId.ENDEDMOVEMENT, PlayerEINF);
                        }
                    }


                if (WillCollide == false) 
                    {
                    if ((CoolT >= 0.5f) && (CoolT < 1.0f) && (HasTriggeredCollision == false))
                        {
                        HasTriggeredCollision = true;
                        
                        HelperFunctions.CheckCollisionCurve(MovementOrigin, MovementOrigin + TargetPosition, transform.position, HitboxSizeModifier);
                        UpdateEventInformation();
                        PEH.HandleEvent(PlayerEventId.ENDEDMOVEMENT, PlayerEINF);
                        }
                    }
                }


            transform.position = GetNextPosition(OldT, CoolT);
            OldT = CoolT;
            }
        }
    }
