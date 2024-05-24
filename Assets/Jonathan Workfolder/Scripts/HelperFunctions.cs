using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions 
    {
    public static void CheckCollisionCurve(Vector2 Origin, Vector2 Destination, Vector2 MidpointPosition, float HitboxSizeModifier)
        {
        Vector2 DirectVector = Destination - Origin;
        float CapsuleAngle = 90 + Mathf.Rad2Deg * (Mathf.Atan((Destination - Origin).y / (Destination - Origin).x));
        Vector2 CapsulePoint = (DirectVector / 2) + Origin;
        Vector2 WidthSizeVector = (MidpointPosition - CapsulePoint);
        Vector2 CapsuleSize = new Vector2 (WidthSizeVector.magnitude, DirectVector.magnitude);

        Collider2D[] HitObjects = Physics2D.OverlapCapsuleAll(CapsulePoint, CapsuleSize * HitboxSizeModifier, CapsuleDirection2D.Vertical, CapsuleAngle);

        if (HitObjects.Length <= 0 ) { return; } //If empty return

        foreach (Collider2D Hit in HitObjects) 
            {
            if (Hit.tag == "Enemy")
                {
                Hit.gameObject.GetComponent<EnemyStatus>().EnemyDeath(); 
                //Object.Destroy(Hit.gameObject); 
                }
            }
        }


    public static void CheckCollisionLine(Vector2 Origin, Vector2 Destination, float LinedashAttackRange, Vector2 LinedashAttackSize, Vector2 TargetPosition)
        {
        float angle = 90 + Mathf.Rad2Deg * (Mathf.Atan((Destination - Origin).y / (Destination - Origin).x));

        Vector2 BoxPoint = Destination + (TargetPosition*LinedashAttackRange); //(MovementUnitVector * LinedashAttackSize);

        Collider2D[] HitObjects = Physics2D.OverlapBoxAll(BoxPoint, LinedashAttackSize, angle);
        if (HitObjects.Length <= 0) { return; } //If empty return


        foreach (Collider2D Hit in HitObjects)
            {
            if (Hit.tag == "Enemy")
                {
                
                Hit.gameObject.GetComponent<EnemyStatus>().EnemyDeath(); 
                //Object.Destroy(Hit.gameObject);
                }
            }
        }
    }
