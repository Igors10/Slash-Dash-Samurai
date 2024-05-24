using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;


/*public enum CardinalsV2
    {
    North = 0,
    East = 1,
    South = 2,
    West = 3,
    NULL = 4,
    }*/


public class LevelHandler2 : MonoBehaviour
    {
    //CONSTANTS
    public const float TILEMAPSIZE = 64.0f;
    public const float LEVELWIDTH = 43.0f; //Amount of TILEMAPSIZExTILEMAPSIZE grids forms a level
    public const float LEVELHEIGHT = 43.0f;
    Vector2[] DIRECTIONS = { new Vector2(0.0f, 1.0f), new Vector2(1.0f, 0.0f), new Vector2(0.0f, -1.0f), new Vector2(-1.0f, 0.0f), new Vector2(0.0f, 0.0f) };
    int DIRECTIONCOUNT = 4;
    
    
    //Variables
    public GameObject[] LevelPrefabs;
    List<GameObject> InstantiatedLevels = new List<GameObject>();
    GameObject OldLevel;
    Vector2 GlobalMovement = Vector2.zero;

    public LevelController2 CurrentLevel;
    [SerializeField] GameObject TrianglePointer;

    void Start()
        {
        for (int i = 0; i < LevelPrefabs.Length; i++) //Pool structure
            {
            GameObject InstantiatedLevel = Instantiate(LevelPrefabs[i]);

            InstantiatedLevel.SetActive(false);
            InstantiatedLevel.GetComponent<LevelController2>().MainLHandler = this;
            InstantiatedLevels.Add(InstantiatedLevel);
            }

        NextLevelIteration(null, CardinalsV2.NULL);
        }


    GameObject GetFreeLevel()
        {
        int Di = 0;

        while (Di < 100)
            {
            int r = Random.Range(0, InstantiatedLevels.Count);
            if (InstantiatedLevels[r].activeSelf == false) { return InstantiatedLevels[r]; }
            Di++;
            }

        return null;
        }


    Vector2 GetNewLevelPosition(CardinalsV2 DirectionVector)
        {
        return new Vector2(
            (DIRECTIONS[(int)DirectionVector].x + GlobalMovement.x) * LEVELWIDTH * 1.0f,
            (DIRECTIONS[(int)DirectionVector].y + GlobalMovement.y) * LEVELHEIGHT * 1.0f);
        }



    /*Vector2 GetWeightedRandomDirection(Vector2 OldDirectionVector, Vector2 ConnectionPosition) Implement Later if deemed necessary
        {
        float CenterToEntrance = Vector2.Distance(ConnectionPosition, GetNewLevelPosition(OldDirectionVector)); //Weight
        }*/


    CardinalsV2 GetRandomDirection(CardinalsV2 DirectionToExclude) 
        {
        while (true) 
            {
            int r = Random.Range(0, DIRECTIONCOUNT);
            if (r != (int)DirectionToExclude) { return (CardinalsV2)r; }
            }
        }


    void ActivateLevel(GameObject LevelToActivate, Vector2 LevelPosition, CardinalsV2 NewLevelDirectionVector, CardinalsV2 OldDirection)
        {
        LevelToActivate.SetActive(true);
        LevelToActivate.transform.position = LevelPosition;
        LevelToActivate.GetComponent<LevelController2>().UpdateLevel(OldLevel, NewLevelDirectionVector, OldDirection);
        }


    public void NextLevelIteration(GameObject LevelToDisable, CardinalsV2 Direction)
        {
        if (LevelToDisable != null) { LevelToDisable.SetActive(false); }

        GameObject FreeLevel = GetFreeLevel();
        CurrentLevel = FreeLevel.GetComponent<LevelController2>();
        CurrentLevel.TrianglePointer = this.TrianglePointer;

        // Fix for enemy spawning

        CurrentLevel.Spawner.GetComponent<EnemySpawner>().ReloadSpawners();

        //

        if (FreeLevel == null) { Debug.Log("LEVELHANDLERERROR_NULLGETLEVEL"); }

        Vector2 ConnectionPosition;
        CardinalsV2 ReversedDirection;
        if (Direction != CardinalsV2.NULL) 
            { 
            ConnectionPosition = GetNewLevelPosition(Direction);
            ReversedDirection = (CardinalsV2)(((int)Direction + 2) % 4);
            }
        else 
            { 
            ConnectionPosition = Vector2.zero;
            ReversedDirection = Direction;
            }

        ActivateLevel(FreeLevel, ConnectionPosition, GetRandomDirection(ReversedDirection), ReversedDirection);

        GlobalMovement += DIRECTIONS[(int)Direction];
        OldLevel = FreeLevel;
        }
    }

