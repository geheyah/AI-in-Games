using UnityEngine;
using UnityEngine.AI;

public class SheepEnemy : MonoBehaviour
{
    private NavMeshAgent badSheep;

    public float theirSheeples = 0;
    private float badRange = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        badSheep = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        badSheep.SetDestination(badRange * Random.insideUnitCircle);
    }
}
