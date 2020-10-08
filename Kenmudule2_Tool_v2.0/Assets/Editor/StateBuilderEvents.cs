using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class StateBuilderEvents 
{
    //Building
    private GameObject tempBuilding;
    private static int size = 4;
    private int selectedNum;
    private int activeFloor = 0;

    private Building myBuilding = new Building(size);
    private GameObject[] floors = new GameObject[size];

    private bool buildingToggle = false;

    //Events
    public Vector3 tempPos;
    public BuildingData tempStartPos;

    public StateBuilderEvents(MapMaker owner)
    {
        this.owner = owner;
    }
    private MapMaker owner;

    public void InitiateNewfloor()
    {
        Debug.Log(myBuilding.myData.Length);

        activeFloor = 0;

        DestroyBuilding();

        tempBuilding = new GameObject("TempBuilding");

        int num = 0;
        for (int i = 0; i < size; i++)
        {

            floors[i] = new GameObject("Floor" + i);
            floors[i].transform.SetParent(tempBuilding.transform);

            for (int j = 0; j < size; j++)
            {
                
                for (int k = 0; k < size; k++)
                {
                    myBuilding.myData[j, i, k] = new BuildingData();
                    myBuilding.myData[j, i, k].myID = num;
                    if (i == 0)
                    {
                        myBuilding.myData[j, i, k].prefabID = 2;//Floor Start
                    }
                    else
                    {
                        myBuilding.myData[j, i, k].prefabID = 1;//EmptyTile
                    }

                    myBuilding.myData[j, i, k].myFloorNum = i;
                    myBuilding.myData[j, i, k].xArrayPos = j;
                    myBuilding.myData[j, i, k].zArrayPos = k;
                    myBuilding.myData[j, i, k].gridPos = new Vector3Int(j, i, k);

                    myBuilding.myData[j, i, k].buildingRot = new Quaternion(0, 0, 0, 0);

                    myBuilding.myData[j, i, k].myObject = SetTile(myBuilding.myData[j, i, k]);
                    myBuilding.myData[j, i, k].myPosition = myBuilding.myData[j, i, k].myObject.transform.position;

                    myBuilding.myData[j, i, k].myObject.transform.SetParent(floors[i].transform);
                    num = num + 1;
                }
                

            }
            floors[i].SetActive(false);
        }
        floors[0].SetActive(true);
        activeFloor = 1;
        floors[1].SetActive(true);


    }

    public void DestroyBuilding()
    {
        if (tempBuilding != null)
        {
            UnityEngine.GameObject.DestroyImmediate(tempBuilding);
        }
    }

    public void SetDistance()
    {
        if(GetObjectRay() != null)
        {
            tempPos = GetObjectRay().transform.position;
        }
    }
    public void PlaceNewTile(Vector3 startPosition, Vector3 endPosition, int selectedBuildingID)
    {
        Quaternion Lookrotation = new Quaternion();
        int count = Mathf.RoundToInt(Vector3.Distance(startPosition, endPosition));
        Vector3 difference = new Vector3(startPosition.x - endPosition.x, startPosition.y - endPosition.y, startPosition.z - endPosition.z);
        Debug.Log("Start Position: " + startPosition + " End Position: " + endPosition + " Count: " + count + " Difference: " + difference + " Vector: " + (difference/count));

        for (int i = 0; i < count; i++)
        {
            Vector3 tilePosition = startPosition - (difference / count * i);
            //Debug.Log("Position: " + tilePosition);

            Vector3 LookDirection = tilePosition - endPosition;
            Lookrotation = Quaternion.LookRotation(LookDirection);
            //Debug.Log("Rotation: " + (Lookrotation));

            for (int j = 0; j < size; j++)
            {
                if(myBuilding.myData[i, activeFloor, j].myPosition == tilePosition)
                {
                    //Debug.Log("Match: " + myBuilding.myData[j].myID + " Position: " + tilePosition);
                    //PlaceObject(myBuilding.myData[i, activeFloor, j].myObject, Lookrotation, selectedBuildingID);
                }
            }
        }
        //Place object on EndPositon
        //Debug.Log(endPosition + " : End Position");

        //for (int j = 0; j < myBuilding.myData.Length; j++)
        //{
        //    if (myBuilding.myData[j].myPosition == endPosition)
        //    {
        //        //Debug.Log("Match: " + myBuilding.myData[j].myID + " Position: " + endPosition);
        //        PlaceObject(myBuilding.myData[j].myObject, Lookrotation, selectedBuildingID);
        //    }
            
        //}

    }
    public void SetStartPos()
    {
        if (GetDataRay() != null)
        {
            tempStartPos = GetDataRay();
        }
    }
    public void PlaceTiles(int selectedBuildingID)
    {
        BuildingData startPos = tempStartPos;
        BuildingData endPos = GetDataRay();


        

        if (startPos != null && endPos != null)
        {
            int count = Mathf.RoundToInt(Vector3Int.Distance(startPos.gridPos, endPos.gridPos));
            Vector3Int difference = startPos.gridPos - endPos.gridPos;

            Quaternion Lookrotation = new Quaternion();
            //Debug.Log("StartPos: " + startPos.gridPos + " EndPos: " + endPos.gridPos + " Count: " + count + " Difference: " + difference);


            if (count > 0)
            {
                for (int i = 0; i <= count; i++)
                {
                    Vector3Int tilePosition = startPos.gridPos - (difference / count * i);
                    Vector3 LookDirection = tilePosition - endPos.gridPos;
                    if (LookDirection != Vector3.zero)
                    {
                        Lookrotation = Quaternion.LookRotation(LookDirection);
                    }

                    PlaceObject(myBuilding.myData[tilePosition.x, tilePosition.y, tilePosition.z].myObject, Lookrotation, selectedBuildingID);

                }
            }
            else
            {
                PlaceObject(myBuilding.myData[startPos.xArrayPos,startPos.myFloorNum,startPos.zArrayPos].myObject, Lookrotation, selectedBuildingID);
                //Debug.Log("SingelClick");

            }
        }
        else
        {
            Debug.Log("Out of bounds");
        }

        tempStartPos = null;


        //Debug.Log("StartPos: " + startPos[0, 0, 0] + " EndPos: " + endPos + " SelectedBuildingID: " + selectedBuildingID);
    }
    public void CheckFloor()
    {
        

        for (int j = 0; j < size; j++)
        {
            for (int k = 0; k < size; k++)
            {

                bool top = false;
                bool bottom = false;
                bool left = false;
                bool right = false;

                if (j - 1 >= 0)
                {
                    if (myBuilding.myData[j, activeFloor, k].prefabID == 4 && myBuilding.myData[j - 1, activeFloor, k].prefabID == 4)
                    {
                        //Debug.Log("Top?");
                        top = true;
                    }
                }
                if(j + 1 < size)
                {
                    if (myBuilding.myData[j, activeFloor, k].prefabID == 4 && myBuilding.myData[j + 1, activeFloor, k].prefabID == 4)
                    {
                        //Debug.Log("Bottom?");
                        bottom = true;
                    }
                }
                
                if (k - 1 >= 0)
                {
                    if (myBuilding.myData[j, activeFloor, k].prefabID == 4 && myBuilding.myData[j, activeFloor, k - 1].prefabID == 4)
                    {
                        //Debug.Log("Left?");
                        left = true;
                    }
                    
                }
                if(k + 1 < size)
                {
                    if (myBuilding.myData[j, activeFloor, k].prefabID == 4 && myBuilding.myData[j, activeFloor, k + 1].prefabID == 4)
                    {
                        //Debug.Log("Right?");
                        right = true;
                    }
                }
                if (top == true && (left == true || right == true))
                {
                    Debug.Log("InitiateCorner");

                    myBuilding.myData[j, activeFloor, k].prefabID = 0;
                    PlaceObject(myBuilding.myData[j, activeFloor, k].myObject, Quaternion.identity, myBuilding.myData[j, activeFloor, k].prefabID);
                }
            }
        }
        
    }
    

    public void PlaceObject(GameObject targetObject,Quaternion lookRoation, int selectedObject)
    {
        if (targetObject != null)
        {
            if (targetObject.GetComponent<Tile>() != null)
            {
                BuildingData tempData = targetObject.GetComponent<Tile>().myData;
                if (tempData.myFloorNum == activeFloor)
                {
                    UnityEngine.GameObject.DestroyImmediate(myBuilding.myData[tempData.xArrayPos,tempData.myFloorNum,tempData.zArrayPos].myObject);//Destroy GameObject.
                    tempData.prefabID = selectedObject;
                    tempData.buildingRot = lookRoation;
                    tempData.myObject = SetTile(tempData);//Initiate new GameObject at same position.
                    myBuilding.myData[tempData.xArrayPos, tempData.myFloorNum, tempData.zArrayPos] = tempData;

                    tempData.myObject.transform.SetParent(floors[activeFloor].transform);

                    CheckFloor();
                }
            }
            else
            {
                Debug.Log("No Tile");
            }

        }
    }
    public GameObject SetTile(BuildingData target)
    {
        GameObject newTile = UnityEngine.Object.Instantiate(owner.myObjectPool.objectTiles[target.prefabID], new Vector3(target.xArrayPos, target.myFloorNum, target.zArrayPos), target.buildingRot);

        if (newTile.GetComponent<Tile>() != null)
        {
            newTile.GetComponent<Tile>().myData = target;
        }
        return newTile;
    }


    public void SwitchFloor(bool up)
    {
        if (up == true && activeFloor < 3)
        {
            activeFloor = activeFloor + 1;
        }
        if (up == false && activeFloor > 0)
        {
            activeFloor = activeFloor - 1;
        }

        for (int i = 0; i < floors.Length; i++)
        {
            if (i > activeFloor)
            {
                floors[i].SetActive(false);
            }
        }
        floors[activeFloor].SetActive(true);
    }
    public void ToggleBuilding()
    {
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i].SetActive(!buildingToggle);
        }

        buildingToggle = !buildingToggle;
        if (floors[activeFloor].activeInHierarchy == false)
        {
            for (int i = 0; i < floors.Length; i++)
            {
                if (i <= activeFloor)
                {
                    floors[i].SetActive(true);
                }
            }

        }

    }

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
    public BuildingData GetDataRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit))
        {
            //Debug.DrawRay(hit.transform.position, new Vector3(0,20,0));

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Tile>() != null)
                {
                    return hit.collider.gameObject.GetComponent<Tile>().myData;
                }
                return null;
            }
            return null;
        }
        return null;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)

    public void SaveObject(string buildingName)
    {
        //myMapMakerWindow.generatedObjects.Add(tempBuilding);
        //PrefabUtility.CreatePrefab("Assets/Resources/Buildings", tempBuilding);

        tempBuilding.tag = "Building";

        bool CheckObjectList(string name)
        {

            foreach (GameObject building in owner.myObjectPool.generatedObjects)
            {
                if (building.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        if (CheckObjectList(buildingName) == true)//Replace with error message/are you sure
        {
            PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab");
        }
        else
        {
            owner.myObjectPool.generatedObjects.Add(PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab"));
        }

        owner.myObjectPool.ReloadAssets();
    }
}
