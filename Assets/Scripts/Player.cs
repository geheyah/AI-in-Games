using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private NavMeshAgent agentPlayer;

    void Start()
    {
        agentPlayer = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            agentPlayer.SetDestination(transform.position + transform.forward * 10f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            agentPlayer.SetDestination(transform.position - transform.forward * 10f);
        }
        if (Input.GetKeyDown(KeyCode.A))       
        {
            agentPlayer.SetDestination(transform.position - transform.right * 10f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            agentPlayer.SetDestination(transform.position + transform.right * 10f);
        }
        
    }
}
