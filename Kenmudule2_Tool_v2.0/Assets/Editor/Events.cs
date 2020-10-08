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
    public void PlaceObject(GameObject targetObject ,Vector3 position, Quaternion rotation)
    {
        GameObject newTile = UnityEngine.Object.Instantiate(targetObject, position, rotation);
    }
    public void DestroyObject(GameObject targetObject)
    {
        UnityEngine.Object.DestroyImmediate(targetObject);
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
    
    

    


}
