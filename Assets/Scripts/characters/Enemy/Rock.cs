using UnityEngine;
using UnityEngine.AI;

public enum RockStates { HitPlayer, HitEnemy, HitNothing }
public class Rock : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    private Vector3 direction;
    public RockStates rockStates;
    public int damage;
    public GameObject breakEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FlyToTarget();
        rockStates = RockStates.HitPlayer;
        rb.velocity = Vector3.one;
    }

    private void FlyToTarget()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1)
        {
            rockStates = RockStates.HitNothing;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterState>().TakeDamage(damage, other.gameObject.GetComponent<CharacterState>());
                }
                rockStates = RockStates.HitNothing;
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem>())
                {
                    other.gameObject.GetComponent<CharacterState>().TakeDamage(damage, other.gameObject.GetComponent<CharacterState>());
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }

                break;
            case RockStates.HitNothing:
                // rb.velocity = Vector3.one;
                break;
            default:
                break;
        }
    }
}