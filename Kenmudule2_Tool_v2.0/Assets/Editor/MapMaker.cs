using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MapMaker : EditorWindow
{
    public StateMachine myStateMachine = new StateMachine();
    public ObjectPool myObjectPool = new ObjectPool();

    

    [MenuItem("Window/MapMaker")]

    static void Init()
    {
        GetWindow(typeof(MapMaker));
    }

    public void OnEnable()
    {
        myObjectPool.myData.myBuildingData = new List<BuildingData>();
        SceneView.duringSceneGui += OnSceneGUI;

        myObjectPool.ReloadAssets();
        myObjectPool.Reload();

        myStateMachine.OnStart();
        myStateMachine.SwithState(1);//PlaceMode


    }
    public void OnDisable()
    {
        BuildingGenerator generatorWindow = GetWindow<BuildingGenerator>();
        if(generatorWindow != null)
        {
            generatorWindow.Close();
        }

        SceneView.duringSceneGui -= OnSceneGUI;
        myStateMachine.SwithState(0);//DisableMode
    }

    public void OnGUI()
    {

        myStateMachine.OnGUI();

        if (GUI.Button(new Rect(10, 480, 300, 20), "Builder Mode"))
        {
            myStateMachine.SwithState(2);//BuilderMode
        }
        if (GUI.Button(new Rect(10, 460, 300, 20), "Place Mode"))
        {
            myStateMachine.SwithState(1);//PlaceMode
        }

    }

    public void OnSceneGUI(SceneView scene)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        myStateMachine.OnUpdate();
    }

   
}
