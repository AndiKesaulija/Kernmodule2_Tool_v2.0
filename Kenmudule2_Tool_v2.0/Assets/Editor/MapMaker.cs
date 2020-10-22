using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MapMaker : EditorWindow
{
    public StateMachine myStateMachine;
    public ObjectPool myObjectPool;



    [MenuItem("Window/MapMaker")]

    static void Init()
    {
        GetWindow(typeof(MapMaker));
        
    }

    public void OnEnable()
    {
        myStateMachine = new StateMachine(this);
        myObjectPool = new ObjectPool();

        myObjectPool.myMapData.myData = new List<BuildingData>();
        myObjectPool.myTileData.myData = new List<TileData>();

        SceneView.duringSceneGui += OnSceneGUI;

        myObjectPool.ReloadAssets();

        myStateMachine.OnStart();
        myStateMachine.SwithState(1);//PlaceMode


    }
    public void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        myStateMachine.SwithState(0);//DisableMode
    }

    public void OnGUI()
    {

        myStateMachine.OnGUI();

        if (GUI.Button(new Rect(10, 440, 300, 20), "Save"))
        {
            myStateMachine.Save();//SaveWindow
        }
        if (GUI.Button(new Rect(10, 460, 300, 20), "Place Mode"))
        {
            myStateMachine.SwithState(1);//PlaceMode
        }
        if (GUI.Button(new Rect(10, 480, 300, 20), "Builder Mode"))
        {
            myObjectPool.myTileData = null;
            myStateMachine.SwithState(2);//BuilderMode
        }
        

    }

    public void OnSceneGUI(SceneView scene)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        myStateMachine.OnUpdate();
    }

   
}
