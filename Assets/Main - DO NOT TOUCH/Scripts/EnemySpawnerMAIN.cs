using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using Unity.VisualScripting;

public class EnemySpawnerMAIN : MonoBehaviour
    {
    public GameObject MeleeEnemy;
    public GameObject RangedEnemy;
    public GameObject ArmoredEnemy;
    public GameObject BigArcherEnemy;
    public GameObject DaggerEnemy;
    public GameObject NinjaEnemy;
    GameObject scoreManager;
    ScoreManagerScript playerScoreRef;
    int numberOfEnemyM;
    int numberOfEnemyR;
    int numberOfEnemyArmor;
    int numberOfEnemyBigArcher;
    int numberOfEnemyDagger;
    public int spawnAreaSize;
    public int enemiesAtOnce;
    public float spawnTimer;
    public int targetTime;
    public int activeEnemies;
    int maxActiveEnemies = 10;
    public int spawnAmount = 10;
    public int spawnAmountLimit = 60;
    public List<GameObject> enemiesList;
    public List<Transform> spawnPoints;
    public int levelCount;
    public bool isSpawning = false;
    bool startSpawning = false;

    public bool SpawningStopped = false;
    public int SpawnedEnemyTracker = 0;
    //public bool spawnerIsCleared;


    // Start is called before the first frame update
    void Start()
        {
        levelCount = MainContextMAIN.LevelCount;

        enemiesList = new List<GameObject>();

        spawnPoints = new List<Transform>();

        if (levelCount == 0)
            {
            targetTime = Random.Range(50, 100);

            startSpawning = true;

            IncreaseEnemySpawns();

            SetEnemyPoolAmount();

            AddEnemy();

            AddSpawnPoints();
            }

        }

    void FixedUpdate()
        {
        if (targetTime == 0)
            {
            isSpawning = true;
            StartCoroutine(SpawnEnemyLoop());
            }

        targetTime--;
        }


    IEnumerator SpawnEnemyLoop()
        {

        AllEnemiesDead();


        for (int i = 0; i < enemiesAtOnce; i++)
            {
            if (spawnAmount != 0)
                {
                if (activeEnemies != maxActiveEnemies)
                    {
                    Spawn();
                    }
                }
            }

        yield return new WaitForSeconds(spawnTimer);

        StartCoroutine(SpawnEnemyLoop());
        }

    public void Spawn()
        {
        int enemyToSpawn = Random.Range(0, enemiesList.Count);

        int spawnPoint = Random.Range(0, spawnPoints.Count);

        if (spawnPoints.Count == 0) { AllEnemiesDead(); return; }

        Vector2 spawnPointPos = spawnPoints[spawnPoint].transform.position;

        var pos = spawnPointPos + Random.insideUnitCircle * spawnAreaSize;


        enemiesList[enemyToSpawn].GetComponent<EnemyStatusMAIN>().SpawnerReference = this;
        GameObject EnemyInstance = Instantiate(enemiesList[enemyToSpawn], pos, Quaternion.identity);
        

        activeEnemies++;
        spawnAmount--;

        }

    public void ReloadSpawners()
        {
        SpawningStopped = false;
        SpawnedEnemyTracker = 0;

        Debug.Log("RELOADSPAWNERS" + " | " + Time.realtimeSinceStartup);

        if (targetTime == 0) return;


        levelCount = MainContextMAIN.LevelCount;

        activeEnemies = 0;

        spawnAmount = 10;

        startSpawning = true;

        IncreaseEnemySpawns();

        SetEnemyPoolAmount();

        AddEnemy();

        AddSpawnPoints();

        targetTime = Random.Range(50, 100);



        }
    void AddEnemy()
        {

        for (int i = 0; i < numberOfEnemyM; i++)
            {
            enemiesList.Add(MeleeEnemy);
            }

        for (int i = 0; i < numberOfEnemyR; i++)
            {
            enemiesList.Add(RangedEnemy);
            }

        for (int i = 0; i < numberOfEnemyArmor; i++)
            {
            enemiesList.Add(ArmoredEnemy);
            }

        for (int i = 0; i < numberOfEnemyBigArcher; i++)
            {
            enemiesList.Add(BigArcherEnemy);
            }

        for (int i = 0; i < numberOfEnemyDagger; i++)
            {
            enemiesList.Add(DaggerEnemy);
            }
        }

    void AddSpawnPoints()
        {
        foreach (Transform child in transform)
            {
            if (child.tag == "SpawnPoint")
                {
                spawnPoints.Add(child);

                }

            }
        }


    void IncreaseEnemySpawns()
        {
        spawnAmount += 6 * levelCount;

        maxActiveEnemies += levelCount;
        }


    void SetEnemyPoolAmount()
        {
        numberOfEnemyM = 5;

        if (levelCount > 1)
            {
            numberOfEnemyR = 1 + levelCount;
            Mathf.Clamp(numberOfEnemyR, 0, 5);
            }
        else
            {
            numberOfEnemyR = 0;
            }

        if (levelCount > 4)
            {
            numberOfEnemyArmor = 1 + levelCount / 4;
            Mathf.Clamp(numberOfEnemyArmor, 0, 3);
            }
        else
            {
            numberOfEnemyArmor = 0;
            }

        if (levelCount > 7)
            {
            numberOfEnemyBigArcher = 1 + levelCount / 7;
            Mathf.Clamp(numberOfEnemyBigArcher, 0, 3);
            }
        else
            {
            numberOfEnemyBigArcher = 0;
            }

        if (levelCount > 10)
            {
            numberOfEnemyDagger = 1 + levelCount / 10;
            Mathf.Clamp(numberOfEnemyDagger, 0, 3);
            }
        else
            {
            numberOfEnemyDagger = 0;
            }

        }

    public void AllEnemiesDead()
        {
        if (activeEnemies == 0 && spawnAmount == 0)
            {
            Debug.Log("ALLENEMIESDEAD");

            SpawningStopped = true;
            StopCoroutine(SpawnEnemyLoop());

            }
        }


    }
