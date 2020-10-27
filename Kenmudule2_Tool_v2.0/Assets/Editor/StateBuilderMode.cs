using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class StateBuilderMode : State 
{
    //private EventHandler myEventHandler = new EventHandler();
    private static MapMaker myMapMaker;
    private StateBuilderEvents myEventHandler;

    //Events
    public Vector3 tempPos;
    public Vector3 tempEndPos;
    public Vector3 tempStartPos;

    //Event
    private bool mouseDown = false;
    public StateBuilderMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }
    public override List<Object> myItems { get { return myMapMaker.myObjectPool.myTiles; } }
    public override TextAsset[] mySaves { get { return myMapMaker.myObjectPool.myBuildingSaves; } }
    public override void OnEnter()
    {
        myMapMaker = owner.myMapMaker;

        myEventHandler = new StateBuilderEvents(myMapMaker);

        myMapMaker.myObjectPool.ReloadMap();
        myEventHandler.HideObjects(myMapMaker.myObjectPool.placedObjects, false);

        myEventHandler.CreateNewBuilding(myMapMaker.myObjectPool.myTileData);

        myEventHandler.FocusObject(myMapMaker.myObjectPool.tempBuilding);


        //UI
        myMapMaker.myGUIHandler.AddItemsToGUI(myMapMaker.myGUIHandler.MainMenuItems);
        myMapMaker.myGUIHandler.AddItemsToGUI(myMapMaker.myGUIHandler.ShowObjectList);
    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        myEventHandler.DestroyObject(myMapMaker.myObjectPool.tempBuilding);
        myEventHandler.DestroyObject(myMapMaker.myObjectPool.RotateImage);
        myEventHandler.HideObjects(myMapMaker.myObjectPool.placedObjects, true);

        //UI
        myMapMaker.myGUIHandler.MyGUI = null;
    }



    public override void OnUpdate()
    {
        //myMouseEvents();
    }
    public override void OnPopUp(int windowType)
    {
        if (windowType == 0)
        {
            EditorWindow.GetWindow<Editor_LoadWindow>();
        }
        if (windowType == 1)
        {
            EditorWindow.GetWindow<Editor_SaveWindow>();
        }
    }
    public override void OnSave(string myString)
    {
        myEventHandler.SaveBuilding(myString,"Buildings");
    }
    public override void OnLoad(string myString)
    {
        string path = "Assets/Resources/Saves/Buildings/" + myString + ".txt";

        string myFile = File.ReadAllText(path);
        
        DataWrapper<TileData> myBuilding = JsonUtility.FromJson<DataWrapper<TileData>>(myFile);

        if (myBuilding != null)
        {
            myEventHandler.CreateNewBuilding(myBuilding);
        }
        else
        {
            Debug.Log("Invalid Save File");
        }
    }

    //public void myMouseEvents()
    //{
    //    Event e = Event.current;
        

    //    //LeftMouseDown
    //    if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
    //    {
    //        myEventHandler.SetPos(true, myEventHandler.activeFloor);

    //        mouseDown = true;
    //    }
    //    //LeftMouseDrag
    //    if (mouseDown == true && e.type == EventType.MouseDrag)
    //    {
    //        myEventHandler.SetEndPos(myEventHandler.activeFloor);
    //    }
    //    //LeftMouseUp
    //    if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
    //    {
    //        myEventHandler.SetPos(false, myEventHandler.activeFloor);
    //        myEventHandler.HanleTilePlaceing(myMapMaker.myGUIHandler.mySelectedObjectID);

    //        mouseDown = false;
    //    }
    //    //RightMouseDown
    //    if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
    //    {
    //        myEventHandler.HanleTilePlaceing(0);//PlaceEmptyTile
    //    }
    //}
   



    public override void OnGUI()
    {
        myMapMaker.myGUIHandler.MyGUI(myMapMaker.myStateMachine.currentState.myItems);
    }

   

}
