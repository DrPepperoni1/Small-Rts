using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Unit Base class 
public class Unit : MonoBehaviour
{
    #region selectionVariables
    public Vector2 screenSpacePosition;
    public bool onScreen;
    public bool selected = false;
    #endregion

    public virtual void Update()
    {
        if (!selected)
        {
            screenSpacePosition = Camera.main.WorldToScreenPoint(transform.position);
            if (UnitSelection.IsUnitOnScreen(screenSpacePosition))
            {
                if (!onScreen)
                {
                    UnitSelection.unitsOnScreen.Add(gameObject);
                    onScreen = true;
                }
                
            }
            else
            {
                if (onScreen)
                {
                    UnitSelection.RemoveUnitFromScreenList(gameObject);
                    onScreen = false;
                }
            }
        }
    }
}
