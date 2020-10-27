using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceMode : State
{
    public StatePlaceEvents myEventHandler = new StatePlaceEvents(myMapMaker);

    public static MapMaker myMapMaker;
    public StatePlaceMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
        myMapMaker = owner.myMapMaker;
    }


    public override List<Object> myItems { get { return myMapMaker.myObjectPool.myBuildings; } }
    public override TextAsset[] mySaves { get { return myMapMaker.myObjectPool.myMapSaves; } }

    

    //Event
    private bool mouseDown = false;

    public override void OnEnter()
    {
        Debug.Log("Enter PlaceMode");
        Debug.Log(myItems.Count);
        myMapMaker.myGUIHandler.AddItemsToGUI(myMapMaker.myGUIHandler.MainMenuItems);
        myMapMaker.myGUIHandler.AddItemsToGUI(myMapMaker.myGUIHandler.ShowObjectList);

        //EventHandler<int>.AddListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetStartPos);
        //EventHandler<int>.AddListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetEndPos);


        //EventHandler<int>.AddListner(EventType.ON_MOUSE_UP, myEventHandler.SetEndPos);

        //EventHandler.AddListner(EventType.ON_MOUSE_UP, myEventHandler.PlaceBuilding);

    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        myMapMaker.myGUIHandler.MyGUI = null;


        //EventHandler<int>.RemoveListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetStartPos);
        //EventHandler<int>.RemoveListner(EventType.ON_MOUSE_DOWN, myEventHandler.SetEndPos);


        //EventHandler<int>.RemoveListner(EventType.ON_MOUSE_UP, myEventHandler.SetEndPos);

        //EventHandler.RemoveListner(EventType.ON_MOUSE_UP, myEventHandler.PlaceBuilding);



    }
    public override void OnUpdate()
    {
        myMouseEvents();
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
    public override void OnGUI()
    {
        myMapMaker.myGUIHandler.MyGUI(myMapMaker.myStateMachine.currentState.myItems);

    }
    public void myMouseEvents()
    {
        Event e = Event.current;

        if(mouseDown == false && e.button == 0 && e.isMouse && e.type == UnityEngine.EventType.MouseDown)
        {
            EventHandler<int>.RaiseEvent(EventType.ON_MOUSE_DOWN, 0);

            mouseDown = true;
        }
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == UnityEngine.EventType.MouseUp)
        {
            EventHandler<int>.RaiseEvent(EventType.ON_MOUSE_UP, 0);
            EventHandler.RaiseEvent(EventType.ON_MOUSE_UP);

            mouseDown = false;
        }
        //LeftMouseDown
        //if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        //{
        //    myEventHandler.SetStartPos(0);
        //    mouseDown = true;
        //}
        ////LeftMouseDrag
        //if (mouseDown == true && e.type == EventType.MouseDrag)
        //{
        //    myEventHandler.SetEndPos(0);
        //}
        ////LeftMouseUp
        //if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        //{

        //    myEventHandler.SetEndPos(0);
        //    myEventHandler.PlaceBuilding();

        //    mouseDown = false;
        //}
        ////RightMouseDown
        //if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
        //{
        //    //myEventHandler.PlaceObject(myEventHandler.GetObjectRay(),Quaternion.identity, 2);//  = EmptyTile
        //}
    }

    public void Test(int my)
    {
        Debug.Log("Test: " + my);
    }

   

    

   
 


}
