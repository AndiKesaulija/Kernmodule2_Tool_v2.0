using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Events
{
    /// <summary>
    /// All events used by MapMaker.
    /// </summary>

    protected Vector3 myTempPos;
    protected Quaternion tempRotation;
    public GameObject myParentObject;   

    public bool snapBuilding;
    //public GameObject previewObject;
    //public GameObject PlaceLine(Vector3 pointA, Vector3 pointB)
    //{

    //}
    public GameObject PlaceObject(GameObject selectedBuilding, Vector3 pos, Quaternion rot)
    {
        selectedBuilding = UnityEngine.Object.Instantiate(selectedBuilding,pos,rot);

        if (selectedBuilding.GetComponent<Building>() == true)
        {
            selectedBuilding.GetComponent<Building>().reloadMyData();
        }
        if(myParentObject != null)
        {
            selectedBuilding.transform.SetParent(myParentObject.transform);
        }

        myTempPos = pos;

        return selectedBuilding;
    }
  
    public void HandlePreview(GameObject selectedBuilding, Vector3 myPos)
    {
        if(selectedBuilding != null)
        {
            selectedBuilding.transform.position = myPos;
            selectedBuilding.transform.rotation = tempRotation;
        }
    }
    

    //public void RotatePreview(GameObject selectedBuilding, Vector3 myPos, Vector3 target)
    //{
    //    //Rotate to Target
    //    //lookAtPos.y is own y for axis rotation
    //    if (selectedBuilding != null)
    //    {
    //        selectedBuilding.transform.position = myPos;

    //        Vector3 lookAtPos = new Vector3(target.x, selectedBuilding.transform.position.y, target.z);
    //        Vector3 lookDirection = selectedBuilding.transform.position - lookAtPos;
    //        Quaternion lookRotation = Quaternion.LookRotation(-lookDirection);

    //        if (snapBuilding == true)
    //        {
    //            lookRotation = new Quaternion(Mathf.Round(lookRotation.x), Mathf.Round(lookRotation.y), Mathf.Round(lookRotation.z), Mathf.Round(lookRotation.w));
    //        }

    //        tempRotation = lookRotation;
    //        selectedBuilding.transform.rotation = lookRotation;
    //    }
    //}
    public Vector3 CastRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        float drawPlaneHeight = 0;//y axis change if needed
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        return mousePosition;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)
    public Vector3 CastRoundRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        float drawPlaneHeight = 0;//y axis change if needed
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        mousePosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), Mathf.Round(mousePosition.z));


        return mousePosition;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)
    public GameObject GetObjectRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)
    public void DestroyPreview(GameObject selectedBuilding)
    {
        Debug.Log("Destroy");
        UnityEngine.Object.DestroyImmediate(selectedBuilding);
    }

    


}
