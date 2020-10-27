﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine.UI;
using System.Linq;
using System.IO;


public class Editor_LoadWindow : EditorWindow
{
    private MapMaker myMapMaker;
    private State myCurrState;

    private int width = 800;
    private int height = 600;

    string saveName;
    public Vector2 scrollPosition = Vector2.zero;

    public TextAsset[] mySaves;
    public List<bool> selectedList = new List<bool>();

    private void OnEnable()
    {
        this.minSize = new Vector2(width, height);
        myMapMaker = GetWindow<MapMaker>();
        myCurrState = myMapMaker.myStateMachine.currentState;
        mySaves = myMapMaker.myStateMachine.currentState.mySaves;

        Debug.Log(mySaves.Length);
    }
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 590, height / 2), "Save Name: ");

        for (int i = 0; i < mySaves.Length; i++)
        {
            if (GUILayout.Button(mySaves[i].name, GUILayout.Width(Screen.width / 3)))
            {
                myCurrState.OnLoad(mySaves[i].name);
                this.Close();
            }
        }
        GUILayout.EndArea();

    }
}
