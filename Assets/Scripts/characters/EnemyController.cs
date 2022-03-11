using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterState))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator animator;
    private Collider coll;
    protected CharacterState characterState;
    [Header("Base Settings")]
    public float sightRedius;

    public bool IsGuard;

    protected GameObject attackTarget;

    private float isLastAttackTime;
    private Vector3 guardPos;
    private Quaternion guardRotation;
    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;

    public float lookAtTime;
    private float remainLookAtTime;

    private float speed;
    //∂Øª≠bool
    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    private bool isDead;
    private bool playerDead;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterState = GetComponent<CharacterState>();
        coll = GetComponent<Collider>();
        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        if (IsGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //fix
        GameManager.Instance.AddObserver(this);
    }
    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}
    void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.RemoveObserver(this);
    }
    private void Update()
    {
        if (characterState.CurrentHealth <= 0)
        {
            isDead = true;
        }
        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimator();
            isLastAttackTime -= Time.deltaTime;
        }

    }

    void SwitchAnimator()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("IsCritical", characterState.isCritical);
        animator.SetBool("Death", isDead);
    }

    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            // ÿŒ¿
            case EnemyStates.GUARD:
                isChase = false;
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.1f);
                    }
                }
                break;
            //—≤¬ﬂ
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;
                // «∑Ò◊ﬂµΩ¡ÀÀÊª˙—≤¬ﬂµ„
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            //◊∑ª˜
            case EnemyStates.CHASE:
                isWalk = false;
                isChase = true;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (IsGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (isLastAttackTime < 0)
                    {
                        isLastAttackTime = characterState.attackData.coolDown;
                        characterState.isCritical = Random.value < characterState.attackData.criticalChance;
                        //π•ª˜
                        Attack();
                    }
                }
                break;
            //À¿Õˆ
            case EnemyStates.DEAD:
                coll.enabled = false;
                //  agent.enabled = false;
                agent.radius = 0;
                Destroy(gameObject, 2f);

                break;
        }
    }
    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            animator.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            animator.SetTrigger("Skill");
        }
    }

    /// <summary>
    /// …Ë÷√—≤¬ﬂÀÊª˙µ„
    /// </summary>
    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPos = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPos, out hit, patrolRange, 1) ? hit.position : transform.position;
    }
    /// <summary>
    /// —∞’“Player
    /// </summary>
    /// <returns></returns>
    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRedius);
        foreach (var target in colliders)
        {
            if (target.transform.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    #region ≈–∂®∑∂Œß
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterState.attackData.attackRange;
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterState.attackData.skillRange;
        else
            return false;
    }
    #endregion
    //ª≠»¶œ‘ æ∑∂Œß
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRedius);
    }
    public void Hit()
    {
        if (attackTarget != null && transform.IsFacingToTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterState>();
            targetStats.TakeDamage(characterState, targetStats);
        }
    }
    /// <summary>
    /// »ÀŒÔÀ¿Õˆ∫Ûevent
    /// </summary>
    public void EndNotify()
    {
        animator.SetBool("Win", true);
        isWalk = false;
        isFollow = false;
        attackTarget = null;
        playerDead = true;
    }
}