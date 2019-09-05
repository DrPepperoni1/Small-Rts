using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public static List<GameObject> selectedUnits = new List<GameObject>();
    public static List<GameObject> unitsOnScreen = new List<GameObject>();
    

    public LayerMask unitLayer;
    public LayerMask selectionPlane;
    bool dragging = false;
    Vector3 GUIstart;
    Vector3 GUIend;
    Vector2 collisionBoxStart;
    Vector2 collisionBoxEnd;
    public GUIStyle mouseDragSkin;

    // GuiVariables
    float width;
    float height;
    float left;
    float top;

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
                Physics.Raycast(ray, out hit, Mathf.Infinity, selectionPlane);
                GUIstart = hit.point;
                GUIend = hit.point;
                dragging = true;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity, selectionPlane);
                GUIend = hit.point;
            }
            width = Camera.main.WorldToScreenPoint(GUIstart).x - Camera.main.WorldToScreenPoint(GUIend).x;
            height = Camera.main.WorldToScreenPoint(GUIstart).y - Camera.main.WorldToScreenPoint(GUIend).y;
            left = Input.mousePosition.x;
            top = (Screen.height - Input.mousePosition.y) - height;
            Debug.Log(width + " " + height);

            if (width > 0f && height < 0f)
            {
                collisionBoxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            else if (width > 0f && height > 0f)
            {
                collisionBoxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + height);
            }
            else if (width < 0f && height < 0f)
            {
                collisionBoxStart = new Vector2(Input.mousePosition.x + width, Input.mousePosition.y);
            }
            else if (width < 0f && height > 0f)
            {
                collisionBoxStart = new Vector2(Input.mousePosition.x + width, Input.mousePosition.y + height);
            }
            collisionBoxEnd = new Vector2(
                        collisionBoxStart.x + Unsigned(width),
                        collisionBoxStart.y - Unsigned(height)
            );

            Debug.Log("start = " + collisionBoxStart + "end = " + collisionBoxEnd);
            
        }
        // Stop dragging
        if (!Input.GetMouseButton(0))
        {
            if (dragging)
            {
                dragging = false;
                 

            }
        }


        

    }
    private void LateUpdate()
    {
        foreach (GameObject obj in unitsOnScreen)
        {
            Debug.Log(obj);
        }
    }
    private void OnGUI()
    {

        if (dragging)
        {
            GUI.Box(new Rect(left, top, width, height), "", mouseDragSkin); 
        }
    }
    public static float Unsigned(float val)
    {
        if (val < 0f)
        {
            val *= -1;
        }
        return val;
    }
    public static bool IsUnitOnScreen(Vector2 unitScreenSpace)
    {
        if ((unitScreenSpace.x < Screen.width && unitScreenSpace.y < Screen.height)&&
           (unitScreenSpace.x > 0f && unitScreenSpace.y > 0f))
        {
            return true;
        }
        return false;
    }
    public static void RemoveUnitFromScreenList(GameObject unit)
    {
        for (int i = 0; i < unitsOnScreen.Count; i++)
        {
            GameObject unitObj = unitsOnScreen[i];
            if (unit == unitObj)
            {
                unitsOnScreen.RemoveAt(i);
                return;
            }
        }
        return;
    }
}
