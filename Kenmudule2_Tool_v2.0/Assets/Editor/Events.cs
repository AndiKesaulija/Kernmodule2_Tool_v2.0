using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class Events
{
    protected Vector3 myTempPos;
    protected Quaternion tempRotation;
    public GameObject myParentObject;

    //Events
    public Vector3 tempPos;
    public Vector3Int tempEndPos;
    public Vector3Int tempStartPos;
    

    public GameObject PlaceObject(GameObject targetObject, Vector3 position, Quaternion rotation, GameObject parentObject)
    {
        if (targetObject != null)
        {
            //targetObject = UnityEngine.Object.Instantiate(targetObject, position, rotation);

            GameObject newObject = UnityEngine.Object.Instantiate(targetObject, position, rotation);
            newObject.name = targetObject.name;
            if (parentObject != null)
            {
                newObject.transform.SetParent(parentObject.transform);
            }
            return newObject;

        }
        return null;

    }

    public void DestroyObject(GameObject targetObject)
    {
        if(targetObject != null)
        {
            UnityEngine.Object.DestroyImmediate(targetObject.gameObject);
        }
    }

    public GameObject HandlePreview(Vector3 position, int size, Material myMaterial)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(size, 1, size);
        plane.transform.localPosition = position + new Vector3(0,0.1f,0);
        plane.transform.rotation = new Quaternion(0, 180, 0, 0);

        plane.GetComponent<MeshRenderer>().material = myMaterial;

        return plane;
    }
    
    public void HideObjects(List<GameObject> myObjects,bool myBool)
    {
        foreach(GameObject foundObjects in myObjects)
        {
            foundObjects.SetActive(myBool);
        }
    }
    public void FocusObject(GameObject target)
    {
        Selection.activeGameObject = target;
        SceneView.FrameLastActiveSceneView();
        Selection.activeGameObject = null;
    }
    public Vector3 GetDataRay(int activeFloor)
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        //Get vector3 when on ActiveFloor height
        float drawPlaneHeight = activeFloor;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);
        mousePosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), Mathf.Round(mousePosition.z));

        if (Physics.Raycast(mouseRay, out hit))//if Ray hits something
        {
            if (hit.collider.tag == "Tile")
            {
                if (hit.collider.gameObject.GetComponent<Tile>() != null)
                {
                    if (hit.collider.transform.position.y == activeFloor)
                    {
                        return hit.collider.transform.position;
                    }
                    return mousePosition;
                }
                return mousePosition;
            }
            return mousePosition;

        }
        Debug.Log("NoHit");
        return Vector3.zero;
        
    }

    public void SetStartPos(int activeFloor)
    {
        tempStartPos = Vector3Int.FloorToInt(GetDataRay(activeFloor));
    }
    public void SetEndPos(int activeFloor)
    {
        tempEndPos = Vector3Int.FloorToInt(GetDataRay(activeFloor));
    }

    
    

    

   



}
