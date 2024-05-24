using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Z VALUE INCREASES ALL THE TIME SO IT WILL CAUSE THEM TO DISSAPEAR AFTER AWHILE!!!!!!!!!!


public enum CardinalDirection
    {
    North = 0,
    East = 1,
    South = 2,
    West = 3,
    }


public class LevelController : MonoBehaviour
    {
    //Assigned in engine
    //0 = N, 1 = E, 2 = S, 3 = W
    public GameObject[] ConnectionWalls;
    public GameObject ExitConnector;
    public Vector3[] ConnectorLocations;

    public CardinalDirection ExitDirection { get; private set; }

    public void ResetLevel(GameObject RoomToDisableOnExit, CardinalDirection ComeFromDirection, Vector3 OtherLevelConnectorLocation)
        {
        foreach (GameObject ConnectionWall in ConnectionWalls) 
            { 
            ConnectionWall.SetActive(true);
            }

        //Debug.Log("CFD: " +  ComeFromDirection);
        ConnectionWalls[(int)ComeFromDirection].SetActive(false);

        float CenterToEntrance = Vector3.Distance(transform.position, OtherLevelConnectorLocation);

        //Weighted Random pick of exit location
        float BiggestDirection = 0;
        int BiggestDirectionIndex = 0;

        for (int i = 0; i < 4; i++)
            {
            if (i == (int)ComeFromDirection) { continue; }
            float DirectionValue = Random.Range(0, CenterToEntrance);

            if (DirectionValue > BiggestDirection) { BiggestDirectionIndex = i; BiggestDirection = DirectionValue; }
            }

        ExitDirection = (CardinalDirection)BiggestDirectionIndex;
        ConnectionWalls[(int)ExitDirection].SetActive(false);


        Vector3 AlignmentVector = OtherLevelConnectorLocation - ConnectorLocations[(int)ComeFromDirection];
        this.transform.Translate(AlignmentVector);

        

        ExitConnector.GetComponent<TransitionConnector>().UpdateConnector(RoomToDisableOnExit, ConnectorLocations[(int)ExitDirection], ExitDirection);   
        }
    }
