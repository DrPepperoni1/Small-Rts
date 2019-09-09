using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> selectedUnits = new List<GameObject>();
    public static List<GameObject> unitsOnScreen = new List<GameObject>();
    [SerializeField]
    public static List<GameObject> unitsInDragg = new List<GameObject>();
    

    public LayerMask unitLayer;
    public LayerMask selectionPlane;
    bool dragging = false;
    Vector3 GUIstart;
    Vector3 GUIend;
    static Vector2 collisionBoxStart;
    static Vector2 collisionBoxEnd;
    public GUIStyle mouseDragSkin;
    bool draggEndedOnThisFrame = false;

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


            
        }
        // Stop dragging
        if (!Input.GetMouseButton(0))
        {
            if (dragging)
            {
                draggEndedOnThisFrame = true;
                dragging = false;
                 

            }
        }


        

    }
    private void LateUpdate()
    {
        Debug.Log("selected units");
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            Debug.Log(selectedUnits[i]);
        }
        Debug.Log("dragged units");
        for (int i = 0; i < unitsInDragg.Count; i++)
        {
            Debug.Log(unitsInDragg[i]);
        }
        unitsInDragg.Clear();
        if ((dragging || draggEndedOnThisFrame) && unitsOnScreen.Count > 0)
        {
            for (int i = 0; i < unitsOnScreen.Count; i++)
            {
                GameObject unitOBJ = unitsOnScreen[i] as GameObject;
                Unit unitsScript = unitOBJ.GetComponent<Unit>();
                GameObject selectionOBJ = unitOBJ.transform.Find("SelectionObject").gameObject;
                if (!UnitAlreadyInDragg(unitOBJ))
                {
                    if (IsUnitInsideSelection(unitsScript.screenSpacePosition))
                    {
                        selectionOBJ.SetActive(true);
                        unitsInDragg.Add(unitOBJ);
                    }
                    else
                    {
                        if (!UnitAlreadySelected(unitOBJ))
                        {
                            selectionOBJ.SetActive(false);
                        }
                    }
                }
            }
        }
        if (draggEndedOnThisFrame)
        {
            draggEndedOnThisFrame = false;
            SelectAllDraggedUnits();  

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
    public static bool ShiftKeyDown()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return true;
        }
        else return false;
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
    public static bool IsUnitInsideSelection(Vector2 unitPosition)
    {
        if ((unitPosition.x > collisionBoxStart.x && unitPosition.y < collisionBoxStart.y) &&
            (unitPosition.x < collisionBoxEnd.x && unitPosition.y > collisionBoxEnd.y))
        {
            return true;
        }
        else return false;
    }
    public static bool UnitAlreadyInDragg(GameObject unit)
    {
        if (unitsInDragg.Count > 0)
        {
            for (int i = 0; i < unitsInDragg.Count; i++)
            {
                GameObject unitGO = unitsInDragg[i];
                if (unit == unitGO)
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }
    public static bool UnitAlreadySelected(GameObject unit)
    {
        if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                GameObject unitGo = selectedUnits[i];
                if (unit == unitGo)
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }
    public static void SelectAllDraggedUnits()
    {
        if (!ShiftKeyDown())
        {
            DeselectAllSelectedUnits();
        }
        if (unitsInDragg.Count > 0)
        {
            for (int i = 0; i < unitsInDragg.Count; i++)
            {
                GameObject unit = unitsInDragg[i] as GameObject;
                if (!UnitAlreadySelected(unit))
                {
                    selectedUnits.Add(unit);
                    unit.transform.Find("SelectionObject").gameObject.SetActive(true);
                    unit.GetComponent<Unit>().selected = true;
                }
            }
            unitsInDragg.Clear();
        }
    }
    public static void DeselectAllSelectedUnits()
    {
        if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                GameObject UnitOBJ = selectedUnits[i] as GameObject;
                UnitOBJ.transform.Find("SelectionObject").gameObject.SetActive(false);
                UnitOBJ.GetComponent<Unit>().selected = false;
            }
            selectedUnits.Clear();
        }
    }
}
