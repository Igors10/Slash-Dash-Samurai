using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject MeleeEnemy;
    public GameObject RangedEnemy;
    public GameObject ArmoredEnemy;
    public GameObject NinjaEnemy;
    GameObject scoreManager;
    ScoreManagerScript playerScoreRef;
    int numberOfEnemyM;
    int numberOfEnemyR;
    public int spawnAreaSize;
    public int enemiesAtOnce;
    public float spawnTimer;
    public int targetTime;
    public int activeEnemies;
    int maxActiveEnemies = 10;
    public int spawnAmount = 10;
    int spawnAmountLimit = 50;
    public int threat;
    public List<GameObject> enemiesList;
    public List<Transform> spawnPoints;
    //public bool spawnerIsCleared;

    
    // Start is called before the first frame update
    void Start()
    {
        targetTime = Random.Range(50,100);

        enemiesList = new List<GameObject>();

        spawnPoints = new List<Transform>();

        scoreManager = GameObject.FindWithTag("Score Manager");

        playerScoreRef = scoreManager.GetComponent<ScoreManagerScript>();

        threat = playerScoreRef.roomsCleared;

        //spawnerIsCleared = false;

        IncreaseEnemySpawns();

        SetEnemyPoolAmount();

        AddEnemy();

        AddSpawnPoints();
    }

    void FixedUpdate()
    {


        if (targetTime == 0)
        {
           
            StartCoroutine(SpawnEnemyLoop());
        }


        targetTime--;

        

    }

    IEnumerator SpawnEnemyLoop()
    {
        AllEnemiesDead();
       
            //for(int i = 0; spawnPoints.Count > 0; i++)
            //{
            //    if (spawnPoints[i].gameObject.active == true)
            //    {
            //        activeSpawners.Add(spawnPoints[i]);
            //    }
            //}

            for(int i = 0;i < enemiesAtOnce;i++)
            {
              if(spawnAmount != 0)
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

        int spawnPoint = Random.Range(0,spawnPoints.Count);

        Vector2 spawnPointPos = spawnPoints[spawnPoint].transform.position;

        var pos = spawnPointPos + Random.insideUnitCircle * spawnAreaSize;

        Instantiate(enemiesList[enemyToSpawn], pos, Quaternion.identity);

        activeEnemies++;
        spawnAmount--;
        
    }

    public void ReloadSpawners()
    {
        Debug.Log("Spawners reloaded");
        if (targetTime == 0) return;
        Debug.Log("yes they did");


        //spawnerIsCleared = false;
        IncreaseEnemySpawns();

        SetEnemyPoolAmount();

        AddEnemy();

        AddSpawnPoints();

        targetTime = Random.Range(50, 100);

        activeEnemies = 0;

        

    }    
    void AddEnemy()
    {

        //SetEnemyPoolAmount();

        for (int i = 0; i < numberOfEnemyM; i++)
        {
            enemiesList.Add(MeleeEnemy);
        }
        
        for(int i = 0; i < numberOfEnemyR; i++)
        {
            enemiesList.Add(RangedEnemy);
        }
        
    }

    void AddSpawnPoints()
    {
        foreach(Transform child in transform)
        {
            if(child.tag == "SpawnPoint")
            {
                spawnPoints.Add(child);

            }
               
        }
    }

    void IncreaseEnemySpawns()
    {
        spawnAmount += 5 * threat;
        Mathf.Clamp(spawnAmount, 0, spawnAmountLimit);

        maxActiveEnemies += threat;

    }
    void SetEnemyPoolAmount()
    {
        numberOfEnemyM = 5;

        if (threat > 1)
        {
            numberOfEnemyR = 1 + threat;
            Mathf.Clamp(numberOfEnemyR, 0, 5);
        }
        else
        {
            numberOfEnemyR = 0;
        }

    }

    public void AllEnemiesDead()
    {
        if (activeEnemies == 0 && spawnAmount == 0)
        {
            StopCoroutine(SpawnEnemyLoop());
        }
    }
}
