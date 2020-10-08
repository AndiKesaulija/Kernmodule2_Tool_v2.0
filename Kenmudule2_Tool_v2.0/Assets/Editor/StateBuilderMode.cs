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


    //UI Object List
    private List<bool> selectedList = new List<bool>();

    public int mySelectedBuidlingID;
    public GameObject mySelectedBuilding;
    public GameObject previewObject;
    public GameObject placedObject;
    private Vector3 pointA;
    private Vector3 pointB;



    

    //Event
    private bool mouseDown = false;
    public StateBuilderMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }

    public override void OnEnter()
    {
        myMapMaker = EditorWindow.GetWindow<MapMaker>();
        myMapMaker.myObjectPool.Reload();

        myEventHandler = new StateBuilderEvents(myMapMaker);
        myEventHandler.InitiateNewfloor();

    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        myEventHandler.DestroyBuilding();
        //myEventHandler.DisableTool(selectedTile);
    }
   

   
    public override void OnUpdate()
    {
        myMouseEvents();
    }
    public void myMouseEvents()
    {
        Event e = Event.current;
        

        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            //myEventHandler.SetDistance();
            myEventHandler.SetStartPos();
            mouseDown = true;
        }
        //LeftMouseDrag
        if (mouseDown == true && e.type == EventType.MouseDrag)
        {
        }
        //LeftMouseUp
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        {
            //myEventHandler.PlaceNewTile(myEventHandler.tempPos, myEventHandler.GetObjectRay().transform.position, mySelectedBuidlingID);
            
            myEventHandler.PlaceTiles(mySelectedBuidlingID);


            //myEventHandler.PlaceObject(myEventHandler.GetObjectRay(), mySelectedBuidlingID);
            mouseDown = false;
        }
        //RightMouseDown
        if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
        {
            myEventHandler.PlaceObject(myEventHandler.GetObjectRay(),Quaternion.identity, 1);// 1 = EmptyTile
        }
    }



    public override void OnGUI()
    {
        ShowObjectList(myMapMaker.myObjectPool.objectTiles);


        if (GUI.Button(new Rect(10, 500, 300, 20), "Save"))
        {
            myEventHandler.SaveObject("TempName");
        }
        if (GUI.Button(new Rect(10, 520, 300, 20), "InitiateNewBuilding"))
        {
            myEventHandler.InitiateNewfloor();
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
    public void ShowObjectList(List<GameObject> ObjectList)
    {

        if (ObjectList != null)
        {
            for (int i = 0; i < ObjectList.Count; i++)
            {
                selectedList.Add(false);
                selectedList[i] = GUILayout.Toggle(selectedList[i], "" + ObjectList[i].name, "Button");

                
                if (selectedList[i] == true)
                {

                    if (mySelectedBuilding != ObjectList[i])
                    {
                        mySelectedBuidlingID = i;
                        //DestroyPreview(mySelectedBuilding);
                        mySelectedBuilding = ObjectList[i];
                        //SelectMyBuilding();
                    }


                    for (int k = 0; k < selectedList.Count; k++)
                    {
                        if (k != i)
                        {
                            selectedList[k] = false;
                        }
                    }
                    

                }
            }

        }
       

    }
   
    

}
