using UnityEngine;

public class SheepMovement : MonoBehaviour
{
     public float moveSpeed = 5f; 
    public float mySheeples = 0;   
    private Transform sheep_transform;
    public Camera plyrCamera;
    private Vector3 mouseXY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sheep_transform = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;   
    }

    // Update is called once per frame
    void Update()
    {
        mouseXY.y += Input.GetAxis("Mouse Y");
        mouseXY.x += Input.GetAxis("Mouse X");
        Vector3 mousePosition = plyrCamera.ScreenToWorldPoint(Input.mousePosition);
        sheep_transform.rotation = Quaternion.Euler(0, mouseXY.x, 0);

        //WASD
        if (Input.GetKey(KeyCode.W))
        {
            sheep_transform.position = Vector3.Lerp(sheep_transform.position, sheep_transform.position + this.transform.forward, moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            sheep_transform.position = Vector3.Lerp(sheep_transform.position, sheep_transform.position - this.transform.forward, moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            sheep_transform.position = Vector3.Lerp(sheep_transform.position, sheep_transform.position - this.transform.right, moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            sheep_transform.position = Vector3.Lerp(sheep_transform.position, sheep_transform.position + this.transform.right, moveSpeed * Time.deltaTime);
        }
    }
}
