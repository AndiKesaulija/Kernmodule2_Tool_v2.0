using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EventHandler : Events
{
    private bool mouseDown = false;

    private GameObject selectedPreview;
    private MapMaker myMapMaker;
    private Vector3 initialPos;

    public List<bool> selectedList = new List<bool>();
    public int selectedID;
    private GameObject mySelectedBuilding;

    

    public GameObject ShowPreview(GameObject selectedBuilding)
    {
        selectedBuilding = UnityEngine.Object.Instantiate(selectedBuilding, new Vector3(0,0,0),new Quaternion(0,0,0,0));
        

        if (selectedBuilding.GetComponent<Building>() == true)
        {
            selectedBuilding.GetComponent<Building>().reloadMyData();
        }
        if (myParentObject != null)
        {
            selectedBuilding.transform.SetParent(myParentObject.transform);
        }

        return selectedBuilding;
    }
    public GameObject PlaceObject(GameObject selectedBuilding,Vector3 myPos)
    {
        Vector3 newPos = CastRay();
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z));

        selectedBuilding = UnityEngine.Object.Instantiate(selectedBuilding, myPos, new Quaternion(0, 0, 0, 0));


        if (selectedBuilding.GetComponent<Building>() == true)
        {
            selectedBuilding.GetComponent<Building>().reloadMyData();
        }
        if (myParentObject != null)
        {
            selectedBuilding.transform.SetParent(myParentObject.transform);
        }

        return selectedBuilding;
    }
    public void PlaceObjects(GameObject selectedBuilding, Vector3 startPos, Vector3 endPos, float distance)
    {
        Vector3 direction = startPos - endPos;

        for (int i = 0; i < distance; i++)
        {
            if (direction.x > 0)
            {
                PlaceObject(selectedBuilding, new Vector3(startPos.x - i, startPos.y, startPos.z));
            }
            if (direction.x < 0)
            {
                PlaceObject(selectedBuilding, new Vector3(startPos.x + i, startPos.y, startPos.z));
            }
            if (direction.z > 0)
            {
                PlaceObject(selectedBuilding, new Vector3(startPos.x, startPos.y , startPos.z - i));
            }
            if (direction.z < 0)
            {
                PlaceObject(selectedBuilding, new Vector3(startPos.x, startPos.y, startPos.z + i));
            }
        }
        
        

    }

    public void DestroyPreviewObject(GameObject previewObject)
    {
        GameObject.DestroyImmediate(previewObject);
        selectedPreview = null;
    }
    public void HandlePreview(GameObject previewObject)
    {
        if(previewObject != null)
        {
            Vector3 newPos = CastRay();
            newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z));
            previewObject.transform.position = newPos;
        }
    }

    
    public void RotatePreview(GameObject selectedBuilding, Vector3 target)
    {
        //Rotate to Target
        //lookAtPos.y is own y for axis rotation
        if (selectedBuilding != null)
        {

            Vector3 lookAtPos = new Vector3(target.x, selectedBuilding.transform.position.y, target.z);
            Vector3 lookDirection = selectedBuilding.transform.position - lookAtPos;
            Quaternion lookRotation = Quaternion.LookRotation(-lookDirection);

            lookRotation = new Quaternion(Mathf.Round(lookRotation.x), Mathf.Round(lookRotation.y), Mathf.Round(lookRotation.z), Mathf.Round(lookRotation.w));
            tempRotation = lookRotation;
            selectedBuilding.transform.rotation = lookRotation;
        }
    }
    public void PlaceObjects()
    {

    }

    //public void MouseEvent(Vector3 pos,bool snapRotation,GameObject parent)
    //{
    //    Event e = Event.current;

    //    if (myMapMaker == null)
    //    {
    //        myMapMaker = EditorWindow.GetWindow<MapMaker>();
    //    }

    //    //if Object NOT Selected
    //    if (previewObject == null)
    //    {
    //        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
    //        {
    //            if(GetObjectRay() != null)
    //            {
    //                //UnityEngine.Object.DestroyImmediate(GetObjectRay());
    //            }
    //        }

    //    }
    //    //if Object Selected
    //    if (previewObject == null && mySelectedBuilding != null)
    //    {
    //        previewObject = PlaceObject(mySelectedBuilding, pos, tempRotation);
    //    }

    //    if (previewObject != null)
    //    {
    //        if (mouseDown == false)
    //        {
    //            HandlePreview(previewObject, pos);
    //        }

    //        //LeftMouseDown
    //        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
    //        {
    //            initialPos = pos;
    //            mouseDown = true;
    //        }
    //        //LeftMouseDrag
    //        if (mouseDown == true && e.type == EventType.MouseDrag)
    //        {
    //            HandlePreview(previewObject, initialPos);
    //        }
    //        //LeftMouseUp
    //        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
    //        {
    //            //GameObject newObject = PlaceObject(mySelectedBuilding, initialPos, tempRotation, parent);

    //            myMapMaker.myObjectPool.placedObjects.Add(PlaceObject(mySelectedBuilding, initialPos, tempRotation));
    //            mouseDown = false;
    //        }
    //        //RightMouseDown
    //        if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
    //        {
    //            Debug.Log(selectedList);
    //            if (selectedList != null)
    //            {
    //                mySelectedBuilding = null;
    //                DisableTool(selectedList);
    //                DestroyPreview(previewObject);
    //                UnityEditor.EditorWindow.FocusWindowIfItsOpen<MapMaker>();
    //            }
    //        }
    //    }


    //}
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
    
    
    //public void ShowObjectList(List<GameObject> ObjectList)
    //public void ShowObjectList(List<GameObject> ObjectList)
    //{
    //    if (ObjectList != null)
    //    {
    //        for (int i = 0; i < ObjectList.Count; i++)
    //        {
    //            selectedList.Add(false);
    //            selectedList[i] = GUILayout.Toggle(selectedList[i], "" + ObjectList[i].name, "Button");

    //            if (selectedList[i] == true)
    //            {
    //                selectedID = i;
    //                if (mySelectedBuilding != ObjectList[i])
    //                {
                        
    //                    //DestroyPreview(mySelectedBuilding);
    //                    mySelectedBuilding = ObjectList[i];
    //                }


    //                for (int k = 0; k < selectedList.Count; k++)
    //                {
    //                    if (k != i)
    //                    {
    //                        selectedList[k] = false;
    //                    }
    //                }

    //            }
    //        }
           
    //    }
    //}
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
   
    public void DisableTool(List<bool> boolList,GameObject previewObject)
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
