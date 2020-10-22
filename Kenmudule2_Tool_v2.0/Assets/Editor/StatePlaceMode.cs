using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceMode : State
{
    public StatePlaceMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }

    private static MapMaker myMapMaker;
    private StatePlaceEvents myEventHandler;


    //Event
    private bool mouseDown = false;

    

    public override void OnEnter()
    {
        Debug.Log("Enter PlaceMode");
        //myMapMaker = EditorWindow.GetWindow<MapMaker>();

        myMapMaker = owner.myMapMaker;

        myEventHandler = new StatePlaceEvents(myMapMaker);

    }
    public override void OnUpdate()
    {
        myMouseEvents();
    }
    public override void OnPopUp()
    {
        EditorWindow.GetWindow<Editor_SaveWindow>();
    }
    public override void OnSave(string myString)
    {
        myMapMaker.myObjectPool.ReloadMap();
        myEventHandler.SaveMap(myString,"Maps",myMapMaker.myObjectPool.myMapData);
    }
    public override void OnGUI()
    {

        myEventHandler.ShowObjectList(myMapMaker.myObjectPool.buildings);

       
        if (GUI.Button(new Rect(10, 130, 300, 20), "Destroy Placed Objects"))
        {
            myMapMaker.myObjectPool.ReloadMap();
            myEventHandler.DestroyPlacedObjects(myMapMaker.myObjectPool.placedObjects);
        }

        if (GUI.Button(new Rect(10, 160, 300, 20), "Load Map>"))
        {
            LoadMap("Test");
        }
        

    }
    public void myMouseEvents()
    {
        Event e = Event.current;


        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            myEventHandler.SetStartPos(0);
            mouseDown = true;
        }
        //LeftMouseDrag
        if (mouseDown == true && e.type == EventType.MouseDrag)
        {
            myEventHandler.SetEndPos(0);
        }
        //LeftMouseUp
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        {

            myEventHandler.SetEndPos(0);
            myEventHandler.PlaceBuilding();

            mouseDown = false;
        }
        //RightMouseDown
        if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
        {
            //myEventHandler.PlaceObject(myEventHandler.GetObjectRay(),Quaternion.identity, 2);//  = EmptyTile
        }
    }

    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        //myEventHandler.DisableTool(myEventHandler.selectedList);

    }

    

    private void LoadMap(string saveName)
    {
        //Clear Map
        myMapMaker.myObjectPool.ReloadMap();
        myEventHandler.DestroyPlacedObjects(myMapMaker.myObjectPool.placedObjects);

        string path = "Assets/Saves/" + saveName + ".txt";

        string myFile = File.ReadAllText(path);

        myMapMaker.myObjectPool.myMapData = JsonUtility.FromJson<DataWrapper<BuildingData>>(myFile);

        for (int i = 0; i < myMapMaker.myObjectPool.myMapData.myData.Count; i++)
        {
            myMapMaker.myObjectPool.placedObjects.Add(myEventHandler.PlaceObject(myMapMaker.myObjectPool.buildings[myMapMaker.myObjectPool.myMapData.myData[i].ID], myMapMaker.myObjectPool.myMapData.myData[i].position, myMapMaker.myObjectPool.myMapData.myData[i].rotation,null));

        }

    }
 


}
