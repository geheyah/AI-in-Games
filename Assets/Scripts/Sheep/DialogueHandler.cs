using UnityEngine;
using TMPro;

using System.Collections.Generic;

public class DialogueHandler : MonoBehaviour
{   public SheepMovement playerSheepScript;
    public SheepEnemy enemySheepScript;

    public TMP_Text countSheeple;
    public TMP_Text timerSheeple;
    public TMP_Text winloseText;

    public float gameTimer = 0f;
    private float timerEnd = 50f;

    public List<GameObject> sheepleGoodList = new List<GameObject>();
    public List<GameObject> sheepleBadList = new List<GameObject>();

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winloseText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        countSheeple.text = "|Enemy Sheeples: " + enemySheepScript.theirSheeples.ToString() + " || Player Sheeples: " + playerSheepScript.mySheeples.ToString() + "|"; 
        timerSheeple.text = gameTimer.ToString() + "s";

        gameTimer += Time.deltaTime;
        
        if (gameTimer >= timerEnd)
        {
            if (enemySheepScript.theirSheeples > playerSheepScript.mySheeples)
            {
                winloseText.enabled = true;   
                winloseText.text = "You lose!";
                Time.timeScale = 0f;
            }
             else if (playerSheepScript.mySheeples > enemySheepScript.theirSheeples)
            {
                winloseText.enabled = true;   
                winloseText.text = "You win!";
                Time.timeScale = 0f;
            }
        }
        playerSheepScript.mySheeples = sheepleGoodList.Count;
        enemySheepScript.theirSheeples = sheepleBadList.Count;
    }
    public void AddGood(GameObject sheep)
    {
        if (!sheepleGoodList.Contains(sheep))
            sheepleGoodList.Add(sheep);

        sheepleBadList.Remove(sheep);
    }

    public void AddBad(GameObject sheep)
    {
        if (!sheepleBadList.Contains(sheep))
            sheepleBadList.Add(sheep);

        sheepleGoodList.Remove(sheep);
    }
}
