using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceEvents : Events
{
    public void PlaceBuilding()
    {
        if (MapMaker.objectSelected == true)
        {
            Vector3Int LookDirection = Vector3Int.FloorToInt(tempStartPos - tempEndPos);
            Quaternion Lookrotation = Quaternion.LookRotation(LookDirection);

            GameObject newBuilding = PlaceObject(StatePlaceMode.myMapMaker.myObjectPool.myBuildings[MapMaker.mySelectedObjectID], tempStartPos, Lookrotation, null);

            StatePlaceMode.myMapMaker.myObjectPool.placedObjects.Add(newBuilding);

        }

    }

    public static void DestroyPlacedObjects(List<GameObject> myObjects)
    {
        StatePlaceMode.myMapMaker.myObjectPool.ReloadMap();

        foreach (GameObject building in myObjects)
        {
            UnityEngine.Object.DestroyImmediate(building);
        }
    }
    public void SaveMap(string saveName, string folder, DataWrapper<BuildingData> Data)
    {
        StatePlaceMode.myMapMaker.myObjectPool.ReloadMap();
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
        StatePlaceMode.myMapMaker.myStateMachine.SwithState(1);//PlaceMode
    }
    public void LoadMap(string saveName)
    {
        //Clear Map
        StatePlaceMode.myMapMaker.myObjectPool.ReloadMap();
        DestroyPlacedObjects(StatePlaceMode.myMapMaker.myObjectPool.placedObjects);

        string path = "Assets/Resources/Saves/Maps/" + saveName + ".txt";

        string myFile = File.ReadAllText(path);

        StatePlaceMode.myMapMaker.myObjectPool.myMapData = JsonUtility.FromJson<DataWrapper<BuildingData>>(myFile);

        for (int i = 0; i < StatePlaceMode.myMapMaker.myObjectPool.myMapData.myData.Count; i++)
        {
            StatePlaceMode.myMapMaker.myObjectPool.placedObjects.Add
                (PlaceObject(StatePlaceMode.myMapMaker.myObjectPool.myBuildings[StatePlaceMode.myMapMaker.myObjectPool.myMapData.myData[i].ID], StatePlaceMode.myMapMaker.myObjectPool.myMapData.myData[i].position, StatePlaceMode.myMapMaker.myObjectPool.myMapData.myData[i].rotation, null));

        }
    }
}
