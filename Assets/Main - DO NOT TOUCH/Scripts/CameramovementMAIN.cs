using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramovementMAIN : MonoBehaviour
    {

    public Transform PlayerTransform;

    // Start is called before the first frame update
    void Start()
        {

        }

    // Update is called once per frame
    void FixedUpdate()
        {
        /*
        Vector2 CurrentLevelPosition = MainContextMAIN.LevelHandler.CurrentLevelCenter;
        Vector2 NewPos = Vector2.Lerp(transform.position, PlayerTransform.position, 0.1f);
        transform.position = new Vector2(Mathf.Clamp(NewPos.x, CurrentLevelPosition.x - (LevelHandlerMAIN.LEVELWIDTH/2) + 5.5f, CurrentLevelPosition.x + (LevelHandlerMAIN.LEVELWIDTH / 2) - 5.5f),
            Mathf.Clamp(NewPos.y, CurrentLevelPosition.y - (LevelHandlerMAIN.LEVELWIDTH / 2) + 12, CurrentLevelPosition.y + (LevelHandlerMAIN.LEVELWIDTH / 2) - 12));*/


        
        Vector2 CurrentLevelPosition = MainContextMAIN.LevelHandler.CurrentLevelCenter;
        
        float LevelDifference =  (MainContextMAIN.LevelHandler.OldLevelCenter - MainContextMAIN.LevelHandler.CurrentLevelCenter).magnitude;
        Vector2 ExpandedDirection = MainContextMAIN.LevelHandler.OldLevelDirection * LevelDifference;

        float XMax = CurrentLevelPosition.x + Mathf.Max(ExpandedDirection.x, 0) + (LevelHandlerMAIN.LEVELWIDTH / 2) - 5.5f;
        float YMax = CurrentLevelPosition.y + Mathf.Max(ExpandedDirection.y, 0) + (LevelHandlerMAIN.LEVELWIDTH / 2) - 12.0f;
        float XMin = CurrentLevelPosition.x + Mathf.Min(ExpandedDirection.x, 0) - (LevelHandlerMAIN.LEVELWIDTH / 2) + 5.5f;
        float YMin = CurrentLevelPosition.y + Mathf.Min(ExpandedDirection.y, 0) - (LevelHandlerMAIN.LEVELWIDTH / 2) + 12.0f;

        Vector2 TargetPosition = new Vector2(Mathf.Clamp(PlayerTransform.position.x, XMin, XMax), Mathf.Clamp(PlayerTransform.position.y, YMin, YMax));
        transform.position = Vector2.Lerp(transform.position, TargetPosition, 0.1f);

        }
    }
