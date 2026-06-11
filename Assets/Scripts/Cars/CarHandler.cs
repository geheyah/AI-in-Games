using UnityEngine;
using UnityEngine.AI;

public class CarHandler : MonoBehaviour
{
    private NavMeshAgent carAgent;
    public Transform [] points;
    public int destPoint = 0;
    //stop light
    public StopLightHandler stopLight;
    public Transform stopPoint;
    public float stopDistance = 2f;
    //speed
    public float slowSpeed = 7f;
    public float normalSpeed = 10f;
    void Start()
    {   
        carAgent = GetComponent<NavMeshAgent>();
        carAgent.speed = normalSpeed;
    }

    void Update()
    {   
        StopAtLight();
        if (carAgent.pathPending)
        {   return; }
            if (carAgent.remainingDistance <= carAgent.stoppingDistance)
            {
                GotoNextPoint();
            }
        
    }

    void StopAtLight()
    {
        float distanceToStopPoint = Vector3.Distance(transform.position, stopPoint.position);
        if (!stopLight.isGreen && distanceToStopPoint <= stopDistance * 2)
        {
            carAgent.isStopped = true;
            carAgent.speed = 0;
        }
        else if (!stopLight.isGreen && distanceToStopPoint <= stopDistance * 4)
        {   
            carAgent.isStopped = false;
            carAgent.speed = slowSpeed;
        }
        else
        {
            carAgent.isStopped = false;
            carAgent.speed = normalSpeed;
        }
    }
    void GoCurentPoint()
    {
        carAgent.destination = points[destPoint].position;
    }
    void GotoNextPoint()
    {
        destPoint++;
        if (destPoint >= points.Length)
        {
            destPoint = 0;
        }
        GoCurentPoint();
    }
}
