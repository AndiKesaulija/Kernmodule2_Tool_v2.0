using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EventHandler : TileHandler
{
    private bool mouseDown = false;

    public void MouseEvent(Event e, GameObject tile,Vector3 pos)
    {
        if (mouseDown == false)
        {
        }

        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown) 
        {
            previewObject = PlaceObject(tile, pos, null);
            
            mouseDown = true;
        }
        //LeftMouseDrag
        if (mouseDown == true && e.type == EventType.MouseDrag)
        {
            HandlePreviewRot(previewObject, previewObject.transform.position, CastRay());
        }
        //LeftMouseUp
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        {

            mouseDown = false;
        }

    }
    Vector3 TilePos(GameObject selectedTile)
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        GameObject myPrefab = selectedTile;

        if (Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider != null)
            {
                Vector3 myPos = hit.collider.transform.position;
                myPos = myPos + new Vector3(0, hit.collider.transform.localScale.y / 2, 0);
                myPos = myPos + new Vector3(0, myPrefab.transform.localScale.y / 2, 0);

                return myPos;
            }
        }
        return Vector3.zero;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)
    public Vector3 CastRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        float drawPlaneHeight = 0;//y axis change if needed
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        return mousePosition;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)

}
