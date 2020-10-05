using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceMode : State
{
    private EventHandler myEventHandler = new EventHandler();
    private MapMaker myMapMaker;

    //UI Object List

    public StatePlaceMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter PlaceMode");
        myMapMaker = EditorWindow.GetWindow<MapMaker>();
        myMapMaker.myObjectPool.Reload();

        

    }
    public override void OnUpdate()
    {
        //myEventHandler.MouseEvent(myEventHandler.CastRay(), false, null);
    }
    public override void OnGUI()
    {
        //if (myEventHandler.CheckActive(myEventHandler.selectedList) == false && myEventHandler.previewObject != null)
        //{
        //    myEventHandler.DisableTool(myEventHandler.selectedList);
        //}
        

        if (GUI.Button(new Rect(10, 110, 300, 20), "Clear List (Disabled)"))
        {
            //myMapMaker.myObjectPool.generatedObjects.Clear();
        }
        if (GUI.Button(new Rect(10, 130, 300, 20), "Destroy Placed Objects"))
        {
            myEventHandler.DestroyPlacedObjects(myMapMaker.myObjectPool.placedObjects);
        }

        if (GUI.Button(new Rect(10, 160, 300, 20), "Load Map>"))
        {
            LoadMap("Test");
        }
        if (GUI.Button(new Rect(10, 180, 300, 20), "Save Map>"))
        {
            SaveMap("Test");
        }

        //myEventHandler.ShowObjectList(myMapMaker.myObjectPool.generatedObjects);


    }


    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        //myEventHandler.DisableTool(myEventHandler.selectedList);

    }

    private void SaveMap(string saveName)
    {
        AssetDatabase.Refresh();

        string path = "Assets/Saves/" + saveName + ".txt";
        StreamWriter writer = new StreamWriter(path, false);

        
        myMapMaker.myObjectPool.Reload();

       

        //Debug.Log(myMapMaker.myBuildingData[0].Pos);

        string myJson = JsonUtility.ToJson(myMapMaker.myObjectPool.myData, false);

        Debug.Log(myJson);


        //writer.WriteLine(myJson);
        writer.Write(myJson);

        writer.Close();
    }

    private void LoadMap(string saveName)
    {
        //Clear Map
        myEventHandler.DestroyPlacedObjects(myMapMaker.myObjectPool.placedObjects);

        
        myMapMaker.myObjectPool.Reload();


        string path = "Assets/Saves/" + saveName + ".txt";

        string myFile = File.ReadAllText(path);

        myMapMaker.myObjectPool.myData = JsonUtility.FromJson<DataWrapper>(myFile);

        for (int i = 0; i < myMapMaker.myObjectPool.myData.myBuildingData.Count; i++)
        {
            //myMapMaker.myObjectPool.placedObjects.Add(myEventHandler.PlaceObject(myMapMaker.myObjectPool.generatedObjects[myMapMaker.myObjectPool.myData.myBuildingData[i].myID], myMapMaker.myObjectPool.myData.myBuildingData[i].buildingPos, myMapMaker.myObjectPool.myData.myBuildingData[i].buildingRot));

        }

    }



}
