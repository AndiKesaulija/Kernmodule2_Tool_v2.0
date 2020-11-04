using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceMode : State
{

    public static MapMaker myMapMaker;
    public StatePlaceMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
        myMapMaker = owner.myMapMaker;
    }
    public StatePlaceEvents myEventHandler = new StatePlaceEvents();
    public override List<Object> myItems { get { return myMapMaker.myObjectPool.myBuildings; } }
    public override TextAsset[] mySaves { get { return myMapMaker.myObjectPool.myMapSaves; } }

    public override void OnEnter()
    {
        Debug.Log("Enter PlaceMode");

        EventHandler.AddListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetStartPos);
        EventHandler.AddListner(EventType.ON_MOUSE_DRAG, myEventHandler.SetEndPos);
        EventHandler.AddListner(EventType.ON_MOUSE_UP, myEventHandler.SetEndPos);
        EventHandler.AddListner(EventType.ON_MOUSE_UP, myEventHandler.PlaceBuilding);


        MapMaker.activefloor = 0;

    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");

        EventHandler.RemoveListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetStartPos);
        EventHandler.RemoveListner(EventType.ON_MOUSE_DRAG, myEventHandler.SetEndPos);
        EventHandler.RemoveListner(EventType.ON_MOUSE_UP, myEventHandler.SetEndPos);
        EventHandler.RemoveListner(EventType.ON_MOUSE_UP, myEventHandler.PlaceBuilding);
    }
    public override void OnGUI()
    {

        GUIHandler.ShowObjectList(myItems, 0);
        GUIHandler.PlaceModeItems(1);
        GUIHandler.MainMenuItems(myMapMaker, 2);

    }
    public override void OnUpdate()
    {
        EventHandler.myMouseEvents();
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
        myEventHandler.SaveMap(myString,"Maps",myMapMaker.myObjectPool.myMapData);
    }
    public override void OnLoad(string saveName)
    {
        myEventHandler.LoadMap(saveName);
    }
   

}
