using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardinalsV2
    {
    North = 0,
    East = 1,
    South = 2,
    West = 3,
    NULL = 4,
    }


public class LevelHandlerMAIN : MonoBehaviour
    {
    //CONSTANTS
    public const float TILEMAPSIZE = 64.0f;
    public const float LEVELWIDTH = 43.0f; //Amount of TILEMAPSIZExTILEMAPSIZE grids forms a level
    public const float LEVELHEIGHT = 43.0f;
    Vector2[] DIRECTIONS = { new Vector2(0.0f, 1.0f), new Vector2(1.0f, 0.0f), new Vector2(0.0f, -1.0f), new Vector2(-1.0f, 0.0f), new Vector2(0.0f, 0.0f) };
    int DIRECTIONCOUNT = 4;

    private int SafetySwitch = 0;
    private const int SAFETYLIMIT = 10000;


    //Variables
    public GameObject[] LevelPrefabs;
    List<GameObject> InstantiatedLevels = new List<GameObject>();
    GameObject OldLevel;
    Vector2 GlobalMovement = Vector2.zero;
    public Vector2 CurrentLevelCenter = Vector2.zero; 
    public Vector2 OldLevelCenter = Vector2.zero;
    public Vector2 OldLevelDirection = Vector2.zero;

    public LevelControllerMAIN CurrentLevel;
    [SerializeField] GameObject TrianglePointer;
    bool FirstSpawn = true;

    void Start()
        {
        for (int i = 0; i < LevelPrefabs.Length; i++) //Pool structure
            {
            GameObject InstantiatedLevel = Instantiate(LevelPrefabs[i]);

            InstantiatedLevel.SetActive(false);
            InstantiatedLevel.GetComponent<LevelControllerMAIN>().MainLHandler = this;
            InstantiatedLevels.Add(InstantiatedLevel);
            }

        NextLevelIteration(null, CardinalsV2.NULL);
        MainContextMAIN.LevelHandler = this;
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


    CardinalsV2 GetRandomDirection(CardinalsV2 DirectionToExclude) 
        {
        SafetySwitch = 0;
        while (true) 
            {
            SafetySwitch++;
            int r = Random.Range(0, DIRECTIONCOUNT);
            if (r != (int)DirectionToExclude) { return (CardinalsV2)r; }

            if (SafetySwitch > SAFETYLIMIT) { Debug.Log("UNSAFEWHILELOOP 242"); return CardinalsV2.East; }
            }
        }


    void ActivateLevel(GameObject LevelToActivate, Vector2 LevelPosition, CardinalsV2 NewLevelDirectionVector, CardinalsV2 OldDirection, bool FirstSpawnArrow)
        {
        LevelToActivate.SetActive(true);
        LevelToActivate.transform.position = LevelPosition;
        OldLevelCenter = CurrentLevelCenter;
        CurrentLevelCenter = LevelPosition;
        LevelToActivate.GetComponent<LevelControllerMAIN>().UpdateLevel(OldLevel, NewLevelDirectionVector, OldDirection, FirstSpawnArrow);
        }


    public void NextLevelIteration(GameObject LevelToDisable, CardinalsV2 Direction)
        {
        if (LevelToDisable != null) { LevelToDisable.SetActive(false); }
        bool FirstSpawnArrow = false;

        GameObject FreeLevel = GetFreeLevel();
        CurrentLevel = FreeLevel.GetComponent<LevelControllerMAIN>();
        CurrentLevel.TrianglePointer = this.TrianglePointer;
        CurrentLevel.Spawner.GetComponent<EnemySpawnerMAIN>().isSpawning = false;

        Debug.Log("INSTANCEDLEVEL: " + CurrentLevel + " | " + Time.realtimeSinceStartup);

        if (FirstSpawn) { CurrentLevel.Spawner.GetComponent<EnemySpawnerMAIN>().ReloadSpawners(); FirstSpawnArrow = true; FirstSpawn = false; }


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

        ActivateLevel(FreeLevel, ConnectionPosition, GetRandomDirection(ReversedDirection), ReversedDirection, FirstSpawnArrow);

        GlobalMovement += DIRECTIONS[(int)Direction];
        OldLevel = FreeLevel;
        OldLevelDirection = DIRECTIONS[(int)ReversedDirection];
        }
    }

