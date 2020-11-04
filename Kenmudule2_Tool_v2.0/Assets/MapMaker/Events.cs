using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class Events
{
    protected Vector3 tempStartPos;
    protected Vector3 tempEndPos;

    public GameObject PlaceObject(Object targetObject, Vector3 position, Quaternion rotation, GameObject parentObject)
    {
        if (targetObject != null)
        {
            //targetObject = UnityEngine.Object.Instantiate(targetObject, position, rotation);

            GameObject newObject = UnityEngine.Object.Instantiate(targetObject, position, rotation) as GameObject;
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
    protected Vector3 GetDataRay(int activefloor)
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);

        RaycastHit hit;
        //Get vector3 when on ActiveFloor height
        float dstToDrawPlane = (activefloor - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        

        if (Physics.Raycast(mouseRay, out hit))//if Ray hits something
        {
            if (hit.collider.gameObject.GetComponent<Tile>() == true)
            {
                if(hit.collider.transform.position.y == activefloor)
                {
                    //Debug.Log("Hit: " + hit.collider.transform.position);
                    return hit.collider.transform.position;
                }
                
            }
        }
        //Debug.Log("MousePosition: " + mousePosition);
        return mousePosition;

    }
    public void SetStartPos()
    {
        if (GetDataRay(MapMaker.activefloor) != Vector3.zero)
        {
            tempStartPos = GetDataRay(MapMaker.activefloor);
        }
    }
    public void SetEndPos()
    {
        if (GetDataRay(MapMaker.activefloor) != Vector3.zero)
        {
            tempEndPos = GetDataRay(MapMaker.activefloor);
        }
    }










}
