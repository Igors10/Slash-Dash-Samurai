using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerMAIN : MonoBehaviour
    {
    public LevelHandlerMAIN MainLHandler;
    GameObject OldLevel;
    CardinalsV2 DirectionVector;
    public GameObject[] ConnectionWalls;
    bool HasCollided = false;
    //BoxCollider2D[] Colliders;
    Vector2 InternalPosition;
    CardinalsV2 SavedEntranceDirection;

    public GameObject TrianglePointer;

    public GameObject LastWall;
    public BoxCollider2D LastCollider;

    public GameObject Spawner;
    public EnemySpawnerMAIN SpawnerScript;

    BoxCollider2D MainTrigger;
    [SerializeField] GameObject NavMeshObject;

    void Awake()
        {
        NavMeshObject.SetActive(true);
        MainTrigger = GetComponent<BoxCollider2D>();
        SpawnerScript = GetComponentInChildren<EnemySpawnerMAIN>();
        }


    public void LevelHasBeenCleared()
        {
        Debug.Log("LevelHasBeenCleared" + " | " + Time.realtimeSinceStartup + " | " + this);

        LastWall.SetActive(false);
        NavMeshObject.SetActive(false);
        SpawnerScript.SpawningStopped = false;

        MainLHandler.NextLevelIteration(OldLevel, DirectionVector);
        }


    private void OnTriggerEnter2D(Collider2D other)
        {
        if (other.gameObject.tag == "Player")
            {
            Debug.Log("OnTriggerEnter" + " | " + +Time.realtimeSinceStartup + " | " + this);
            }


        if ((HasCollided == false) && (other.gameObject.tag == "Player"))
            {
            TrianglePointer.SetActive(false);
            HasCollided = true;
            TrianglePointer.GetComponent<Arrow_pointer_scriptMAIN>().target = LastWall.transform;

            NavMeshObject.SetActive(true);
            SpawnerScript.ReloadSpawners();

            if (SavedEntranceDirection != CardinalsV2.NULL)
                {
                ConnectionWalls[(int)SavedEntranceDirection].SetActive(true);
                }
            }
        }


    public void UpdateLevel(GameObject OldLevel, CardinalsV2 DirectionVector, CardinalsV2 EntranceDirection, bool FirstSpawnArrow)
        {
        SpawnerScript.SpawningStopped = false;

        this.OldLevel = OldLevel;
        this.DirectionVector = DirectionVector;
        SavedEntranceDirection = EntranceDirection;

        foreach (GameObject ConnectionWall in ConnectionWalls)
            {
            ConnectionWall.SetActive(true);
            }

        /*foreach (BoxCollider2D Collider in Colliders)
            {
            Collider.enabled = false;
            }*/

        MainTrigger.enabled = false;

        LastWall = ConnectionWalls[(int)DirectionVector];//.SetActive(false);

        //Colliders[(int)DirectionVector].enabled = true;
        //LastCollider = Colliders[(int)DirectionVector];
        if (EntranceDirection != CardinalsV2.NULL) { MainTrigger.enabled = true; }

        if (EntranceDirection != CardinalsV2.NULL)
            {
            ConnectionWalls[(int)EntranceDirection].SetActive(false);
            }

        if (FirstSpawnArrow) { TrianglePointer.GetComponent<Arrow_pointer_scriptMAIN>().target = LastWall.transform; }

        HasCollided = false;
        }
    }
