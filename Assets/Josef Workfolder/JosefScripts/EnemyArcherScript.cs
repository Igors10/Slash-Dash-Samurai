using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyArcherScript : MonoBehaviour
{
    [SerializeField] Transform target;


    NavMeshAgent agent;

    public bool chaseState;
    public bool idleState = true;
    public bool attackState;
    public bool attackExclamationEnabled;
    bool isAttacking;
    bool isDead;
    bool isShooting;
    public int scoreValue = 1;
    public float rotateSpeed = 0.5f;
    public float aggroRange;
    public float attackTimer;
    public float stopDistance;
    private float dist;
    public GameObject scoreManagerRef;
    public GameObject attackExclamation;
    public GameObject projectileArrow;
    public ParticleSystem deathParticle;
    private ScoreManagerScript updateScoreRef;

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
        else if (Vector3.Distance(transform.position, target.transform.position) > aggroRange)
        {
            StopEnemy();
        }


        if (isAttacking)
        {
            agent.isStopped = true;
        }

        if (target != null)
        {

            dist = Vector2.Distance(transform.position, target.transform.position);
            RotateTowardsTarget();
        }


        if (dist < stopDistance)
        {
            StopEnemy();
        }
        else if (!idleState && !isAttacking)
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

        if (!idleState && !isShooting)
        {
            Vector2 targetDirection = target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
        }

    }

    private void EnemyAttack()
    {
        if (attackState && !isAttacking)
        {
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator AttackTimer()
    {

        isAttacking = true;

        yield return new WaitForSeconds(1f);

        bool attackWindUp = true;
        var spriteRendererRef = gameObject.GetComponent<SpriteRenderer>();
        float flashAmount = 0;

        if (attackExclamationEnabled)
        {
            yield return new WaitForSeconds(.5f);

            Instantiate(attackExclamation, transform.GetChild(0).transform.position, Quaternion.identity);

           isShooting= true;
        }

        while (attackWindUp && flashAmount < 3f)
        {
            spriteRendererRef.color = Color.red;

            yield return new WaitForSeconds(.1f);

            spriteRendererRef.color = Color.white;

            yield return new WaitForSeconds(.1f);
            flashAmount++;
        }

        yield return new WaitForSeconds(attackTimer);
        attackWindUp = false;
        flashAmount = 0;
        spriteRendererRef.color = Color.white;

        RangedAttack();


        isAttacking = false;
        isShooting= false;
        if (attackState && !isAttacking)
        {

            StartCoroutine(AttackTimer());

        }

    }

    private void RangedAttack()
    {
        var targetPos = target.transform.position;
        var fireingPoint = transform.GetChild(1).transform;
        Instantiate(projectileArrow, fireingPoint.position, fireingPoint.rotation);

    }
}
