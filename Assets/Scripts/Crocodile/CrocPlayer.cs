using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;

public class CrocPlayer : MonoBehaviour
{   
    private float crocStamina = 200f;
    private float crocHealth = 200f;
    public float moveSpeed = 50f;    
    private Transform croc_transform;
    public Camera plyrCamera;
    private Vector3 mouseXY;
    public TMP_Text debugText;
    void Start()
    {
        croc_transform = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {   //DEBUG
        debugText.text = "DEV: \nStamina: " + crocStamina.ToString() + "\nHealth: " + crocHealth.ToString();
        
        mouseXY.y += Input.GetAxis("Mouse Y");
        mouseXY.x += Input.GetAxis("Mouse X");
        Vector3 mousePosition = plyrCamera.ScreenToWorldPoint(Input.mousePosition);
        croc_transform.rotation = Quaternion.Euler(0, mouseXY.x, 0);
        
        //WASD
        if (Input.GetKey(KeyCode.W))
        {
            croc_transform.position = Vector3.Lerp(croc_transform.position, croc_transform.position + this.transform.forward, moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            croc_transform.position = Vector3.Lerp(croc_transform.position, croc_transform.position - this.transform.forward, moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            croc_transform.position = Vector3.Lerp(croc_transform.position, croc_transform.position - this.transform.right, moveSpeed * Time.deltaTime);
        }
        //DASH + STAMINA 
        if (Input.GetKey(KeyCode.D))
        {
            croc_transform.position = Vector3.Lerp(croc_transform.position, croc_transform.position + this.transform.right, moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space) && crocStamina > 0)
        {   
            crocStamina --;
            croc_transform.position = Vector3.Lerp(croc_transform.position, croc_transform.position + this.transform.forward * 2f, moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && crocStamina <= 0)
        {
            crocStamina = 200;
        }
        
    }
    
}
