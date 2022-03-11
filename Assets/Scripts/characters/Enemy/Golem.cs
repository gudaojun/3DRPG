using UnityEngine;
using UnityEngine.AI;
public class Golem : EnemyController
{
    [Header("skill")]
    public float kickForce = 25;

    public GameObject rockPrefab;
    public Transform handPos;
    //Animation Event
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingToTarget(attackTarget.transform))
        {

            var targetStats = attackTarget.GetComponent<CharacterState>();
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            targetStats.GetComponent<Animator>().SetTrigger("Dizzy");
            targetStats.TakeDamage(characterState, targetStats);
        }
    }

    //Animation Event
    public void ThrowRock()
    {
        var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
        rock.GetComponent<Rock>().target = attackTarget;
    }
}