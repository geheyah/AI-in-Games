using UnityEngine;
using UnityEngine.AI;

public class Sheeple : MonoBehaviour
{   
    public NavMeshAgent playerSheep;
    public NavMeshAgent enemySheep;
    private NavMeshAgent npcSheep;
    private float npcRange = 50f;
    private float followRange = 10f;

    //change color
    public MeshRenderer[] sheepFluff;
    public Material playerFluff;
    public Material enemyFluff;

    //change total
    public DialogueHandler dialogueHandlerScript;
    

    void Start()
    {
        npcSheep = GetComponent<NavMeshAgent>();
    }

    void Update()
    {   
         
        npcSheep.SetDestination(npcRange * Random.insideUnitCircle);
       
        //change color + follow

        if (Vector3.Distance(npcSheep.transform.position, playerSheep.transform.position) < 5f)
        {
            sheepFluff[0].material = playerFluff;
            sheepFluff[1].material = playerFluff;
            sheepFluff[2].material = playerFluff;
            dialogueHandlerScript.AddGood(gameObject); 
            npcSheep.SetDestination(playerSheep.transform.position + new Vector3(Random.Range(-followRange, followRange), 0, Random.Range(-followRange, followRange)));
        }

        if (Vector3.Distance(npcSheep.transform.position, enemySheep.transform.position) < 5f)
        {
            sheepFluff[0].material = enemyFluff;
            sheepFluff[1].material = enemyFluff;
            sheepFluff[2].material = enemyFluff;
            dialogueHandlerScript.AddBad(gameObject); 
            npcSheep.SetDestination(enemySheep.transform.position + new Vector3(Random.Range(-followRange, followRange), 0, Random.Range(-followRange, followRange)));
        }
    }
}
