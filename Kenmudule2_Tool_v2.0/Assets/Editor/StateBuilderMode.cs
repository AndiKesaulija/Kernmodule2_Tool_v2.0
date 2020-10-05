using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class StateBuilderMode : State
{
    private EventHandler myEventHandler = new EventHandler();
    private MapMaker myMapMaker;



    //UI Object List
    private List<bool> selectedTile = new List<bool>();

    private List<bool> selectedList = new List<bool>();

    public int mySelectedBuidlingID;
    public GameObject mySelectedBuilding;
    public GameObject previewObject;
    public GameObject placedObject;
    private Vector3 pointA;
    private Vector3 pointB;

    //Building
    private bool buildingToggle = false;
    private GameObject tempBuilding;
    private static int size = 4;
    private int selectedNum;
    private int activeFloor = 0;

    private Building myBuilding = new Building();
    private GameObject[] floors = new GameObject[size];

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
        previewObject = null;
        InitiateNewfloor();


    }
    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        UnityEngine.Object.DestroyImmediate(tempBuilding);
        //myEventHandler.DisableTool(selectedTile);
        myEventHandler.DestroyPreviewObject(previewObject);
    }
    private void InitiateNewfloor()
    {
        activeFloor = 0;

        if(tempBuilding != null)
        {
            UnityEngine.GameObject.DestroyImmediate(tempBuilding);
        }
        tempBuilding = new GameObject("TempBuilding");

        int num = 0;
        for (int i = 0; i < size; i++)
        {
            
            floors[i] = new GameObject("Floor" + i);
            floors[i].transform.SetParent(tempBuilding.transform);

            for (int j = 0; j < size; j++)
            {

                for (int k = 0; k < size; k++)
                {
                    

                    myBuilding.myData.Add(new BuildingData());
                    myBuilding.myData[num].myID = num;
                    if(i == 0)
                    {
                        myBuilding.myData[num].prefabID = 2;//Floor Start
                    }
                    else
                    {
                        myBuilding.myData[num].prefabID = 1;//EmptyTile
                    }
                    myBuilding.myData[num].myFloorNum = i;

                    myBuilding.myData[num].xArrayPos = j;
                    myBuilding.myData[num].zArrayPos = k;
                    myBuilding.myData[num].buildingRot = new Quaternion(0, 0, 0, 0);

                    myBuilding.myData[num].myObject = SetTile(myBuilding.myData[num]);
                    myBuilding.myData[num].myObject.transform.SetParent(floors[i].transform);

                    num = num + 1;
                }

            }
            floors[i].SetActive(false);
        }
        floors[0].SetActive(true);
        activeFloor = 1;
        floors[1].SetActive(true);

    }
    public void placeTile(GameObject target, int selectedTile)
    {
        if (target != null)
        {
            if(target.GetComponent<Tile>() != null)
            {
                BuildingData tempData = target.GetComponent<Tile>().myData;
                if(tempData.myFloorNum == activeFloor)
                {
                    UnityEngine.GameObject.DestroyImmediate(myBuilding.myData[tempData.myID].myObject);
                    tempData.prefabID = selectedTile;
                    tempData.myObject = SetTile(tempData);
                    myBuilding.myData[tempData.myID] = tempData;

                    tempData.myObject.transform.SetParent(floors[activeFloor].transform);
                }
            }
            else
            {
                Debug.Log("No Tile");
            }


        }
    }
    public GameObject SetTile(BuildingData target)
    {
        GameObject newTile = UnityEngine.Object.Instantiate(myMapMaker.myObjectPool.objectTiles[target.prefabID], new Vector3(target.xArrayPos, target.myFloorNum, target.zArrayPos), target.buildingRot);

        if(newTile.GetComponent<Tile>() != null)
        {
            newTile.GetComponent<Tile>().myData = target;
        }
        return newTile;
    }
    public void SwitchFloor(bool up)
    {
       

        if(up == true && activeFloor < 3)
        {
            activeFloor = activeFloor + 1;
        }
        if (up == false && activeFloor > 0)
        {
            activeFloor = activeFloor - 1;
        }

        for (int i = 0; i < floors.Length; i++)
        {
            if (i > activeFloor)
            {
                floors[i].SetActive(false);
            }
        }
        floors[activeFloor].SetActive(true);

    }
    public void ToggleBuilding(bool toggle)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i].SetActive(!toggle);
        }

        buildingToggle = !toggle;
        if(floors[activeFloor].activeInHierarchy == false)
        {
            for (int i = 0; i < floors.Length; i++)
            {
                if (i <= activeFloor)
                {
                    floors[i].SetActive(true);
                }
            }

        }

    }

   
    public override void OnUpdate()
    {
        myMouseEvents();
    }

   
    private void myMouseEvents()
    {
        Event e = Event.current;



        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            placeTile(myEventHandler.GetObjectRay(), mySelectedBuidlingID);

            //pointA = myEventHandler.CastRoundRay();

            //placedObject = myEventHandler.PlaceObject(previewObject);
            mouseDown = true;
        }
        //LeftMouseDrag
        if (mouseDown == true && e.type == EventType.MouseDrag)
        {
            //previewObject.SetActive(false);
            myEventHandler.RotatePreview(placedObject, myEventHandler.CastRoundRay());
        }
        //LeftMouseUp
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        {
            //pointB = myEventHandler.CastRoundRay();

            //Debug.Log(pointA);
            //Debug.Log(pointB);
            //Debug.Log(pointA - pointB);

            //float dist = Vector3.Distance(pointA, pointB);
            //Debug.Log(dist);
            //myEventHandler.PlaceObjects(mySelectedBuilding, pointA, pointB, dist);


            //previewObject.SetActive(true);
            mouseDown = false;
        }
        //RightMouseDown
        if (e.button == 1 && e.isMouse && e.type == EventType.MouseDown)
        {

        }
    }
    public override void OnGUI()
    {
        ShowObjectList(myMapMaker.myObjectPool.objectTiles);


        if (GUI.Button(new Rect(10, 500, 300, 20), "Save"))
        {
            SaveObject("TempName");
        }
        if (GUI.Button(new Rect(10, 520, 300, 20), "InitiateNewBuilding"))
        {
            InitiateNewfloor();
        }
        if (GUI.Button(new Rect(10, 540, 300, 20), "Floor Up"))
        {
            SwitchFloor(true);
        }
        if (GUI.Button(new Rect(10, 560, 300, 20), "Floor Down"))
        {
            SwitchFloor(false);
        }
        if (GUI.Button(new Rect(10, 600, 300, 20), "Togglle floors"))
        {
            ToggleBuilding(buildingToggle);
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
   
    private void SaveObject(string buildingName)
    {
        //myMapMakerWindow.generatedObjects.Add(tempBuilding);
        //PrefabUtility.CreatePrefab("Assets/Resources/Buildings", tempBuilding);

        tempBuilding.tag = "Building";

        bool CheckObjectList(string name)
        {

            foreach (GameObject building in myMapMaker.myObjectPool.generatedObjects)
            {
                if (building.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        if (CheckObjectList(buildingName) == true)//Replace with error message/are you sure
        {
            PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab");
        }
        else
        {
            myMapMaker.myObjectPool.generatedObjects.Add(PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab"));
        }

        myMapMaker.myObjectPool.ReloadAssets();
    }

}
