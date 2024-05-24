using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController2 : MonoBehaviour
    {
    public LevelHandler2 MainLHandler;
    GameObject OldLevel;
    CardinalsV2 DirectionVector;
    public GameObject[] ConnectionWalls;
    bool HasCollided = false;
    BoxCollider2D[] Colliders;
    Vector2 InternalPosition;
    CardinalsV2 SavedEntranceDirection;

    public GameObject TrianglePointer;

    // fixing enemy spawning
    public GameObject Spawner;

    public GameObject LastWall;


    void Awake()
        {
        Colliders = GetComponents<BoxCollider2D>();
        }


    void OnTriggerEnter2D(Collider2D Col)
        {
        if (HasCollided == false)
            {
            TrianglePointer.SetActive(false);
            MainLHandler.NextLevelIteration(OldLevel, DirectionVector);
            HasCollided = true;

            if (SavedEntranceDirection != CardinalsV2.NULL)
                {
                ConnectionWalls[(int)SavedEntranceDirection].SetActive(true);
                }
            }
        }


    public void UpdateLevel(GameObject OldLevel, CardinalsV2 DirectionVector, CardinalsV2 EntranceDirection)
        {
        this.OldLevel = OldLevel;
        this.DirectionVector = DirectionVector;
        SavedEntranceDirection = EntranceDirection;

        foreach (GameObject ConnectionWall in ConnectionWalls)
            {
            ConnectionWall.SetActive(true);
            }

        foreach (BoxCollider2D Collider in Colliders)
            {
            Collider.enabled = false;
            }


        LastWall = ConnectionWalls[(int)DirectionVector];//.SetActive(false);
        TrianglePointer.GetComponent<Arrow_pointer_script>().target = LastWall.transform;
        Colliders[(int)DirectionVector].enabled = true;

        if (EntranceDirection != CardinalsV2.NULL) 
            { 
            ConnectionWalls[(int)EntranceDirection].SetActive(false); 
            }

        HasCollided = false;
        }
    }
