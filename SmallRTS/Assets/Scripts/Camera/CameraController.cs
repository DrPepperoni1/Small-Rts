using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 mousePosition;
    public float movementSpeed;
    public float edgePanBorder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        //edgepan
        mousePosition = Input.mousePosition;
        
        
    }
    private void FixedUpdate()
    {
        float xMovement = 0;
        float zMovement = 0;

        // left/right movement
        if (mousePosition.x < edgePanBorder)
        {
            xMovement = -1f;
        }
        else if (mousePosition.x > Screen.width - edgePanBorder)
        {
            xMovement = 1f;
        }

        // up/down movement
        if (mousePosition.y < edgePanBorder)
        {
            zMovement = -1f;
        }
        else if (mousePosition.y > Screen.height - edgePanBorder)
        {
            zMovement = 1f;
        }
        transform.position = new Vector3(transform.position.x + xMovement * movementSpeed * Time.fixedDeltaTime, transform.position.y, transform.position.z + zMovement * movementSpeed * Time.fixedDeltaTime);
    }
}
