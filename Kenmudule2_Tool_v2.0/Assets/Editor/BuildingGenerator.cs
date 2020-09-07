using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingGenerator : EditorWindow
{
    
    public List<Object> objectGeneratorTiles;
    private GameObject tempGrid;
    private GameObject selectedGridPoint;
    private GameObject tempBuilding;

    private int columLength = 3;
    private int rowLength = 3;

    private MapMaker myMapMakerWindow;

    private List<bool> selectedTile = new List<bool>();
    private int selectedBuilding;

    public static int x = 3;
    public static int y = 1;
    public static int z = 3;

    private int[,,] buildingArray = new int[x,y,z];

    private GameObject[,,] objectArray = new GameObject[x, y, z];
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        myMapMakerWindow = GetWindow<MapMaker>();

        objectGeneratorTiles = myMapMakerWindow.objectTiles;

        for(int i=0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {
                    objectArray[i, j, k] = objectGeneratorTiles[0] as GameObject;
                }
            }
        }
        

    }
    private void OnDisable()
    {
        if (tempGrid != null)
        {
            DestroyImmediate(tempGrid);
        }
        if(tempBuilding != null)
        {
            DestroyImmediate(tempBuilding);

        }
        AssetDatabase.Refresh();
    }
    private void OnGUI()
    {

        //foreach (Object obj in objectGeneratorTiles)
        //{
        //    EditorGUILayout.ObjectField(obj, typeof(Object), true);
        //}


        GUILayout.Label("Object Window");

        EditorGUILayout.ObjectField(tempBuilding, typeof(Object), true);

        GUILayout.Label("Object Window");
        if (selectedGridPoint!= null)
        {
            GUILayout.Label("Selected Grid Object: " + selectedGridPoint.name);

        }

        if (objectGeneratorTiles != null)
        {
            for (int i = 0; i < objectGeneratorTiles.Count; i++)
            {
                selectedTile.Add(false);
                selectedTile[i] = GUILayout.Toggle(selectedTile[i], "" + objectGeneratorTiles[i].name, "Button");

                if (selectedTile[i])
                {
                    if (selectedBuilding != i)
                    {
                        selectedBuilding = i;
                        for (int k = 0; k < objectGeneratorTiles.Count; k++)
                        {
                            if (k != i)
                            {
                                selectedTile[k] = false;
                            }
                        }
                    }
                }
            }
        }


        if (GUI.Button(new Rect(10, 300, 300, 20), "Generate Tile"))
        {
            if (tempGrid != null)
            {
                DestroyImmediate(tempGrid);
            }
            if (tempBuilding != null)
            {
                DestroyImmediate(tempBuilding);

            }

            GenerateGridObject();

        }
        if (GUI.Button(new Rect(10, 320, 300, 20), "Save Building"))
        {
            SaveObject();
        }

    }
    public void OnSceneGUI(SceneView scene)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    
        MouseEvent(Event.current);

       
    }

    void MouseEvent(Event e)
    {
        //LeftMouseDown
        if (e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            selectedGridPoint = CastRay();

            if(CheckSelected() == true)
            {
                if(tempBuilding != null)
                {
                    Instantiate(objectGeneratorTiles[selectedBuilding], selectedGridPoint.transform.position, Quaternion.Euler(0, 0, 0), tempBuilding.transform);

                }
                else
                {
                    tempBuilding = new GameObject("TempBuilding");

                }
            }
        }
    }
    GameObject CastRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

        float drawPlaneHeight = 0;//y axis change if needed
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        if (Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }  //Cast ray from scenecamera to point in 3d scene(look at math)

    bool CheckSelected()
    {
        foreach(bool check in selectedTile)
        {
            if (check == true)
            {
                return true;
            }
        }
        return false;
    }
    private void GenerateGridObject()
    {
        if (tempBuilding == null)
        {
            tempBuilding = new GameObject("TempBuilding");
        }

        objectArray[0, 0, 0] = objectGeneratorTiles[1] as GameObject;
        objectArray[1, 0, 0] = objectGeneratorTiles[1] as GameObject;
        objectArray[2, 0, 0] = objectGeneratorTiles[1] as GameObject;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {

                    Instantiate(objectArray[i,j,k], new Vector3(i, j, k), Quaternion.Euler(0, 0, 0), tempBuilding.transform);
                    //Instantiate(objectGeneratorTiles[0], new Vector3(i, j, k), Quaternion.Euler(0, 0, 0), tempBuilding.transform);

                }
            }
        }



        //tempGrid = new GameObject("TempGrid");
        //tempGrid.transform.position = new Vector3(0, 0, 0);


        //for (int i = 0; i < columLength * rowLength; i++)
        //{
        //    Instantiate(objectGeneratorTiles[2], new Vector3(1 * (i % columLength), 0, 1 * (i / columLength)), Quaternion.Euler(0, 0, 0), tempGrid.transform);
        //}

        //tempBuilding = parentObject;

    }

    private void SaveObject()
    {
        myMapMakerWindow.generatedObjects.Add(tempBuilding);
        
        
        this.Close();
    }
}

