using UnityEngine;

public class StopLightHandler : MonoBehaviour
{
    public bool isGreen = true;
    public float redTime = 5f;
    public float greenTime = 5f;
    private float timer = 0f;
  
    void Update()
    {
        timer += Time.deltaTime;
        if (isGreen && timer >= greenTime)
        {
            isGreen = false;
            timer = 0f;
            Debug.Log("Red Light");
        }
        else if (!isGreen && timer >= redTime)
        {
            isGreen = true;
            timer = 0f;
            Debug.Log("Green Light");
        }
    }
}
