using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;



public class StateBuilderEvents : Events
{
    //Building
    private static int size = 8;// +2 for Outerring 
    public int activeFloor = 0;
    private int floorsize = 1;

    public Building myBuilding = new Building(size);
    private GameObject[] floors = new GameObject[size];

    private bool buildingToggle = false;

    //UI
    public List<bool> selectedList = new List<bool>();
    //public int mySelectedBuidlingID;
    private GameObject mySelectedBuilding;


    public StateBuilderEvents(MapMaker myMapMaker)
    {
        this.myMapMaker = myMapMaker;
    }
    private MapMaker myMapMaker;


    public void CreateNewBuilding(DataWrapper<TileData> BuildingData)
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
        myMapMaker.myObjectPool.RotateImage = HandlePreview(myMapMaker.myObjectPool.tempBuilding.transform.position, size / 6, myMapMaker.myObjectPool.buildingRotationMat);




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
                            myBuilding.myData[j, i, k].ObjectID = 1;//set Floor0 Objects to floorTile
                        }
                        else
                        {
                            myBuilding.myData[j, i, k].ObjectID = 0;//EmptyTile
                        }
                        if (j == 0 || k == 0 || j == size - 1 || k == size - 1)
                        {
                            myBuilding.myData[j, i, k].ObjectID = 0;//EmptyTile
                        }


                        myBuilding.myData[j, i, k].myFloorNum = i;
                        myBuilding.myData[j, i, k].xArrayPos = j - ((size / 2) - 0.5f);
                        myBuilding.myData[j, i, k].zArrayPos = k - ((size / 2) - 0.5f);
                        myBuilding.myData[j, i, k].gridPos = new Vector3Int(j, i, k);

                        myBuilding.myData[j, i, k].myObject = 
                            PlaceObject(myMapMaker.myObjectPool.myTiles[myBuilding.myData[j, i, k].ObjectID] as GameObject, new Vector3(myBuilding.myData[j, i, k].xArrayPos, myBuilding.myData[j, i, k].myFloorNum, myBuilding.myData[j, i, k].zArrayPos), Quaternion.identity, floors[activeFloor]);
                        myBuilding.myData[j, i, k].myObject.transform.SetParent(floors[i].transform);

                        myBuilding.myData[j, i, k].myRotation = myBuilding.myData[j, i, k].myObject.transform.rotation;


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
                        myBuilding.myData[j, i, k].xArrayPos = j - ((size / 2) - 0.5f);
                        myBuilding.myData[j, i, k].zArrayPos = k - ((size / 2) - 0.5f);
                        myBuilding.myData[j, i, k].gridPos = new Vector3Int(j, i, k);

                    }
                }
            }
            for (int l = 0; l < BuildingData.myData.Count; l++)
            {
                Debug.Log(BuildingData.myData[l].gridPos);

                myBuilding.myData[BuildingData.myData[l].gridPos.x, BuildingData.myData[l].myFloorNum, BuildingData.myData[l].gridPos.z].ObjectID = BuildingData.myData[l].ObjectID;

                myBuilding.myData[BuildingData.myData[l].gridPos.x, BuildingData.myData[l].myFloorNum, BuildingData.myData[l].gridPos.z].myObject = 
                    PlaceObject(myMapMaker.myObjectPool.myTiles[BuildingData.myData[l].ObjectID] as GameObject, new Vector3(BuildingData.myData[l].xArrayPos , BuildingData.myData[l].myFloorNum, BuildingData.myData[l].zArrayPos), BuildingData.myData[l].myRotation, floors[BuildingData.myData[l].myFloorNum]);

                myBuilding.myData[BuildingData.myData[l].gridPos.x, BuildingData.myData[l].myFloorNum, BuildingData.myData[l].gridPos.z].myRotation = 
                    myBuilding.myData[BuildingData.myData[l].gridPos.x, BuildingData.myData[l].myFloorNum, BuildingData.myData[l].gridPos.z].myObject.transform.rotation;

            }
        }

        floors[0].SetActive(true);
        activeFloor = 1;
        floors[1].SetActive(true);

    }
    public void HanleTilePlaceing(int selectedBuildingID , Vector3 tempStartPos, Vector3 tempEndPos)
    {
        if(selectedBuildingID == 2)//(temp) Wall ID - [Dragable Tiles]
        {
            PlaceTiles(selectedBuildingID, tempStartPos, tempEndPos);
        }
        else//[Single Tiles]
        {
            PlaceSingleTile(selectedBuildingID, tempStartPos, tempEndPos);
        }
    }
    public void PlaceTiles(int selectedBuildingID, Vector3 tempStartPos, Vector3 tempEndPos)// [Dragable Tiles]
    {
        Vector3 difference = new Vector3();
        Vector3Int tilePosition = new Vector3Int();
        Quaternion Lookrotation = new Quaternion();

        Vector3Int LookDirection = Vector3Int.FloorToInt(tempStartPos - tempEndPos);
        Lookrotation = Quaternion.LookRotation(LookDirection);


        float count = Vector3.Distance(tempStartPos, tempEndPos);
        difference = tempStartPos - tempEndPos;
        

        if (count> 0)
        {
            for (int i = 0; i <= count; i++)
            {
                
                tilePosition = Vector3Int.FloorToInt(tempStartPos - (difference / count) * i);
                Vector3Int myBuildingGridPos = new Vector3Int(tilePosition.x + ((size / 2)), activeFloor, tilePosition.z + ((size / 2)));
                myBuildingGridPos.Clamp(new Vector3Int(1, 1, 1), new Vector3Int(size-2, size-2, size-2));//Clamp for empty outer ring

                SetTileData(myBuilding.myData[myBuildingGridPos.x, myBuildingGridPos.y, myBuildingGridPos.z], Lookrotation, selectedBuildingID);

            }
        }

        //Debug.Log("StartPos: " + tempStartPos + " EndPos: " + tempEndPos + "LookRotation: " + LookDirection);
        tempStartPos = Vector3Int.zero;
        tempEndPos = Vector3Int.zero;
    }
    public void PlaceSingleTile(int selectedBuildingID, Vector3 tempStartPos, Vector3 tempEndPos)//[Single Tiles]
    {
        Vector3Int tilePosition = new Vector3Int();
        Quaternion Lookrotation = new Quaternion();

        Vector3Int LookDirection = Vector3Int.FloorToInt(tempStartPos - tempEndPos);
        LookDirection = new Vector3Int(LookDirection.x, 0, LookDirection.z);

        Lookrotation = Quaternion.LookRotation(LookDirection);

        tilePosition = Vector3Int.FloorToInt(tempStartPos);
        Vector3Int myBuildingGridPos = new Vector3Int(tilePosition.x + ((size / 2)), activeFloor, tilePosition.z + ((size / 2)));
        myBuildingGridPos.Clamp(new Vector3Int(1, 1, 1), new Vector3Int(size - 2, size - 2, size - 2));//Clamp for empty outer ring

        SetTileData(myBuilding.myData[myBuildingGridPos.x, myBuildingGridPos.y, myBuildingGridPos.z], Lookrotation, selectedBuildingID);

        Debug.Log("StartPos: " + tempStartPos + " EndPos: " + tempEndPos + "LookRotation: " + Lookrotation + "Direction: " + LookDirection);
        tempStartPos = Vector3Int.zero;
        tempEndPos = Vector3Int.zero;
    }
    public void SetTileData(TileData targetObject, Quaternion lookRoation, int selectedBuildingID)
    {
        if (targetObject != null)
        {
            Vector3 tilePosition = new Vector3(targetObject.xArrayPos, targetObject.myFloorNum, targetObject.zArrayPos);
            TileData tempData = targetObject;

            tempData.ObjectID = selectedBuildingID;

            UnityEngine.GameObject.DestroyImmediate(myBuilding.myData[tempData.gridPos.x, tempData.gridPos.y, tempData.gridPos.z].myObject);//Destroy GameObject.
            tempData.myObject = PlaceObject(myMapMaker.myObjectPool.myTiles[selectedBuildingID] as GameObject, tilePosition, lookRoation, floors[activeFloor]);//Init New GameObject
            tempData.myRotation = tempData.myObject.transform.rotation;

            myBuilding.myData[tempData.gridPos.x, tempData.gridPos.y, tempData.gridPos.z] = tempData;
            
        }
        else
        {
            Debug.Log("Target = Null");
        }
    }
    
    public void SaveBuilding(string saveName, string folder)
    {
        //CleanBuilding
        foreach (GameObject floor in floors)
        {
            floor.SetActive(true);
        }
        foreach(TileData data in myBuilding.myData)
        {
            if (data.ObjectID == 0)
            {
                DestroyObject(data.myObject);
            }
            if(data.myObject != null)
            {
                UnityEngine.GameObject.DestroyImmediate(data.myObject.GetComponent<Tile>());
            }
        }

        //Add Building.TileData to DataWrapper<TileData>
        DataWrapper<TileData> mySaveData = new DataWrapper<TileData>();
        foreach(TileData TData in myBuilding.myData)
        {
            mySaveData.myData.Add(TData);
        }

        Object[] mySaves = Resources.LoadAll<TextAsset>("Saves/" + folder + "/");

        string path = "Assets/Resources/Saves/" + folder + "/" + saveName + ".txt";
        StreamWriter writer = new StreamWriter(path, false);


        string myJson = JsonUtility.ToJson(mySaveData, false);

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
        myMapMaker.myStateMachine.SwithState(1);//PlaceMode
    }



}
