using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private NavMeshAgent agentNPC;
    public Transform [] waypoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentNPC = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agentNPC.SetDestination(waypoints[0].position);

        if (agentNPC.pathPending || agentNPC.remainingDistance > 0.5f)
        {
            return;
        }
        else
        {
            Transform temp = waypoints[0];
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                waypoints[i] = waypoints[i + 1];
            }
            waypoints[waypoints.Length - 1] = temp;
        }

    }
}
