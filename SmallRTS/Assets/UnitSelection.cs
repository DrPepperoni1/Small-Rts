using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayer;
    public LayerMask groundLayer;
    public List<GameObject> selectedUnits;
    bool dragging = false;
    Vector3 start;
    Vector3 end;
    public GUIStyle mouseDragSkin;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        // Start Dragging
        if (Input.GetMouseButton(0))
        {
            if (dragging == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer);
                start = hit.point;
                dragging = true;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer);
                end = hit.point;
            }
            
        }
        // Stop dragging
        if (!Input.GetMouseButton(0))
        {
            if (dragging)
            {
                dragging = false;
                //Collider[] col = 

            }
        }
        // Handle Drag
        
    }
    private void OnGUI()
    {

        if (dragging)
        {
            float width = Camera.main.WorldToScreenPoint(start).x - Camera.main.WorldToScreenPoint(end).x;
            float height = Camera.main.WorldToScreenPoint(start).y - Camera.main.WorldToScreenPoint(end).y;
            float left = Input.mousePosition.x;
            float top = (Screen.height - Input.mousePosition.y) - height;

            GUI.Box(new Rect(left, top, width, height), "", mouseDragSkin); 
        }
    }
}
