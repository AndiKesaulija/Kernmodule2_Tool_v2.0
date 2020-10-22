using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;



public class StateBuilderEvents : Events
{
    //Building
    private static int size = 6;
    public int activeFloor = 0;
    private int floorsize = 1;

    private Building myBuilding = new Building(size);
    private GameObject[] floors = new GameObject[size];

    private bool buildingToggle = false;

    //UI
    public List<bool> selectedList = new List<bool>();
    public int mySelectedBuidlingID;
    private GameObject mySelectedBuilding;


    public StateBuilderEvents(MapMaker myMapMaker)
    {
        this.myMapMaker = myMapMaker;
    }
    private MapMaker myMapMaker;


    public void InitiateNewfloor(DataWrapper<TileData> BuildingData)
    {
        activeFloor = 0;

        if(myMapMaker.myObjectPool.tempBuilding != null)
        {
            DestroyObject(myMapMaker.myObjectPool.tempBuilding);
        }
        if (myMapMaker.myObjectPool.RotateImage != null)
        {
            DestroyObject(myMapMaker.myObjectPool.RotateImage);
        }

        myMapMaker.myObjectPool.tempBuilding = new GameObject("TempBuilding");
        myMapMaker.myObjectPool.tempBuilding.tag ="Building";




        if (BuildingData == null)//initiate new Building (Clean)
        {
            for (int i = 0; i < size; i++)
            {

                floors[i] = new GameObject("Floor" + i);
                floors[i].transform.SetParent(myMapMaker.myObjectPool.tempBuilding.transform);
                floors[i].SetActive(false);

                for (int j = 0; j < size; j++)
                {

                    for (int k = 0; k < size; k++)
                    {
                        myBuilding.myData[j, i, k] = new TileData();
                        if (i == 0)
                        {
                            myBuilding.myData[j, i, k].ObjectID = 1;//set Floor 0 Objects to floorTile
                        }
                        else
                        {
                            myBuilding.myData[j, i, k].ObjectID = 0;//EmptyTile
                        }

                        myBuilding.myData[j, i, k].myFloorNum = i;
                        myBuilding.myData[j, i, k].xArrayPos = j;
                        myBuilding.myData[j, i, k].zArrayPos = k;
                        myBuilding.myData[j, i, k].gridPos = new Vector3Int(j, i, k);


                        myBuilding.myData[j, i, k].myObject = PlaceObject(myMapMaker.myObjectPool.objectTiles[myBuilding.myData[j, i, k].ObjectID], myBuilding.myData[j, i, k].gridPos, Quaternion.identity, floors[activeFloor]);

                        myBuilding.myData[j, i, k].myObject.transform.SetParent(floors[i].transform);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                floors[i] = new GameObject("Floor" + i);
                floors[i].transform.SetParent(myMapMaker.myObjectPool.tempBuilding.transform);
                floors[i].SetActive(false);

                for (int j = 0; j < size; j++)
                {

                    for (int k = 0; k < size; k++)
                    {
                        myBuilding.myData[j, i, k] = new TileData();
                        
                        myBuilding.myData[j, i, k].ID = myBuilding.myData.Length;
                        myBuilding.myData[j, i, k].myFloorNum = i;
                        myBuilding.myData[j, i, k].xArrayPos = j;
                        myBuilding.myData[j, i, k].zArrayPos = k;
                        myBuilding.myData[j, i, k].gridPos = new Vector3Int(j, i, k);

                    }
                }
            }
            for (int l = 0; l < BuildingData.myData.Count; l++)
            {
                myBuilding.myData[BuildingData.myData[l].xArrayPos, BuildingData.myData[l].myFloorNum, BuildingData.myData[l].zArrayPos].ObjectID = BuildingData.myData[l].ObjectID;

                myBuilding.myData[BuildingData.myData[l].xArrayPos, BuildingData.myData[l].myFloorNum, BuildingData.myData[l].zArrayPos].myObject = PlaceObject(myMapMaker.myObjectPool.objectTiles[BuildingData.myData[l].ObjectID], BuildingData.myData[l].gridPos, BuildingData.myData[l].myRotation, floors[BuildingData.myData[l].myFloorNum]);
            }
        }

        floors[0].SetActive(true);
        activeFloor = 1;
        floors[1].SetActive(true);

        Vector3 center = myMapMaker.myObjectPool.tempBuilding.transform.position + new Vector3((size / 2) - 0.5f, 0, (size / 2) - 0.5f);
        myMapMaker.myObjectPool.RotateImage = HandlePreview(center, size / 6, myMapMaker.myObjectPool.buildingRotationMat);
    }


    public void PlaceTiles(int selectedBuildingID)
    {
        Vector3Int difference = new Vector3Int();
        Vector3Int tilePosition = new Vector3Int();
        Quaternion Lookrotation = new Quaternion();

        
        int count = Mathf.RoundToInt(Vector3Int.Distance(tempStartPos, tempEndPos));
            
        difference = tempStartPos - tempEndPos;

            
        Debug.Log("StartPos: " + tempStartPos + " EndPos: " + tempEndPos + " Count: " + count + " Difference: " + difference + " SelectedBuildingID: " + selectedBuildingID );
        Debug.Log(tilePosition + " : " + activeFloor);

        if(tempStartPos != Vector3.zero)
        {
            if (count > 0)
            {
                for (int i = 0; i <= count; i++)
                {

                    tilePosition = tempStartPos - (difference / count) * i;
                    Vector3Int LookDirection = Vector3Int.FloorToInt(tilePosition - tempEndPos);


                    if (LookDirection != Vector3.zero)
                    {
                        Lookrotation = Quaternion.LookRotation(LookDirection);
                    }
                    if (tilePosition.x >= 0 && tilePosition.x < size && tilePosition.x >= 0 && tilePosition.x < size &&
                        tilePosition.z >= 0 && tilePosition.z < size && tilePosition.z >= 0 && tilePosition.z < size)
                    {
                        if (selectedBuildingID == 2)//wall
                        {
                            SetTileData(myBuilding.myData[tilePosition.x, tilePosition.y, tilePosition.z], Lookrotation, selectedBuildingID);

                        }
                        else
                        {
                            if (i == 0)
                            {
                                SetTileData(myBuilding.myData[tilePosition.x, tilePosition.y, tilePosition.z], Lookrotation, selectedBuildingID);
                            }
                        }
                    }
                    else
                    {
                        if (selectedBuildingID == 2)//wall
                        {
                            Debug.Log("OutOfBounds: " + tilePosition + " Count: " + i);
                        }
                    }
                }
            }
            else
            {
                if (tempStartPos.x >= 0 && tempStartPos.x < size && tempStartPos.x >= 0 && tempStartPos.x < size &&
                    tempStartPos.z >= 0 && tempStartPos.z < size && tempStartPos.z >= 0 && tempStartPos.z < size)
                {
                    SetTileData(myBuilding.myData[tempStartPos.x, tempStartPos.y, tempStartPos.z], Lookrotation, selectedBuildingID);
                }
            }
        }
        
        
        tempStartPos = Vector3Int.zero;
        tempEndPos = Vector3Int.zero;
    }
    public void SetTileData(TileData targetObject, Quaternion lookRoation, int selectedBuildingID)
    {
        if (targetObject != null)
        {
            Vector3 tilePosition = targetObject.gridPos;
            TileData tempData = targetObject;

            tempData.ObjectID = selectedBuildingID;

          
            UnityEngine.GameObject.DestroyImmediate(myBuilding.myData[tempData.xArrayPos, tempData.myFloorNum, tempData.zArrayPos].myObject);//Destroy GameObject.
            tempData.myObject = PlaceObject(myMapMaker.myObjectPool.objectTiles[selectedBuildingID], tilePosition, lookRoation, floors[activeFloor]);//Init New GameObject

            myBuilding.myData[tempData.xArrayPos, tempData.myFloorNum, tempData.zArrayPos] = tempData;
            
        }
        else
        {
            Debug.Log("Target = Null");
        }
    }
    public void SwitchFloor(bool up)
    {
        if (up == true && activeFloor < size-1)
        {
            activeFloor = activeFloor + floorsize;
        }
        if (up == false && activeFloor > 0)
        {
            activeFloor = activeFloor - floorsize;
        }

        for (int i = 0; i < floors.Length; i++)
        {
            if (i > activeFloor)
            {
                floors[i].SetActive(false);
            }
        }
        Debug.Log(activeFloor);
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

                    if (mySelectedBuilding != ObjectList[i])
                    {
                        mySelectedBuidlingID = i;
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
    public void SaveBuilding(string saveName, string folder, DataWrapper<TileData> Data)
    {

        foreach(TileData data in myBuilding.myData)
        {
            if (data.ObjectID == 0)
            {
                DestroyObject(data.myObject);
            }
        }
        AssetDatabase.Refresh();
        
        Object[] mySaves = Resources.LoadAll<TextAsset>("Saves/" + folder + "/");

        string path = "Assets/Resources/Saves/" + folder + "/" + saveName + ".txt";
        StreamWriter writer = new StreamWriter(path, false);


        string myJson = JsonUtility.ToJson(Data, false);

        bool CheckObjectList(string name)
        {

            for (int i = 0; i < mySaves.Length; i++)
            {
                if (mySaves[i].name == name)
                {
                    return true;
                }
            }
            return false;

        }

        if (CheckObjectList(saveName) == true)//Replace with error message/are you sure
        {
            Debug.Log("OverWrite: " + saveName);
            PrefabUtility.SaveAsPrefabAsset(myMapMaker.myObjectPool.tempBuilding, "Assets/Resources/Prefabs/Buildings/" + saveName + ".prefab");
            writer.Write(myJson);
        }
        else
        {
            myMapMaker.myObjectPool.buildings.Add(myMapMaker.myObjectPool.buildings.Count,PrefabUtility.SaveAsPrefabAsset(myMapMaker.myObjectPool.tempBuilding, "Assets/Resources/Prefabs/Buildings/" + saveName + ".prefab"));
            writer.Write(myJson);
        }

        writer.Close();
        AssetDatabase.Refresh();
    }



}
