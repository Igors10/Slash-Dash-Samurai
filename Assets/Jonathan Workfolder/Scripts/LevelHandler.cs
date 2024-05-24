using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelHandler : MonoBehaviour
    {
    public Transform PlayerTransform; 
    public GameObject[] LevelPrefabs;


    List<GameObject> LevelList = new List<GameObject>();
    GameObject OldLevel;
    CardinalDirection CurrentLevelExitDirection = CardinalDirection.East;

    // Start is called before the first frame update
    void Start()
        {
        for (int i = 0; i < LevelPrefabs.Length; i++) //Pool structure
            {
            GameObject InstantiatedLevel = Instantiate(LevelPrefabs[i]);

            InstantiatedLevel.SetActive(false);
            LevelList.Add(InstantiatedLevel);
            }

        
        NewLevel();
        }


    public void NewLevel()
        {
        int NewRoomId = Random.Range(0, LevelList.Count);

        Vector3 ConnectorLocation = Vector3.zero;

        if (OldLevel != null) 
            { 
            CurrentLevelExitDirection = OldLevel.GetComponent<LevelController>().ExitDirection;
            ConnectorLocation = OldLevel.GetComponent<LevelController>().ExitConnector.GetComponent<TransitionConnector>().ExitLocation.transform.position;
            //OldLevel.SetActive(false);
            while ((OldLevel == LevelList[NewRoomId]) || (LevelList[NewRoomId].activeSelf)) { NewRoomId = Random.Range(0, LevelList.Count); }
            }


        GameObject NewLevel = LevelList[NewRoomId];
        CardinalDirection NewLevelExitDirection = (CardinalDirection)(((int)CurrentLevelExitDirection + 2) % 4); //Reverse the direction for the next level

        LevelController InstantiatedLevelController = LevelList[NewRoomId].GetComponent<LevelController>(); 
        if (InstantiatedLevelController == null) { Debug.Log("FAULTY LEVEL"); return; }

        InstantiatedLevelController.ResetLevel(OldLevel, NewLevelExitDirection, ConnectorLocation);
        NewLevel.SetActive(true);



        OldLevel = NewLevel;
        CurrentLevelExitDirection = NewLevelExitDirection;
        }

    }
