using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapMaker : EditorWindow
{
    private EventHandler myEventHandler = new EventHandler();

    public List<Object> objectTiles = new List<Object>();
    public List<GameObject> generatedObjects = new List<GameObject>();
    private List<bool> selectedBuilding =  new List<bool>();
    
    private int building;

    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Window/MapMaker")]

    static void Init()
    {
        GetWindow(typeof(MapMaker));
    }

    public void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        objectTiles.Clear();
        Object[] myTiles = Resources.LoadAll("Prefabs/Tiles");
        generatedObjects.Clear();
        Object[] myBuildings = Resources.LoadAll("Prefabs/Buildings");

        foreach (Object foundObject in myTiles)
        {
            objectTiles.Add(foundObject);
        }
        foreach (Object foundObject in myBuildings)
        {
            generatedObjects.Add(foundObject as GameObject);
        }
    }
    public void OnDisable()
    {
        BuildingGenerator generatorWindow = GetWindow<BuildingGenerator>();
        if(generatorWindow != null)
        {
            generatorWindow.Close();
        }
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(10, 80, 300, 20),"Generate Object"))
        {
            GetWindow(typeof(BuildingGenerator));
        }
        if (GUI.Button(new Rect(10, 110, 300, 20), "Clear List"))
        {
            generatedObjects.Clear();
        }

        if (generatedObjects != null)
        {
            for (int i = 0; i < generatedObjects.Count; i++)
            {
                selectedBuilding.Add(false);
                selectedBuilding[i] = GUILayout.Toggle(selectedBuilding[i], "" + generatedObjects[i].name, "Button");

                if (selectedBuilding[i])
                {
                    if (building != i)
                    {
                        building = i;
                        for (int k = 0; k < generatedObjects.Count; k++)
                        {
                            if (k != i)
                            {
                                selectedBuilding[k] = false;
                            }
                        }
                    }
                }
            }
        }

    }

    public void OnSceneGUI(SceneView scene)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        myEventHandler.MouseEvent(Event.current, generatedObjects[0], myEventHandler.CastRay());
    }

    private void PlaceBuilding(GameObject selectedObject, Vector3 myPosition, Vector3 myRotation)
    {

    }


}
