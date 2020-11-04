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
    public StateBuilderMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }
    public override List<Object> myItems { get { return myMapMaker.myObjectPool.myTiles; } }
    public override TextAsset[] mySaves { get { return myMapMaker.myObjectPool.myBuildingSaves; } }

    public override void OnEnter()
    {
        myMapMaker = owner.myMapMaker;

        myEventHandler = new StateBuilderEvents();

        myMapMaker.myObjectPool.ReloadMap();
        myEventHandler.HideObjects(myMapMaker.myObjectPool.placedObjects, false);

        myEventHandler.CreateNewBuilding(myMapMaker.myObjectPool.myTileData);

        myEventHandler.FocusObject(myMapMaker.myObjectPool.tempBuilding);

        EventHandler.AddListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetStartPos);
        EventHandler.AddListner(EventType.ON_MOUSE_DRAG, myEventHandler.SetEndPos);
        EventHandler.AddListner(EventType.ON_MOUSE_UP, myEventHandler.SetEndPos);
        EventHandler.AddListner(EventType.ON_MOUSE_UP, myEventHandler.HanleTilePlaceing);

    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        myEventHandler.DestroyObject(myMapMaker.myObjectPool.tempBuilding);
        myEventHandler.DestroyObject(myMapMaker.myObjectPool.rotateImage);
        myEventHandler.HideObjects(myMapMaker.myObjectPool.placedObjects, true);

        EventHandler.RemoveListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetStartPos);
        EventHandler.RemoveListner(EventType.ON_MOUSE_DRAG, myEventHandler.SetEndPos);
        EventHandler.RemoveListner(EventType.ON_MOUSE_UP, myEventHandler.SetEndPos);
        EventHandler.RemoveListner(EventType.ON_MOUSE_UP, myEventHandler.HanleTilePlaceing);

    }
    public override void OnGUI()
    {
        GUIHandler.ShowObjectList(myItems, 0);
        GUIHandler.BuilderModeItems(1);
        GUIHandler.MainMenuItems(myMapMaker,2);
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
        myEventHandler.SaveBuilding(myString,"Buildings");
    }
    public override void OnLoad(string myString)
    {
        myEventHandler.LoadBuilding(myString);
    }

}
