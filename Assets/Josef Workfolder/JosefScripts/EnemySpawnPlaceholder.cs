using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPlaceholder : MonoBehaviour
{
    public GameObject EnemyToSpawn;
    public float spawnTimer;
    public int targetTime;
    // Start is called before the first frame update
    void Start()
    {

        targetTime = Random.Range(100, 401);
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (targetTime == 0) 
        {
            StartCoroutine(SpawnEnemy());
        }
        
        
            targetTime--;
        
    }

    IEnumerator SpawnEnemy()
    {
        Instantiate(EnemyToSpawn, transform.position, transform.rotation);

        yield return new WaitForSeconds(spawnTimer);

        StartCoroutine(SpawnEnemy());
    }

}
