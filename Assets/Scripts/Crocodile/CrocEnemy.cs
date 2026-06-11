using UnityEngine;
using UnityEngine.AI;

public class CrocEnemy : MonoBehaviour
{   private int enemyHealth = 100;
    public NavMeshAgent playerCroc;
    private NavMeshAgent enemyCroc;
    private float followRange = 8f;
    private float turnSpeed = 5f;
    private float stopDist = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCroc = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {   
       float distanceToTarget = Vector3.Distance(transform.position, playerCroc.transform.position);
       if (distanceToTarget <= stopDist)
        {
            enemyCroc.isStopped = true;
            enemyCroc.ResetPath();
            FaceTarget(playerCroc.transform.position);
        }
        else
        {
            enemyCroc.isStopped = false;
            enemyCroc.SetDestination(playerCroc.transform.position);
        }
    }

     private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
        {
            return;
        }
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.Slerp
        (
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }
}
