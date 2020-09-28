using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EventHandler : TileHandler
{
    private bool mouseDown = false;
    public GameObject previewObject;
    private MapMaker myMapMaker;
    private Vector3 initialPos;

    public List<bool> selectedList = new List<bool>();
    public int selectedID;
    private GameObject mySelectedBuilding;

   
    public void MouseEvent(Vector3 pos,bool snapRotation,GameObject parent)
    {
        Event e = Event.current;

        if (myMapMaker == null)
        {
            myMapMaker = EditorWindow.GetWindow<MapMaker>();
        }

        //if Object NOT Selected
        if (previewObject == null)
        {
            if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
            {
                if(GetObjectRay() != null)
                {
                    //UnityEngine.Object.DestroyImmediate(GetObjectRay());
                }
            }

        }
        //if Object Selected
        if (previewObject == null && mySelectedBuilding != null)
        {
            previewObject = PlaceObject(mySelectedBuilding, pos, tempRotation, null);
        }
        
        if (previewObject != null)
        {
            if (mouseDown == false)
            {
                HandlePreviewPos(previewObject, pos);
            }

            //LeftMouseDown
            if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
            {
                initialPos = pos;
                mouseDown = true;
            }
            //LeftMouseDrag
            if (mouseDown == true && e.type == EventType.MouseDrag)
            {
                HandlePreviewRot(previewObject, initialPos, CastRay(), snapRotation);
            }
            //LeftMouseUp
            if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
            {
                //GameObject newObject = PlaceObject(mySelectedBuilding, initialPos, tempRotation, parent);

                myMapMaker.myObjectPool.placedObjects.Add(PlaceObject(mySelectedBuilding, initialPos, tempRotation, parent));
                mouseDown = false;
            }
            //RightMouseDown
            if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
            {
                Debug.Log(selectedList);
                if (selectedList != null)
                {
                    mySelectedBuilding = null;
                    DisableTool(selectedList);
                    DestroyPreview(previewObject);
                    UnityEditor.EditorWindow.FocusWindowIfItsOpen<MapMaker>();
                }
            }
        }
       

    }
    public Vector3 TilePos(GameObject selectedTile)
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        GameObject myPrefab = selectedTile;

        if (Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider != null && (hit.collider.tag == "Tile" || hit.collider.tag == "GridTile"))
            {
                
                Vector3 myPos = hit.collider.transform.position;
                myPos = myPos + new Vector3(0, hit.collider.transform.localScale.y, 0);
                
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
    public GameObject GetObjectRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider != null && hit.collider.tag == "Tile")
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)
    //public void ShowObjectList(List<GameObject> ObjectList)
    public void ShowObjectList(List<GameObject> ObjectList)
    {
        if (ObjectList != null)
        {
            for (int i = 0; i < ObjectList.Count; i++)
            {
                selectedList.Add(false);
                selectedList[i] = GUILayout.Toggle(selectedList[i], "" + ObjectList[i].name, "Button");

                if (selectedList[i] == true)
                {
                    selectedID = i;
                    if (mySelectedBuilding != ObjectList[i])
                    {
                        
                        //DestroyPreview(mySelectedBuilding);
                        mySelectedBuilding = ObjectList[i];
                    }


                    for (int k = 0; k < selectedList.Count; k++)
                    {
                        if (k != i)
                        {
                            selectedList[k] = false;
                        }
                    }

                }
            }
           
        }
    }
    public bool CheckActive(List<bool> boolList)
    {
        foreach(bool mybool in boolList)
        {
            if(mybool == true)
            {
                return true;

            }
        }

        return false;
    }
   
    public void DisableTool(List<bool> boolList)
    {
        for (int i = 0; i < boolList.Count; i++)
        {
            boolList[i] = false;
        }
        UnityEngine.Object.DestroyImmediate(previewObject);
    }
    public void DestroyPlacedObjects(List<GameObject> myObjects)
    {
        foreach (GameObject building in myObjects)
        {
            UnityEngine.Object.DestroyImmediate(building);
        }
        myMapMaker.myObjectPool.Reload();
        //myMapMaker.myObjectPool.myData.myBuildingData.Clear();
        //myMapMaker.myObjectPool.placedObjects.Clear();
    }
}
