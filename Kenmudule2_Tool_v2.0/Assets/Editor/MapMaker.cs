using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapMaker : EditorWindow

{
    public List<Object> objectTiles = new List<Object>();
    public List<GameObject> generatedObjects = new List<GameObject>();

    private List<SerializedObjectData> placedObjects;
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Window/MapMaker")]

    static void Init()
    {
        GetWindow(typeof(MapMaker));
    }

    public void OnEnable()
    {
        objectTiles.Clear();
        Object[] myTiles = Resources.LoadAll("Prefabs/Tiles");

        foreach(Object foundObject in myTiles)
        {
            objectTiles.Add(foundObject);
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
        if (GUI.Button(new Rect(10, 10, 300, 20),"Generate Object"))
        {
            GetWindow(typeof(BuildingGenerator));
        }
        if (GUI.Button(new Rect(10, 30, 300, 20), "Clear List"))
        {
            generatedObjects.Clear();
        }

        scrollPosition = GUI.BeginScrollView(new Rect(10, 50, 400, 600), scrollPosition, new Rect(0, 0, 220, 200));
        foreach (Object obj in generatedObjects)
        {
            EditorGUILayout.ObjectField(obj, typeof(Object), true);
        }

        GUI.EndScrollView();

    }

    public void OnSceneGUI(SceneView sceneView)
    {

    }

    private void PlaceBuilding(GameObject selectedObject, Vector3 myPosition, Vector3 myRotation)
    {

    }


}
