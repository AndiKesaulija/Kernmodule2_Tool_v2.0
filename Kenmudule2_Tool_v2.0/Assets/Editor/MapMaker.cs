using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MapMaker : EditorWindow
{
    public StateMachine myStateMachine;
    public ObjectPool myObjectPool = new ObjectPool();

    public static bool objectSelected;
    public static bool mouseDown = false;
    public static int activefloor = 0;
    public static int mySelectedObjectID = 0;

    [MenuItem("Window/MapMaker")]
    static void Init()
    {
        GetWindow(typeof(MapMaker));
    }

    public void OnEnable()
    {
        myStateMachine = new StateMachine(this);

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
        //GUIHandler.MyGUI(myStateMachine.currentState.myItems);

        myStateMachine.currentState?.OnGUI();
        
    }

    public void OnSceneGUI(SceneView scene)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        myStateMachine.OnUpdate();

    }

   
}
