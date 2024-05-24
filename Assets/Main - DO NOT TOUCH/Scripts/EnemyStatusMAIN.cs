using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyStatusMAIN : MonoBehaviour
{

    public GameObject scoreManagerRef;
    public GameObject BubbleObject;
    public GameObject DeathExplosion;
    GameObject enemySpawner;
    bool isDead;
    bool isFrozen = false;
    bool augmentRolled;
    bool IFramesActive = false;
    public bool Armor;
    public bool Bubble;
    public bool ExplodeOnDeath;
    int BubbleHealth = 0;
    public int scoreValue = 1;
    public int BubbleResetTimer = 5;
    public ParticleSystem deathParticle;
    private ScoreManagerScriptMAIN updateScoreRef;
    private EnemySpawnerMAIN enemySpanwerRef;
    EnemyAugmentScript augmentScript;
    Transform target;
    NavMeshAgent agent;
    Rigidbody rb;
    //public Sprite UnArmoredSprite;
    public GameObject ArmorIndicator;
    public EnemySpawnerMAIN SpawnerReference;
    public Sprite UnArmoredSprite;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnerReference.SpawnedEnemyTracker++;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawner = FindObjectOfType<LevelHandlerMAIN>().CurrentLevel.Spawner;
        enemySpanwerRef = enemySpawner.GetComponent<EnemySpawnerMAIN>();
        augmentScript = GetComponent<EnemyAugmentScript>();

        if (!augmentRolled)
        {
            ActivateAugment();
        }
    }

    void ActivateAugment()
    {
        if(augmentScript.selectedAugment == "1")
        {
            BubbleHealth = 1;
            BubbleObject.SetActive(true);
        }
        else if(augmentScript.selectedAugment == "2")
        {
            ExplodeOnDeath = true;
        }
        else
        {
            return;
        }
        augmentRolled = true;
    }

    public void EnemyDeath()
    {

        if (BubbleHealth == 1)
        {
           BubbleHealth--;
           BubbleObject.SetActive(false);
           StartCoroutine(BubbleReset());

        }
        else
        {
            if (Armor)
            {
                Armor = false;
                //ArmorIndicator.SetActive(false);
                StartCoroutine(ArmorIFrames());
                GetComponent<SpriteRenderer>().sprite = UnArmoredSprite;
            }
            else if (!isDead && !IFramesActive)
            {
                isDead = true;

                SpawnerReference.SpawnedEnemyTracker--;

                AudioManagerMAIN.instance.PlaySFX("EnemyDeath");    // <<<< I've added a soundeffect here for their death

                scoreManagerRef = GameObject.FindWithTag("Score Manager");

                if (MainContextMAIN.homing_missles != null) MainContextMAIN.homing_missles.GetComponent<HomingMissle_script>().ActivateSlot();

                updateScoreRef = scoreManagerRef.GetComponent<ScoreManagerScriptMAIN>();

                updateScoreRef.UpdateScore(scoreValue);

                if (ExplodeOnDeath)
                {
                    Instantiate(DeathExplosion, transform.position, Quaternion.identity);
                }
                else
                {
                    ParticleSystem deathParticleRef = Instantiate(deathParticle, transform.position, Quaternion.identity);

                    deathParticleRef.gameObject.GetComponent<DeathParticleScript>().ParticleDirection(target);
                }

                enemySpanwerRef.activeEnemies--;
                Destroy(gameObject);

            }
        }
        
       

    }

    IEnumerator BubbleReset()
    {
        yield return new WaitForSeconds(BubbleResetTimer);
        BubbleHealth++;
        BubbleObject.SetActive(true);
    }

    IEnumerator ArmorIFrames()
    {
        IFramesActive = true;
        yield return new WaitForSeconds(1.5f);
        IFramesActive = false;

    }

    public void Freeze()
    {
        agent.isStopped = true;
        rb.freezeRotation = true;
        isFrozen = true;
        StartCoroutine(UnFreezeTimer());

    }
    IEnumerator UnFreezeTimer()
    {
        yield return new WaitForSeconds(2);
        UnFreeze();
    }


    public void UnFreeze()
    {
        agent.isStopped = false;
        rb.freezeRotation = false;
        isFrozen = false;
    }
}
