using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStatus : MonoBehaviour
{

    public GameObject scoreManagerRef;
    GameObject enemySpawner;
    bool isDead;
    public int scoreValue = 1;
    public int budgetCost;
    public ParticleSystem deathParticle;
    private ScoreManagerScript updateScoreRef;
    private EnemySpawner enemySpanwerRef;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawner = GameObject.FindGameObjectWithTag("Enemy Spawner");
        enemySpanwerRef = enemySpawner.GetComponent<EnemySpawner>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EnemyDeath()
    {
        if (!isDead)
        {
            isDead = true;

            AudioManager.instance.PlaySFX("EnemyDeath");    // <<<< I've added a soundeffect here for their death

            scoreManagerRef = GameObject.FindWithTag("Score Manager");

            updateScoreRef = scoreManagerRef.GetComponent<ScoreManagerScript>();

            updateScoreRef.UpdateScore(scoreValue);

            ParticleSystem deathParticleRef = Instantiate(deathParticle, transform.position, Quaternion.identity);

            deathParticleRef.gameObject.GetComponent<DeathParticleScript>().ParticleDirection(target);

        }
        enemySpanwerRef.activeEnemies--;
        Destroy(gameObject);

    }
}
