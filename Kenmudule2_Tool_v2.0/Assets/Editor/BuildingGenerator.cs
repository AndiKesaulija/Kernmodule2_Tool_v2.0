using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingGenerator : EditorWindow
{
    private MapMaker myMapMakerWindow;
    private TileHandler myTileHandler = new TileHandler();
    private EventHandler myEventHandler = new EventHandler();

    private GameObject PreviewTile;
    private GameObject tempName;

    private GameObject tempGrid;
    private GameObject tempBuilding;

    private List<bool> selectedTile = new List<bool>();
    private Object mySelectedTile;

    private bool GroupEnabled = false;

    private int selectedBuilding;

    private bool mouseDown;

    public int gridSize = 5;
    public int gridZ = 5;

    private Vector2 scrollPosition = Vector2.zero;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        myMapMakerWindow = GetWindow<MapMaker>();
        GenerateGridObject(gridSize);
        this.Focus();
        


    }
    private void OnDisable()
    {
        ResetBuilding();
        SceneView.duringSceneGui -= OnSceneGUI;
        
    }
    private void OnGUI()
    {
        //GUILayout.Label("Object Window");
        //if (myMapMakerWindow.myObjectPool.objectTiles != null)
        //{
        //    for (int i = 0; i < myMapMakerWindow.myObjectPool.objectTiles.Count; i++)
        //    {
        //        selectedTile.Add(false);
        //        selectedTile[i] = GUILayout.Toggle(selectedTile[i], "" + myMapMakerWindow.myObjectPool.objectTiles[i].name, "Button");

        //        if (selectedTile[i])
        //        {
        //            if (selectedBuilding != i)
        //            {
        //                selectedBuilding = i;
        //                for (int k = 0; k < myMapMakerWindow.myObjectPool.objectTiles.Count; k++)
        //                {
        //                    if (k != i)
        //                    {
        //                        selectedTile[k] = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        ////Temp
        //gridSize = EditorGUILayout.IntField(gridSize);

        //if (GUI.Button(new Rect(10, 300, 280, 20), "Generate new grid"))
        //{
        //    GenerateGridObject(gridSize);
        //}
        //if (GUI.Button(new Rect(10, 320, 280, 20), "Save Building"))
        //{
        //    SaveObject("TempName");
        //}
        //if (myMapMakerWindow.myObjectPool.objectTiles != null)
        //{
        //    scrollPosition = GUI.BeginScrollView(new Rect(10, 350, 280, 300), scrollPosition, new Rect(0, 0, 200, 700));
        //    foreach (GameObject tile in myMapMakerWindow.myObjectPool.objectTiles)
        //    {
                
        //        EditorGUILayout.ObjectField(tile, typeof(string), true);
        //    }

        //    GUI.EndScrollView();
        //}
       

    }
    public void OnSceneGUI(SceneView scene)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    
        //myEventHandler.MouseEvent(Event.current, myMapMakerWindow.myObjectPool.objectTiles[selectedBuilding] as GameObject, myEventHandler.TilePos(myMapMakerWindow.myObjectPool.objectTiles[selectedBuilding] as GameObject), true, tempBuilding);

    }

    //void MouseEvent(Event e)
    //{
    //    bool CheckSelected()
    //    {
    //        foreach (bool check in selectedTile)
    //        {

    //            if (check == true)
    //            {
    //                if (mySelectedTile != null)
    //                {
    //                    if (mySelectedTile != myMapMakerWindow.objectTiles[selectedBuilding])
    //                    {
    //                        DestroyImmediate(PreviewTile);
    //                        mySelectedTile = myMapMakerWindow.objectTiles[selectedBuilding];
    //                        PreviewTile = Instantiate(mySelectedTile) as GameObject;
    //                    }
    //                }
    //                else
    //                {
    //                    mySelectedTile = myMapMakerWindow.objectTiles[selectedBuilding];
    //                    PreviewTile = Instantiate(mySelectedTile) as GameObject;
    //                }
    //                return true;
    //            }
    //        }
    //        if (PreviewTile != null)
    //        {
    //            DestroyImmediate(PreviewTile);
    //        }
    //        return false;
    //    }

    //    if (CheckSelected() == true)
    //    {
    //        if (PreviewTile != null && mouseDown == false)
    //        {
    //            myTileHandler.HandlePreviewPos(PreviewTile, TilePos());
    //        }

    //        //LeftMouseDown
    //        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
    //        {
    //            PreviewTile.SetActive(false);
    //            tempName = myTileHandler.PlaceObject(myMapMakerWindow.objectTiles[selectedBuilding] as GameObject, TilePos(), tempBuilding);
    //            mouseDown = true;


    //        }
    //        //LeftMouseDrag
    //        if (mouseDown == true && e.type == EventType.MouseDrag)
    //        {
    //            Debug.Log("Drag");
    //            myTileHandler.HandlePreviewRot(tempName, tempName.transform.position, CastRay(), true);
                
    //        }
    //        //LeftMouseUp
    //        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
    //        {
    //            PreviewTile.SetActive(true);
    //            mouseDown = false;
    //            //myTileHandler.PlaceObject(myMapMakerWindow.objectTiles[selectedBuilding] as GameObject, ObjectPos(), tempBuilding);

    //        }

    //    }
    //}
    //Vector3 TilePos()
    //{
    //    Event curr = Event.current;
    //    Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);  
    //    RaycastHit hit;

    //    GameObject myPrefab = myMapMakerWindow.objectTiles[selectedBuilding] as GameObject;

    //    if (Physics.Raycast(mouseRay, out hit))
    //    {
    //        if (hit.collider != null)
    //        {
    //            Vector3 myPos = hit.collider.transform.position;
    //            myPos = myPos + new Vector3(0, hit.collider.transform.localScale.y / 2, 0);
    //            myPos = myPos + new Vector3(0, myPrefab.transform.localScale.y / 2, 0);

    //            return myPos;
    //        }
    //    }
    //    return Vector3.zero;
    //}  //Cast ray from scenecamera to point in 3d scene(look at math)
    //Vector3 CastRay()
    //{
    //    Event curr = Event.current;
    //    Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
    //    float drawPlaneHeight = 0;//y axis change if needed
    //    float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
    //    Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

    //    return mousePosition;
    //}  //Cast ray from scenecamera to point in 3d scene(look at math)

   
    private void GenerateGridObject(int Size)
    {
        ResetBuilding();
        PreviewTile = new GameObject("Preview");
        tempGrid = new GameObject("Temp Grid");
        tempBuilding = new GameObject("Temp Building");
        GameObject gridTile = Resources.Load("Prefabs/MapMaker/GridTile") as GameObject;

        for (int i = 0; i < Size; i++)
        {
            for (int k = 0; k < Size; k++)
            {
                if (gridTile != null)
                {
                    //Instantiate(gridTile, new Vector3(i, j, k),Quaternion.Euler(0,0,0));
                    //myTileHandler.PlaceObject(gridTile, new Vector3(i, -0.4f, k), tempGrid, null);
                }
            }
        }
    }
    private void SaveObject(string buildingName)
    {
        ////myMapMakerWindow.generatedObjects.Add(tempBuilding);
        ////PrefabUtility.CreatePrefab("Assets/Resources/Buildings", tempBuilding);
        //bool CheckObjectList(string name)
        //{
        //    foreach (GameObject building in myMapMakerWindow.myObjectPool.generatedObjects)
        //    {
        //        if (building.name == name)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //if (CheckObjectList(buildingName)== true)
        //{
        //    PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab");
        //}
        //else
        //{
        //    myMapMakerWindow.myObjectPool.generatedObjects.Add(PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab"));
        //}
        //this.Close();
    }

    private void ResetBuilding()
    {
        for (int i = 0; i < selectedTile.Count; i++)
        {
            selectedTile[i] = false;
        }

        if (tempGrid != null)
        {
            DestroyImmediate(tempGrid);
        }
        if (tempBuilding != null)
        {
            DestroyImmediate(tempBuilding);
        }
        if (PreviewTile != null)
        {
            DestroyImmediate(PreviewTile);

        }
    }
}

