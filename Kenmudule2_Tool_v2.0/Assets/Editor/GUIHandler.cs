using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GUIHandler
{
    
    private static List<bool> selectedObjectList = new List<bool>();
    //public static int mySelectedObjectID;

    public delegate void MyGUIDelegate<T>(List<T> items);
    public static MyGUIDelegate<UnityEngine.Object> MyGUI;

    private static float boxHeight = 300 + 20;
    private static Vector2 scrollx;

    public static void AddItemsToGUI(MyGUIDelegate<UnityEngine.Object> item)
    {
        MyGUI += item;
    }
    public static void ShowObjectList(List<UnityEngine.Object> items, float height)
    {
        GUILayout.BeginArea(new Rect(0, (boxHeight * height), 300, 300));
        scrollx = EditorGUILayout.BeginScrollView(scrollx);
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                selectedObjectList.Add(false);
                selectedObjectList[i] = GUILayout.Toggle(selectedObjectList[i], "" + items[i].name, "Button");

                if (selectedObjectList[i] == true)
                {
                    for (int k = 0; k < selectedObjectList.Count; k++)
                    {
                        if (k != i)
                        {
                            selectedObjectList[k] = false;

                        }
                        else
                        {
                            MapMaker.mySelectedObjectID = i;
                            MapMaker.objectSelected = true;
                        }
                    }
                }
                
            }
        }
        bool CheckSelectedObjectList(){
            foreach(bool obj in selectedObjectList)
            {
                if(obj == true)
                {
                    return true;
                }
            }
            return false;
        }
        if (CheckSelectedObjectList() == false)
        {
            MapMaker.objectSelected = false;
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    public static void MainMenuItems(MapMaker myMapMaker,float height)
    {
        GUILayout.BeginArea(new Rect(0, (boxHeight * height), 300, 300));

        if (GUILayout.Button("Load"))
        {
            myMapMaker.myStateMachine.Load();//LoadWindow
        }
        if (GUILayout.Button("Save"))
        {
            myMapMaker.myStateMachine.Save();//SaveWindow
        }
        if (GUILayout.Button("Place Mode"))
        {
            myMapMaker.myStateMachine.SwithState(1);//PlaceMode
        }
        if (GUILayout.Button("Builder Mode"))
        {
            myMapMaker.myObjectPool.myTileData = null;
            myMapMaker.myStateMachine.SwithState(2);//BuilderMode
        }
        GUILayout.EndArea();

    }

    public static void BuilderModeItems(float height)
    {
        GUILayout.BeginArea(new Rect(0, (boxHeight * height), 300, 300));

        if (GUILayout.Button("Floor Up"))
        {
            if(MapMaker.activefloor < StateBuilderEvents.size - 1)
            {
                MapMaker.activefloor = MapMaker.activefloor + StateBuilderEvents.floorsize;

                for (int i = 0; i < StateBuilderEvents.floors.Length; i++)
                {
                    if (i > MapMaker.activefloor)
                    {
                        StateBuilderEvents.floors[i].SetActive(false);
                    }
                }
                StateBuilderEvents.floors[MapMaker.activefloor].SetActive(true);
            }
        }
        if (GUILayout.Button("Floor Down"))
        {
            if (MapMaker.activefloor > 0)
            {
                MapMaker.activefloor = MapMaker.activefloor - StateBuilderEvents.floorsize;

                for (int i = 0; i < StateBuilderEvents.floors.Length; i++)
                {
                    if (i > MapMaker.activefloor)
                    {
                        StateBuilderEvents.floors[i].SetActive(false);
                    }
                }
                StateBuilderEvents.floors[MapMaker.activefloor].SetActive(true);
            }
        }
        if (GUILayout.Button("Toggle Building"))
        {
            for (int i = 0; i < StateBuilderEvents.floors.Length; i++)
            {
                StateBuilderEvents.floors[i].SetActive(!StateBuilderEvents.buildingToggle);
            }

            StateBuilderEvents.buildingToggle = !StateBuilderEvents.buildingToggle;
            if (StateBuilderEvents.floors[MapMaker.activefloor].activeInHierarchy == false)
            {
                for (int i = 0; i < StateBuilderEvents.floors.Length; i++)
                {
                    if (i <= MapMaker.activefloor)
                    {
                        StateBuilderEvents.floors[i].SetActive(true);
                    }
                }

            }
        }
        GUILayout.EndArea();

    }
    public static void PlaceModeItems(float height)
    {
        GUILayout.BeginArea(new Rect(0, (boxHeight * height), 300, 300));

        if (GUILayout.Button("Clear Map"))
        {
            StatePlaceEvents.DestroyPlacedObjects(StatePlaceMode.myMapMaker.myObjectPool.placedObjects);
        }
        GUILayout.EndArea();
    }
}
