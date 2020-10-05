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
    private GameObject tempBuilding;
    private GameObject[] floors = new GameObject[4];
    private List<int[,]> myBuilding = new List<int[,]>();
    private int activeFloor = 0;


    private int selectedNum;

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
        if(tempBuilding != null)
        {
            UnityEngine.Object.DestroyImmediate(tempBuilding);
        }

        tempBuilding = new GameObject("TempBuilding");
        int size = 4;
        activeFloor = 0;
        //Initiate
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i] = new GameObject("Floor" + i);
            myBuilding.Add(new int[size, size]);

            if (myBuilding[i] != null)
            {
                for (int j = 0; j < myBuilding[i].GetLength(0); j++)
                {
                    for (int k = 0; k < myBuilding[i].GetLength(1); k++)
                    {
                        myBuilding[i][j, k] = 2;
                        GameObject setTile = UnityEngine.Object.Instantiate(myMapMaker.myObjectPool.objectTiles[myBuilding[i][j, k]], new Vector3(j, i, k), new Quaternion(0, 0, 0, 0));

                        setTile.transform.SetParent(floors[i].transform);
                    }
                }
            }
            floors[i].transform.SetParent(tempBuilding.transform);
            floors[i].SetActive(false);
        }
        floors[activeFloor].SetActive(true);


        //for (int i = 0; i < myBuilding[floorNum].GetLength(0); i++)
        //{
        //    for (int j = 0; j < myBuilding[floorNum].GetLength(1); j++)
        //    {
        //        myBuildingLevel[i, j] = 1;
        //        //Debug.Log(myBuildingLevel[i, j]);
        //        GameObject setTile = UnityEngine.Object.Instantiate(myMapMaker.myObjectPool.objectTiles[myBuildingLevel[i, j]], new Vector3(i, 0, j), new Quaternion(0, 0, 0, 0));
        //        setTile.GetComponent<Building>().myData.xArrayPos = i;
        //        setTile.GetComponent<Building>().myData.zArrayPos = j;

        //        setTile.transform.SetParent(floor.transform);
        //    }
        //}
    }
    public void SetTile(GameObject target)
    {
       
        if(target != null && target.transform.position.y == activeFloor)
        {
            Debug.Log(target);

            if (target.tag == "GridTile" || target.tag == "Tile")
            {
                //BuildingData tempData = target.GetComponent<Building>().myData;

                //myBuildingLevel[tempData.xArrayPos, tempData.zArrayPos] = mySelectedBuidlingID;

                //Destroy target GameObject
                myBuilding[activeFloor][(int)target.transform.position.x, (int)target.transform.position.z] = mySelectedBuidlingID;

                //Instantiate new GameObject 
                GameObject setTile = UnityEngine.Object.Instantiate(myMapMaker.myObjectPool.objectTiles[myBuilding[activeFloor][(int)target.transform.position.x, (int)target.transform.position.z]], target.transform.position, new Quaternion(0, 0, 0, 0));
                UnityEngine.GameObject.DestroyImmediate(target);

                //GameObject setTile = UnityEngine.Object.Instantiate(myMapMaker.myObjectPool.objectTiles[myBuildingLevel[tempData.xArrayPos, tempData.zArrayPos]], new Vector3(tempData.xArrayPos, 0, tempData.zArrayPos), new Quaternion(0, 0, 0, 0));

                //setTile.GetComponent<Building>().myData = tempData;
                setTile.transform.SetParent(floors[activeFloor].transform);
                //Set int at arraypos to new object ID
                //initiate new Object of Array[i,j] ID
            }
        }
    }
    public void SwitchFloor(bool up)
    {
        activeFloor = Mathf.Clamp(activeFloor, 0, 4);

        for (int i = 0; i < floors.Length; i++)
        {
            if(i > activeFloor)
            {
                floors[i].SetActive(false);
            }
        }
        if(up == true && activeFloor < 3)
        {
            activeFloor = activeFloor + 1;
        }
        if (up == false && activeFloor > 0)
        {
            activeFloor = activeFloor - 1;
        }
        floors[activeFloor].SetActive(true);

    }

    public override void OnUpdate()
    {
        //myEventHandler.MouseEvent(myEventHandler.TilePos(selectedBuilding), true, tempBuilding);
        myMouseEvents();
        //myEventHandler.HandlePreview(previewObject);
    }

    private void SelectMyBuilding()
    {
        if(previewObject != null)
        {
            myEventHandler.DestroyPreviewObject(previewObject);
        }
        previewObject = myEventHandler.ShowPreview(mySelectedBuilding);
    }
    private void myMouseEvents()
    {
        Event e = Event.current;



        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            SetTile(myEventHandler.GetObjectRay());

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
    
    public bool CheckActive(List<bool> boolList)
    {
        foreach (bool mybool in boolList)
        {
            if (mybool == true)
            {
                return true;

            }
        }

        return false;
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
