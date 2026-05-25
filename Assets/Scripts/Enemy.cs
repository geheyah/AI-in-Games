using UnityEngine;
using UnityEngine.AI;   
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agentEnemy;
    public Transform targetPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentEnemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPlayer.position) < 8f)
        {
            agentEnemy.SetDestination(targetPlayer.position);
        }
    }
}
