using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScriptMAIN : MonoBehaviour
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
    public bool AttackTypeFast;
    bool isDaggering;
    Vector2 endPos;
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
    Animator enemyAnim;
    private float dist;
    private int SafetySwitch = 0;
    private const int SAFETYLIMIT = 10000;
    string attackAnimName;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
        var status = GetComponent<EnemyStatusMAIN>();

        if(status.Armor == true)
        {
            attackAnimName = "armorattack";
        }
        else
        {
            attackAnimName = "swordmanattack";
        }
        
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


        if (target != null && !isDashing)
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


        enemyAnim.SetBool("Chasing", true);
        
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

        enemyAnim.SetBool("Chasing", true);
    }


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
        enemyAnim.SetBool("Chasing", false);
        if (AttackTypeFast)
        {
            if (MainContextMAIN.DashScript.IsDashing == false)
            {
                StartCoroutine(FastAttack());
            }
        }
        else
        {
            bool attackWindUp = true;
            var spriteRendererRef = gameObject.GetComponent<SpriteRenderer>();

            float flashAmount = 0;
            endPos = target.transform.position;
            if (attackExclamationEnabled)
            {
                yield return new WaitForSeconds(.5f);

                // transform.GetChild(2).gameObject.SetActive(true);

                Instantiate(attackExclamation, transform.GetChild(3).transform.position, Quaternion.identity);
            }

            SafetySwitch = 0;
            while (attackWindUp && !isDashing && flashAmount < 3f)
            {
                spriteRendererRef.color = Color.red;

                yield return new WaitForSeconds(.1f);

                spriteRendererRef.color = Color.white;

                yield return new WaitForSeconds(.1f);
                flashAmount++;

                SafetySwitch++;

                if (SafetySwitch > SAFETYLIMIT) { Debug.Log("UNSAFEWHILELOOP 184"); break; }
            }

            yield return new WaitForSeconds(attackTimer);
            attackWindUp = false;
            flashAmount = 0;
            spriteRendererRef.color = Color.white;

            StartCoroutine(DashAttack(transform.position, endPos, dashDuration));

            SafetySwitch = 0;
            while (isDashing || isDaggering)
            {
                yield return new WaitForSeconds(0.2f);

                SafetySwitch++;

                if (SafetySwitch > SAFETYLIMIT) { Debug.Log("UNSAFEWHILELOOP 208"); break; }
            }

            isAttacking = false;

            if (attackState && !isAttacking)
            {

                StartCoroutine(AttackTimer());

            }
        }
     }





    IEnumerator DashAttack(Vector2 startPos, Vector2 endPos, float duration)
    {
        isDashing= true;
        

        var timeElapsed = 0f;

        SafetySwitch = 0;
        while (timeElapsed < duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, timeElapsed/duration);
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            SafetySwitch++;

            if (SafetySwitch > SAFETYLIMIT) { Debug.Log("UNSAFEWHILELOOP 242"); break; }
            }

        transform.position = Vector2.Lerp(startPos, endPos, timeElapsed / duration);

        isDashing = false;
        enemyAnim.SetBool("Attacking", true);
        enemyAnim.Play(attackAnimName);
        AudioManagerMAIN.instance.PlaySFX("EnemySlash"); // <<<< And I've added soundeffect here for them slashing
        yield return new WaitForSeconds(.5f);       
        transform.GetChild(0).gameObject.SetActive(true);
        enemyAnim.SetBool("Attacking", false);



        //transform.GetChild(0).gameObject.SetActive(false);


    }

    IEnumerator FastAttack()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(attackTimer);
        isAttacking = false;

        if (attackState && !isAttacking)
        {

            StartCoroutine(AttackTimer());

        }


    }





}
