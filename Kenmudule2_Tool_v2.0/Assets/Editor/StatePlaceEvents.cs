using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceEvents : Events
{
    
    public StatePlaceEvents(MapMaker myMapMaker)
    {
        this.myMapMaker = myMapMaker;
    }
    private MapMaker myMapMaker;

    //GUI
    public List<bool> selectedList = new List<bool>();
    public int mySelectedBuidlingID;
    private GameObject mySelectedBuilding;
    public void PlaceBuilding(Vector3 tempStartPos, Vector3 tempEndPos)
    {
        Vector3Int LookDirection = Vector3Int.FloorToInt(tempStartPos - tempEndPos);
        Quaternion Lookrotation = Quaternion.LookRotation(LookDirection);

        GameObject newBuilding = PlaceObject(myMapMaker.myObjectPool.myBuildings[myMapMaker.myGUIHandler.mySelectedObjectID], tempStartPos, Lookrotation, null);

        myMapMaker.myObjectPool.placedObjects.Add(newBuilding);

    }

    public void DestroyPlacedObjects(List<GameObject> myObjects)
    {
        foreach (GameObject building in myObjects)
        {
            UnityEngine.Object.DestroyImmediate(building);
        }
    }
    public void SaveMap(string saveName, string folder, DataWrapper<BuildingData> Data)
    {
        myMapMaker.myObjectPool.ReloadMap();
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
            writer.Write(myJson);
        }
        else
        {
            writer.Write(myJson);
        }

        writer.Close();
        AssetDatabase.Refresh();
    }
    public void LoadMap(string saveName)
    {
        //Clear Map
        myMapMaker.myObjectPool.ReloadMap();
        DestroyPlacedObjects(myMapMaker.myObjectPool.placedObjects);

        string path = "Assets/Resources/Saves/Maps/" + saveName + ".txt";

        string myFile = File.ReadAllText(path);

        myMapMaker.myObjectPool.myMapData = JsonUtility.FromJson<DataWrapper<BuildingData>>(myFile);

        for (int i = 0; i < myMapMaker.myObjectPool.myMapData.myData.Count; i++)
        {
            myMapMaker.myObjectPool.placedObjects.Add(PlaceObject(myMapMaker.myObjectPool.buildings[myMapMaker.myObjectPool.myMapData.myData[i].ID], myMapMaker.myObjectPool.myMapData.myData[i].position, myMapMaker.myObjectPool.myMapData.myData[i].rotation, null));

        }
    }
}
