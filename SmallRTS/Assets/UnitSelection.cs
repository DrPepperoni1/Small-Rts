using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> SelectedUnits = new List<GameObject>();
    public static List<GameObject> UnitsOnScreen = new List<GameObject>();
    [SerializeField]
    public static List<GameObject> UnitsInDragg = new List<GameObject>();
    

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

        
        UnitsInDragg.Clear();
        if ((dragging || draggEndedOnThisFrame) && UnitsOnScreen.Count > 0)
        {
            for (int i = 0; i < UnitsOnScreen.Count; i++)
            {
                GameObject unitOBJ = UnitsOnScreen[i] as GameObject;
                Unit unitsScript = unitOBJ.GetComponent<Unit>();
                GameObject selectionOBJ = unitOBJ.transform.Find("SelectionObject").gameObject;
                if (!UnitAlreadyInDragg(unitOBJ))
                {
                    if (IsUnitInsideSelection(unitsScript.screenSpacePosition))
                    {
                        selectionOBJ.SetActive(true);
                        UnitsInDragg.Add(unitOBJ);
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
        for (int i = 0; i < UnitsOnScreen.Count; i++)
        {
            GameObject unitObj = UnitsOnScreen[i];
            if (unit == unitObj)
            {
                UnitsOnScreen.RemoveAt(i);
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
        if (UnitsInDragg.Count > 0)
        {
            for (int i = 0; i < UnitsInDragg.Count; i++)
            {
                GameObject unitGO = UnitsInDragg[i];
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
        if (SelectedUnits.Count > 0)
        {
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                GameObject unitGo = SelectedUnits[i];
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
        if (UnitsInDragg.Count > 0)
        {
            for (int i = 0; i < UnitsInDragg.Count; i++)
            {
                GameObject unit = UnitsInDragg[i] as GameObject;
                if (!UnitAlreadySelected(unit))
                {
                    SelectedUnits.Add(unit);
                    unit.transform.Find("SelectionObject").gameObject.SetActive(true);
                    unit.GetComponent<Unit>().selected = true;
                }
            }
            UnitsInDragg.Clear();
        }
    }
    public static void DeselectAllSelectedUnits()
    {
        if (SelectedUnits.Count > 0)
        {
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                GameObject UnitOBJ = SelectedUnits[i] as GameObject;
                UnitOBJ.transform.Find("SelectionObject").gameObject.SetActive(false);
                UnitOBJ.GetComponent<Unit>().selected = false;
            }
            SelectedUnits.Clear();
        }
    }
}
