using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class StateBuilderMode : State 
{
    //private EventHandler myEventHandler = new EventHandler();
    private static MapMaker myMapMaker;
    private StateBuilderEvents myEventHandler;

    //Event
    private bool mouseDown = false;
    public StateBuilderMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;

    }

    public override void OnEnter()
    {
        myMapMaker = owner.myMapMaker;

        myEventHandler = new StateBuilderEvents(myMapMaker);

        myMapMaker.myObjectPool.ReloadMap();
        myEventHandler.HideObjects(myMapMaker.myObjectPool.placedObjects, false);

        myEventHandler.InitiateNewfloor(myMapMaker.myObjectPool.myTileData);

        myEventHandler.FocusObject(myMapMaker.myObjectPool.tempBuilding);

    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        myEventHandler.DestroyObject(myMapMaker.myObjectPool.tempBuilding);
        myEventHandler.DestroyObject(myMapMaker.myObjectPool.RotateImage);
        myEventHandler.HideObjects(myMapMaker.myObjectPool.placedObjects, true);

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
        myMapMaker.myObjectPool.ReloadTiles();
        myEventHandler.SaveBuilding(myString,"Buildings",myMapMaker.myObjectPool.myTileData);
    }

    public void myMouseEvents()
    {
        Event e = Event.current;
        

        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            //myEventHandler.SetDistance();
            myEventHandler.SetStartPos(myEventHandler.activeFloor);
            mouseDown = true;
        }
        //LeftMouseDrag
        if (mouseDown == true && e.type == EventType.MouseDrag)
        {
            myEventHandler.SetEndPos(myEventHandler.activeFloor);
        }
        //LeftMouseUp
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        {
            //myEventHandler.PlaceNewTile(myEventHandler.tempPos, myEventHandler.GetObjectRay().transform.position, mySelectedBuidlingID);

            myEventHandler.SetEndPos(myEventHandler.activeFloor);
            myEventHandler.PlaceTiles(myEventHandler.mySelectedBuidlingID);


            //myEventHandler.PlaceObject(myEventHandler.GetObjectRay(), mySelectedBuidlingID);
            mouseDown = false;
        }
        //RightMouseDown
        if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
        {
            //myEventHandler.PlaceObject(myEventHandler.GetObjectRay(),Quaternion.identity, 2);//  = EmptyTile
        }
    }
   



    public override void OnGUI()
    {
        
        myEventHandler.ShowObjectList(myMapMaker.myObjectPool.objectTiles);


        
        if (GUI.Button(new Rect(10, 520, 300, 20), "InitiateNewBuilding"))
        {
            myEventHandler.InitiateNewfloor(null);
        }
        if (GUI.Button(new Rect(10, 540, 300, 20), "Floor Up"))
        {
            myEventHandler.SwitchFloor(true);
        }
        if (GUI.Button(new Rect(10, 560, 300, 20), "Floor Down"))
        {
            myEventHandler.SwitchFloor(false);
        }
        if (GUI.Button(new Rect(10, 600, 300, 20), "Togglle floors"))
        {
            myEventHandler.ToggleBuilding();
        }


    }

   

}
