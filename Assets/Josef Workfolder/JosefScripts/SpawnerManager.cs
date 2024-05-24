using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject melleeSpawner;
    public GameObject rangedSpawner;
    public GameObject armoredSpawner;
    public GameObject ninjaSpawner;
    public int spawnBudget;
    public int activeEnemies;
    public int spawnTimer;
    public int melleeEnemyWeight;
    public int rangedEnemyWeight;
    public int armoredEnemyWeight;
    public int ninjaEnemyWeight;
    int maximumActiveEnemies;
    List<string> spawnProbability;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<Transform>();
        CollectSpawnPoints();

        FillPool();

        StartCoroutine(SpawnLoop());

    }

    void CollectSpawnPoints()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "SpawnPoint")
            {
                spawnPoints.Add(child);

            }
        }
    }
    void FillPool()
    {
        for (int i = 0; i < melleeEnemyWeight; i++)
        {
            spawnProbability.Add("0");
        }

        for (int i = 0; i < rangedEnemyWeight; i++)
        {
            spawnProbability.Add("1");
        }

        for (int i = 0; i < armoredEnemyWeight; i++)
        {
            spawnProbability.Add("2");
        }

        for (int i = 0; i < ninjaEnemyWeight; i++)
        {
            spawnProbability.Add("3");
        }
    }


    IEnumerator SpawnLoop()
    {
        if(spawnBudget! <= 0)
        {
            if (activeEnemies! >= maximumActiveEnemies)
            {
                var selectedSpawn = spawnProbability[Random.Range(0, spawnProbability.Count)];

                if (selectedSpawn == "0") 
                { 
                    //SpawnMellee
                }
                else if (selectedSpawn == "1")
                {
                    //SpawnRanged
                }
                else if(selectedSpawn == "2")
                {
                    //SpawnArmored
                }
                else if(selectedSpawn == "3")
                {
                    //SpawnNinja
                }
            }
        }
        

        yield return new WaitForSeconds(spawnTimer);
        StartCoroutine(SpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
