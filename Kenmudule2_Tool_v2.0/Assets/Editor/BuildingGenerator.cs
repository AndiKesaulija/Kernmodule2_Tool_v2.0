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

    public int gridX = 3;
    public int gridY = 1;
    public int gridZ = 3;

    private int activeLevel = 0;


    //private int[,,] buildingArray = new int[x,y,z];

    private GameObject[,,] objectArray;
    private GameObject[][][] tempArray;

    private Vector2 scrollPosition = Vector2.zero;

    private Material selectedMaterial;
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        myMapMakerWindow = GetWindow<MapMaker>();

        objectGeneratorTiles = myMapMakerWindow.objectTiles;

        selectedMaterial = Resources.Load<Material>("Material/SelectedObject.mat");
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


        gridX = EditorGUILayout.IntField(gridX);
        //gridY = EditorGUILayout.IntField(1);
        gridZ = EditorGUILayout.IntField(gridZ);



        if (GUI.Button(new Rect(290, 280, 20, 20), "^"))
        {
            activeLevel += 1;

            if(activeLevel > gridY - 1)
            {
                gridY = gridY + 1;
                
                GenerateGridObject(gridX, gridY, gridZ, objectArray);
                Reset();

            }

        }
        if (GUI.Button(new Rect(290, 300, 20, 20), "v"))
        {
            activeLevel -= 1;
            //GenerateGridObject(gridX, gridY, gridZ);
        }

        if (GUI.Button(new Rect(10, 300, 280, 20), "Generate Tile"))
        {
            activeLevel = 0;
            gridY = 1;
            Reset();
            GenerateGridObject(gridX, gridY, gridZ, null);
        }
        if (GUI.Button(new Rect(10, 320, 280, 20), "Save Building"))
        {
            SaveObject("TempName");
        }
        if (objectArray != null)
        {
            scrollPosition = GUI.BeginScrollView(new Rect(10, 350, 280, 300), scrollPosition, new Rect(0, 0, 200, 700));
            foreach (Object obj in objectArray)
            {
                EditorGUILayout.ObjectField(obj, typeof(Object), true);
            }

            GUI.EndScrollView();
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


            for (int i = 0; i < gridX; i++)
            {
                for (int j = 0; j < gridY; j++)
                {
                    for (int k = 0; k < gridZ; k++)
                    {
                        //Debug.Log(selectedGridPoint);
                        //Debug.Log(objectArray[i, j, k]);

                        if (selectedGridPoint != null && objectArray[i, j, k] == selectedGridPoint)
                        {
                            Debug.Log(objectArray[i,j,k] + "  -  " + objectGeneratorTiles[selectedBuilding]);
                            //objectArray[i, j, k] = new GameObject("Empty");
                            

                            if(objectArray[i, j, k].name != objectGeneratorTiles[selectedBuilding].name)
                            {
                                DestroyImmediate(selectedGridPoint.gameObject);
                                objectArray[i, j, k] = objectGeneratorTiles[selectedBuilding] as GameObject;

                                objectArray[i, j, k] = PrefabUtility.InstantiatePrefab(objectArray[i, j, k]) as GameObject;
                                objectArray[i, j, k].transform.position = new Vector3(i, j, k);
                                objectArray[i, j, k].transform.rotation = Quaternion.Euler(0, 0, 0);
                                //objectArray[i, j, k].transform.parent = buildingfloor.transform;
                            }

                            //selectedGridPoint.GetComponent<Renderer>().material = selectedMaterial;
                            ////selected Object
                            //objectArray[i, j, k] = objectGeneratorTiles[selectedBuilding] as GameObject;

                            ////Replace Object with selected Object
                            //DestroyImmediate(selectedGridPoint.gameObject);
                            ////objectArray[i, j, k] = Instantiate(objectArray[i, j, k], new Vector3(i, activeLevel, k), Quaternion.Euler(0, 0, 0), tempBuilding.transform);
                            //selectedGridPoint = null;
                        }
                        else
                        {
                            //Debug.Log("activeLevel: " + activeLevel);
                        }

                    }
                }
            }

            
        }
    }
    GameObject CastRay()
    {
        Event curr = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(curr.mousePosition);
        RaycastHit hit;

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
    private void GenerateGridObject(int sizeX, int sizeY, int sizeZ, GameObject[,,] temp)
    {

        GameObject[,,] tempArray = temp;

        //Debug.Log(tempArray[0].le);


        if (temp == null)
        {
            objectArray = new GameObject[sizeX, sizeY, sizeZ];
        }
        else
        {
            objectArray = new GameObject[sizeX, sizeY, sizeZ];
            

            for (int i = 0; i < sizeX; i++)
            {

                for (int j = 0; j < sizeY; j++)
                {

                    for (int k = 0; k < sizeZ; k++)
                    {
                       
                        
                        //objectArray[i, j, k] = tempArray[i, j, k];
                    }
                }
            }
            
        }
        

        GameObject buildingfloor = new GameObject("Floor" + activeLevel);
        buildingfloor.transform.parent = tempBuilding.transform;

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                for (int k = 0; k < sizeZ; k++)
                {
                    if (objectArray[i, j, k] == null)
                    {
                        objectArray[i, j, k] = objectGeneratorTiles[0] as GameObject;
                    }

                    objectArray[i, j, k] = PrefabUtility.InstantiatePrefab(objectArray[i, j, k]) as GameObject;
                    objectArray[i, j, k].transform.position = new Vector3(i, j, k);
                    objectArray[i, j, k].transform.rotation = Quaternion.Euler(0, 0, 0);
                    objectArray[i, j, k].transform.parent = buildingfloor.transform;

                    //GameObject tile = PrefabUtility.InstantiatePrefab(objectArray[i, j, k]) as GameObject;

                    //GameObject tile = new GameObject();
                    //tile = objectArray[i, j, k] as GameObject;

                    //tile.transform.position = new Vector3(i, j, k);
                    //tile.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //tile.transform.parent = buildingfloor.transform;

                    //if (objectArray[i, j, k] == null)
                    //{

                    //    objectArray[i, j, k] = objectGeneratorTiles[0] as GameObject;
                    //    //objectArray[i, j, k] = Instantiate(objectArray[i, j, k], new Vector3(i, j, k), Quaternion.Euler(0, 0, 0), buildingfloor.transform);
                    //    Instantiate(objectArray[i, j, k], new Vector3(i, j, k), Quaternion.Euler(0, 0, 0), buildingfloor.transform);
                    //}
                    //else
                    //{
                    //    //objectArray[i, j, k] = Instantiate(objectArray[i, j, k], new Vector3(i, j, k), Quaternion.Euler(0, 0, 0), buildingfloor.transform);
                    //    Instantiate(objectArray[i, j, k], new Vector3(i, j, k), Quaternion.Euler(0, 0, 0), buildingfloor.transform);
                    //}
                    //Debug.Log(objectArray.Length);


                }
            }
        }
    }

    

    private void SaveObject(string buildingName)
    {
        //myMapMakerWindow.generatedObjects.Add(tempBuilding);
        //PrefabUtility.CreatePrefab("Assets/Resources/Buildings", tempBuilding);

        if (CheckObjectList(buildingName)== true)
        {
            PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab");
        }
        else
        {
            myMapMakerWindow.generatedObjects.Add(PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab"));
        }
        this.Close();
    }

    private bool CheckObjectList(string name)
    {
        foreach (GameObject building in myMapMakerWindow.generatedObjects)
        {
            if (building.name == name)
            {
                return true;
            }
        }
        return false;
    }
    private void Reset()
    {
        //gridY = 1;
        if (tempBuilding == null)
        {
            tempBuilding = new GameObject("TempBuilding");
        }
        else
        {
            Object.DestroyImmediate(tempBuilding);
            tempBuilding = new GameObject("TempBuilding");
        }
    }
}

