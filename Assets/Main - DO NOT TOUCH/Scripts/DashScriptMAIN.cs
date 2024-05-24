using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public enum FunctionType
    {
    BEZIER,
    LINE,
    MODIFIEDLINE
    }


public class DashScriptMAIN : MonoBehaviour
    {
    [SerializeField] float ShortDashCap;
    [SerializeField] float MinimumTouchTime;
    [SerializeField] GameObject TargetPointPrefab;
    [SerializeField] int TargetPointCount;
    [SerializeField] public float SpeedCoefficient = 1.0f;
    [SerializeField] PlayerEventHandlerMAIN PEH;
    [SerializeField] public float ForwardSlashDistance = 0.5f;
    [SerializeField] public Vector2 ForwardSlashSize = new Vector2(3.0f, 1.5f);
    [SerializeField] Samurai_state_scriptMAIN SSS;
    [SerializeField] float MaxDashMagnitude = 10.0f;
    [SerializeField] float MinimumMagnitude = 1.0f;
    [SerializeField] float Sensitivity = 1.0f;
    [SerializeField] float ShortDashBuffer;

    public float DistanceModifier = 1.0f;

    public bool IsDashing = false;
    bool StartedInput = false;
    Samurai_state_scriptMAIN SamuraiState;
    float TouchTime = 0.0f;
    public bool HasReversedControls = false;

    Animator PlayerAnimator;


    FunctionType CurrentFuncType;
    Vector2 TouchStartPosition;
    Vector2 TargetPosition;
    Vector2 MovementOrigin;

    Vector2 ModifiedLineStart;
    Vector2 ModifiedLineEnd;
    float ModifiedA = 0.0f;

    float MovementLength = 0.0f;
    float CurrentTime = 0.0f;
    bool HasMoved = false;
    float OldT = 0.0f;
    bool IsCurved = false;
    bool HasTriggeredCollision = false;
    Vector2 CurvedDashCenter;
    bool WillCollide = false;
    //bool HasFireSlash = false;
    float HitboxSizeModifier = 1.0f;
    Vector2 OldPositionTracker = Vector2.zero;

    List<GameObject> TargetPoints = new List<GameObject>();
    EventInformation PlayerEINF;
    CircleCollider2D MovingCollider;
    bool HasFireSlash = false;

    void UpdateEventInformation()
        {
        PlayerEINF.TargetPosition = this.TargetPosition;
        PlayerEINF.MovementOrigin = this.MovementOrigin;
        PlayerEINF.IsCurved = this.IsCurved;
        PlayerEINF.ModifiedLineStart = this.ModifiedLineStart;

        PlayerEINF.CurrentFunctType = this.CurrentFuncType;
        PlayerEINF.HasFireSlash = this.HasFireSlash;
        PlayerEINF.HitboxSizeManager = this.HitboxSizeModifier;
        }


    public void FireSlashUpgrade()
        {
        HasFireSlash = true;
        HitboxSizeModifier += 0.2f;
        UpdateEventInformation();
        }

    private void Start()
        {
        HasFireSlash = false;
        SamuraiState = GetComponent<Samurai_state_scriptMAIN>();
        MovementOrigin = transform.position;
        PlayerAnimator = GetComponent<Animator>();

        for (int i = 0; i < TargetPointCount; i++) 
            {
            GameObject TargetPInstance = Instantiate(TargetPointPrefab);
            TargetPInstance.SetActive(false);
            TargetPoints.Add(TargetPInstance);
            }

        PEH.PlayerReference = this;
        PEH.HandleEvent(PlayerEventId.START, PlayerEINF);
        MovingCollider = GetComponent<CircleCollider2D>();

        MainContextMAIN.DashScript = this;
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

        //Debug.DrawLine(CurrentPosition, CurrentPosition + (MovementVector.normalized * MovementVector.magnitude * 1.2f), Color.red, 5.0f);

        RaycastHit2D RayHit = Physics2D.Raycast(CurrentPosition, MovementVector.normalized, MovementVector.magnitude*1.0f);
        if (RayHit.collider == null) { return CurrentPosition; } //If nothing was hit procceed as normal 
        else if (RayHit.collider.tag != "COLLISION") { return CurrentPosition; }

        //Else mirror the movement vector by the collision normal and return the lineposition from those parameters
        CurrentFuncType = FunctionType.MODIFIEDLINE;
        ModifiedLineStart = RayHit.point;// - MovementVector.normalized * 0.1f;

        Debug.DrawLine(RayHit.point, RayHit.point + (RayHit.normal * 5.0f), Color.blue, 5.0f);
        Debug.Log(RayHit.normal + " | " + Vector2.Dot(RayHit.normal, MovementVector) + " | " + Time.realtimeSinceStartup);

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


    Vector2 BezierCurvePosition(float a, Vector2 Start, Vector2 End) //Can get rid of control
        {
        Vector2 MovementVector = (-TargetPosition) * 0.5f;
        Vector2 C = new Vector2(-MovementVector.y, MovementVector.x) - MovementVector;

        C += MovementOrigin;
        End += MovementOrigin;

        Vector2 Result = (Mathf.Pow(1 - a, 2) * Start) + (2 * (1 - a) * a * C) + (Mathf.Pow(a, 2) * End);
        return (Result);
        }


    Vector2 BezierCurveExtended(float a, float a2, Vector2 Start, Vector2 End)
        {
        if (a2 == 0.0f) { return BezierCurvePosition(a, Start, End); }

        Vector2 CurrentPosition = BezierCurvePosition(a, Start, End);
        Vector2 NextPosition = BezierCurvePosition(a2, Start, End);

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
                return BezierCurveExtended(a, a2, MovementOrigin, TargetPosition);

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
        if (Input.touchCount > 0 && IsDashing == false && SamuraiState.dead == false && !Upgrade_managerMAIN.IsUpgradePicking()) //Tapping on screen
            {
            Touch TouchReference = Input.GetTouch(0);
            Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(TouchReference.position);


            if (TouchReference.phase == TouchPhase.Began) //have started touching the screen
                {
                if (TouchTime <= MinimumTouchTime) { return; }

                StartedInput = true;
                MovementOrigin = transform.position;
                TouchTime = 0.0f;
                TouchStartPosition = TouchPosition;

                //ActivatePath(true);
                PlayerAnimator.ResetTrigger("Aim");
                PlayerAnimator.ResetTrigger("Slash");
                PlayerAnimator.SetTrigger("Idle");
                WillCollide = false;
                }


            if (TouchReference.phase == TouchPhase.Moved)
                {
                if (StartedInput == false) { return; }

                PlayerAnimator.ResetTrigger("Idle");
                PlayerAnimator.ResetTrigger("Slash");
                PlayerAnimator.SetTrigger("Aim");
                

                if (HasReversedControls == false) { TargetPosition = Vector2.ClampMagnitude((TouchStartPosition - TouchPosition) * Sensitivity, MaxDashMagnitude * DistanceModifier); }
                else { TargetPosition = Vector2.ClampMagnitude((TouchPosition - TouchStartPosition) * Sensitivity, MaxDashMagnitude * DistanceModifier); }
                if (TargetPosition.magnitude < MinimumMagnitude) { return; }
                
                ModifiedA = 0.0f;

                //Debug.DrawLine(MovementOrigin, MovementOrigin + TargetPosition);

                float DistanceCalc = Vector2.Distance(TouchStartPosition, new Vector2(TargetPosition.x + TouchStartPosition.x, TargetPosition.y + TouchStartPosition.y));

                
                if ((DistanceCalc > ShortDashCap) && (DistanceCalc < (ShortDashBuffer + ShortDashCap))) 
                    { 
                    TargetPosition = Vector2.ClampMagnitude(TargetPosition, ShortDashCap); 
                    IsCurved = false; 
                    }
                else { IsCurved = DistanceCalc > (ShortDashCap); }

                MovementLength = Vector2.Distance(MovementOrigin, TargetPosition + MovementOrigin);


                DashTrajectory(); //Updates line trajectory
                ActivatePath(true);

                UpdateEventInformation();
                PEH.HandleEvent(PlayerEventId.PREDICTEDMOVEMENT, PlayerEINF);

                float rotation_angle = Mathf.Atan2(TargetPosition.x, TargetPosition.y) * Mathf.Rad2Deg * -1;
                transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);
                HasMoved = true;
                }

            
            }
        else if (StartedInput) //Start dash
            {
            if (HasMoved == false)
                {
                StartedInput = false;
                PlayerAnimator.ResetTrigger("Slash");
                PlayerAnimator.ResetTrigger("Aim");
                PlayerAnimator.SetTrigger("Idle");
             
                return;
                }

            HasMoved = false;

            StartedInput = false;
            ActivatePath(false);
            
            CurrentFuncType = IsCurved ? FunctionType.BEZIER : FunctionType.LINE;
            IsDashing = true;
            CurrentTime = 0.0f;
            HasTriggeredCollision = false;
            OldT = 0.0f;
            ModifiedA = 0.0f;

            PEH.HandleEvent(PlayerEventId.STARTEDMOVEMENT, PlayerEINF);
            }
        }


    private void FixedUpdate()
        {
        //if (OldPositionTracker != (Vector2) transform.position) { Debug.Log("NOT SAME POSITION: " + OldPositionTracker + " | " + transform.position); }

        TouchTime += Time.deltaTime;
        if (IsDashing)
            {
            CurrentTime += Time.deltaTime;
            
            float Progress = CurrentTime * SpeedCoefficient / MovementLength;

            float CoolT = Mathf.Pow(Progress, 2);


            if ((CurrentFuncType == FunctionType.MODIFIEDLINE) || (CurrentFuncType == FunctionType.LINE)) 
                {
                MovingCollider.enabled = true;
                }
            else
                {
                MovingCollider.enabled = false;
                }


            if (IsCurved == false)
                { 
                if (CoolT >= 1.0f)
                    {
                    CoolT = 1.0f;
                    IsDashing = false;

                    if (CurrentFuncType == FunctionType.MODIFIEDLINE) 
                        {
                        HelperFunctionsMAIN.CheckCollisionLine(ModifiedLineStart, PlayerEINF.FinalPosition, ForwardSlashDistance, ForwardSlashSize * HitboxSizeModifier, PlayerEINF.FinalPosition - ModifiedLineStart);
                        PlayerAnimator.ResetTrigger("Idle");
                        PlayerAnimator.ResetTrigger("Aim");
                        PlayerAnimator.SetTrigger("Slash");
                        
                        }
                    else 
                        {
                        HelperFunctionsMAIN.CheckCollisionLine(MovementOrigin, MovementOrigin + TargetPosition, ForwardSlashDistance, ForwardSlashSize * HitboxSizeModifier, TargetPosition);
                        PlayerAnimator.ResetTrigger("Idle");
                        PlayerAnimator.ResetTrigger("Aim");
                        PlayerAnimator.SetTrigger("Slash");
                        
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
                        HelperFunctionsMAIN.CheckCollisionLine(ModifiedLineStart, PlayerEINF.FinalPosition, ForwardSlashDistance, ForwardSlashSize * HitboxSizeModifier, PlayerEINF.FinalPosition - ModifiedLineStart);
                        UpdateEventInformation();
                        PEH.HandleEvent(PlayerEventId.ENDEDMOVEMENT, PlayerEINF);
                        PlayerAnimator.ResetTrigger("Idle");
                        PlayerAnimator.ResetTrigger("Aim");
                        PlayerAnimator.SetTrigger("Slash");
                        
                        }
                    }


                if (WillCollide == false) 
                    {
                    PlayerAnimator.ResetTrigger("Idle");
                    PlayerAnimator.ResetTrigger("Aim");
                    PlayerAnimator.SetTrigger("Slash");

                    if ((CoolT >= 0.5f) && (CoolT < 1.0f) && (HasTriggeredCollision == false))
                        {
                        HasTriggeredCollision = true;
                        
                        HelperFunctionsMAIN.CheckCollisionCurve(MovementOrigin, MovementOrigin + TargetPosition, transform.position, HitboxSizeModifier);
                        UpdateEventInformation();
                        PEH.HandleEvent(PlayerEventId.ENDEDMOVEMENT, PlayerEINF);
                        //PlayerAnimator.ResetTrigger("Idle");
                        //PlayerAnimator.ResetTrigger("Aim");
                        //PlayerAnimator.SetTrigger("Slash");
                        
                        }
                    }
                }


            GetComponent<Rigidbody2D>().simulated = false;

            Vector2 NextPos2 = GetNextPosition(OldT, CoolT);
            Debug.DrawLine(transform.position, NextPos2, Color.red, 10.0f);
            transform.position = NextPos2;

            GetComponent<Rigidbody2D>().simulated = true;

            OldT = CoolT;
            }
        else
            {
            MovingCollider.enabled = false;
            }

        OldPositionTracker = transform.position;
        }



    private void OnTriggerEnter2D(Collider2D collision)
        {
        if ((collision.tag == "Enemy") && (IsDashing))
            {
            collision.GetComponent<EnemyStatusMAIN>().EnemyDeath();
            }
        }
    }
