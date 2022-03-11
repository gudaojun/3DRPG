using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator anim;
    private GameObject attackTarget;
    private CharacterState characterState;

    private float lastAttackTime;
    public bool isDead;
    private float stopDistance;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterState = GetComponent<CharacterState>();
        stopDistance = agent.stoppingDistance;
    }
    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }
    private void OnEnable()
    {
        GameManager.Instance.ResgisterPlayer(characterState);
        MouseManager.Instance.OnMouseClick += MoveToTarget;
        MouseManager.Instance.OnEnemyClick += EventAttack;
    }

    private void OnDisable()
    {
        if (!MouseManager.IsInitialized) return;
        MouseManager.Instance.OnMouseClick -= MoveToTarget;
        MouseManager.Instance.OnEnemyClick -= EventAttack;

    }

    private void EventAttack(GameObject obj)
    {
        if (obj != null)
        {
            attackTarget = obj;
            characterState.isCritical = Random.value < characterState.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }


    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterState.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterState.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        if (lastAttackTime < 0)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("IsCritical", characterState.isCritical);
            lastAttackTime = characterState.attackData.coolDown;
        }
    }

    private void Update()
    {
        isDead = characterState.CurrentHealth == 0;
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("IsCritical", characterState.isCritical);
        anim.SetBool("Death", isDead);
    }



    public void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.gameObject.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockStates = RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterState>();
            targetStats.TakeDamage(characterState, targetStats);
        }

    }
}