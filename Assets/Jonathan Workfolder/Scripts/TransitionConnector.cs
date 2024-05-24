using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TransitionConnector : MonoBehaviour
    {

    public GameObject ExitLocation;
    GameObject RoomToDisableOnExit;
    bool HasTriggered = false;

    void OnTriggerEnter2D(Collider2D Col)
        {
        if (HasTriggered == false)
            {
            MainContext.Instance.LevelHandler.NewLevel();
            if (RoomToDisableOnExit != null) 
                { 
                RoomToDisableOnExit.SetActive(false);
                RoomToDisableOnExit.transform.position = Vector3.zero;
                TransitionConnector OtherRoomTC = RoomToDisableOnExit.GetComponent<LevelController>().ExitConnector.GetComponent<TransitionConnector>();
                OtherRoomTC.transform.position = Vector3.left;
                OtherRoomTC.transform.rotation = Quaternion.identity;
                }
            HasTriggered = true;
            }
        }


    public void UpdateConnector(GameObject RoomToDisableOnExit, Vector3 Position, CardinalDirection Direction)
        {
        transform.localPosition = Position;
        transform.Rotate(Vector3.forward, -90 * (int) Direction);
        
        this.RoomToDisableOnExit = RoomToDisableOnExit;

        HasTriggered = false;
        }
    }
