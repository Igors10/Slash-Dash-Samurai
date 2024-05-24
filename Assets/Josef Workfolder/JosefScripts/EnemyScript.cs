using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] Transform target;
    

    NavMeshAgent agent;

    public bool chaseState;
    public bool idleState = true;
    public bool attackState;
    public bool attackExclamationEnabled; 
    bool isAttacking;
    bool isDead;
    bool isDashing;
    public int scoreValue = 1;
    public float rotateSpeed = 0.5f;
    public float aggroRange;
    public float attackTimer;
    public float dashDuration;
    public float stopDistance;
    public GameObject scoreManagerRef;
    public GameObject attackExclamation;
    public ParticleSystem deathParticle;
    private ScoreManagerScript updateScoreRef;

    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, target.transform.position) < aggroRange)
        {
            GoToTarget();
        }
        else if(Vector3.Distance(transform.position, target.transform.position) > aggroRange)
        {
            StopEnemy();
        }


        if (isAttacking)
        {
            agent.isStopped = true;
        }


        if (target != null && !isAttacking) 
        {
            
            dist = Vector2.Distance(transform.position, target.transform.position);
            RotateTowardsTarget();
        }
         

        if (dist < stopDistance)
        {
            StopEnemy();
        }
        else if(!idleState && !isAttacking)
        {
            GoToTarget();
            
        }

       
    }

    private void StopEnemy()
    {
        agent.isStopped = true;

        if (chaseState)
        {
            attackState = true;

            chaseState = false;

            EnemyAttack();
        }

    }

    private void GoToTarget()
    {
        agent.isStopped = false;
        idleState = false;
        agent.SetDestination(target.position);

        if (chaseState == false)
        {
            chaseState = true;
        }

        if (attackState == true)
        {
            attackState = false;
        }
    }

    //public void EnemyDeath()
    //{
    //    if (!isDead)
    //    {
    //        isDead = true;

    //        AudioManager.instance.PlaySFX("EnemyDeath");    // <<<< I've added a soundeffect here for their death

    //        scoreManagerRef = GameObject.FindWithTag("Score Manager");

    //        updateScoreRef = scoreManagerRef.GetComponent<ScoreManagerScript>();

    //        updateScoreRef.UpdateScore(scoreValue);

    //        ParticleSystem deathParticleRef = Instantiate(deathParticle, transform.position, Quaternion.identity);

    //        deathParticleRef.gameObject.GetComponent<DeathParticleScript>().ParticleDirection(target);

    //    }

    //    Destroy(gameObject);

    //}

    private void RotateTowardsTarget()
    {

        if(!idleState) 
        {
            Vector2 targetDirection = target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
        }
        
    }

    private void EnemyAttack()
    {
        if (attackState && !isAttacking ) 
        {
           StartCoroutine(AttackTimer());
            
        }
    }

    IEnumerator AttackTimer()
    {

        isAttacking = true;
        bool attackWindUp = true;
        var spriteRendererRef = gameObject.GetComponent<SpriteRenderer>();
        float flashAmount = 0;
        Vector2 endPos = target.transform.position;
        if (attackExclamationEnabled)
        {
            yield return new WaitForSeconds(.5f);
            
           // transform.GetChild(2).gameObject.SetActive(true);

            Instantiate(attackExclamation,transform.GetChild(3).transform.position, Quaternion.identity);
            
        }

        while (attackWindUp && !isDashing && flashAmount < 3f ) 
        {
            spriteRendererRef.color = Color.red;

            yield return new WaitForSeconds(.1f);

            spriteRendererRef.color = Color.white;

            yield return new WaitForSeconds(.1f);
            flashAmount++;
        }

        yield return new WaitForSeconds(attackTimer);
        attackWindUp = false;
        flashAmount= 0;
        spriteRendererRef.color = Color.white;

        StartCoroutine(DashAttack(transform.position,endPos,dashDuration));

        int SafetySwitch = 0;

        while (isDashing)
        {
            yield return new WaitForSeconds(0.2f);

            SafetySwitch++;
            
        }


        isAttacking = false;
        

        if (attackState && !isAttacking )
        {

            StartCoroutine(AttackTimer());

        }

    }
    
   



    IEnumerator DashAttack(Vector2 startPos, Vector2 endPos, float duration)
    {
        isDashing= true;
        

        var timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, timeElapsed/duration);
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

        transform.position = Vector2.Lerp(startPos, endPos, timeElapsed / duration);

        isDashing = false;
        AudioManager.instance.PlaySFX("EnemySlash"); // <<<< And I've added soundeffect here for them slashing
        transform.GetChild(0).gameObject.SetActive(true);

        //yield return new WaitForSeconds(.5f);

        //transform.GetChild(0).gameObject.SetActive(false);


    }
    
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        target = other.transform;

    //        chaseState = true;
    //        idleState = false;

    //    }
        
    //}

   



}
