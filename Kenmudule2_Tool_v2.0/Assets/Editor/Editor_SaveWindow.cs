using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine.UI;
using System.Linq;
using System.IO;


public class Editor_SaveWindow : EditorWindow
{
    private MapMaker myMapMaker;
    private State myCurrState;

    private int width = 800;
    private int height = 300;

    string saveName;
    public Vector2 scrollPosition = Vector2.zero;

    public List<Object> mySaves = new List<Object>();
    public List<bool> selectedList = new List<bool>();

    private void OnEnable()
    {
        this.minSize = new Vector2(width, height);
        myMapMaker = GetWindow<MapMaker>();
        myCurrState = myMapMaker.myStateMachine.currentState;
    }
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 590, 390), "Save Name: ");

        saveName = EditorGUILayout.TextField("Save Name: ", saveName);

        if (GUILayout.Button("Save", GUILayout.Width(Screen.width / 3)))
        {
            myCurrState.OnSave(saveName);
            this.Close();  
        }
        
        GUILayout.EndArea();
        
    }
   
}
