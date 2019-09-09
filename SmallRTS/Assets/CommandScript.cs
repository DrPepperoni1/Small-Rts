using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandScript : MonoBehaviour
{
    public LayerMask selectionLayer;
   
    void Update()
    {
        if (Input.GetButton("MouseRight"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, selectionLayer);
            SendMoveCommand(hit.point);

        }
    }
    private void SendMoveCommand(Vector3 _position)
    {
        Debug.Log("sending move command to " + UnitSelection.SelectedUnits.Count + " units");
        for (int i = 0; i < UnitSelection.SelectedUnits.Count; i++)
        {
            Unit unit = UnitSelection.SelectedUnits[i].GetComponent<Unit>();
            unit.Move(_position);
        }
    }
}
