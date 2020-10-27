using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIHandler
{
    
    private List<bool> selectedObjectList = new List<bool>();
    public int mySelectedObjectID;

    public delegate void MyGUIDelegate(List<Object> items);
    public MyGUIDelegate MyGUI;

    public MapMaker myMapMaker;
    public GUIHandler(MapMaker owner)
    {
        myMapMaker = owner;
    }

    public void AddItemsToGUI(MyGUIDelegate item)
    {
        MyGUI += item;
    }
    public void MainMenuItems(List<Object> items)
    {

        if (GUI.Button(new Rect(10, 420, 300, 20), "Load"))
        {
            myMapMaker.myStateMachine.Load();//LoadWindow
        }
        if (GUI.Button(new Rect(10, 440, 300, 20), "Save"))
        {
            myMapMaker.myStateMachine.Save();//SaveWindow
        }
        if (GUI.Button(new Rect(10, 460, 300, 20), "Place Mode"))
        {
            myMapMaker.myStateMachine.SwithState(1);//PlaceMode
        }
        if (GUI.Button(new Rect(10, 480, 300, 20), "Builder Mode"))
        {
            myMapMaker.myObjectPool.myTileData = null;
            myMapMaker.myStateMachine.SwithState(2);//BuilderMode
        }

    }

    public void ShowObjectList(List<Object> items)
    {

        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                selectedObjectList.Add(false);
                selectedObjectList[i] = GUILayout.Toggle(selectedObjectList[i], "" + items[i].name, "Button");

                if (selectedObjectList[i] == true)
                {
                    
                    Debug.Log(this.mySelectedObjectID);
                    for (int k = 0; k < selectedObjectList.Count; k++)
                    {
                        if (k != i)
                        {
                            selectedObjectList[k] = false;
                        }
                        else
                        {
                            this.mySelectedObjectID = i;
                        }
                    }
                }
            }
        }
    }
    //public void SwitchFloor(bool up)
    //{
    //    if (up == true && myMapMaker.myStateMachine.currentState.mye activeFloor < size - 1)
    //    {
    //        activeFloor = activeFloor + floorsize;
    //    }
    //    if (up == false && activeFloor > 0)
    //    {
    //        activeFloor = activeFloor - floorsize;
    //    }

    //    for (int i = 0; i < floors.Length; i++)
    //    {
    //        if (i > activeFloor)
    //        {
    //            floors[i].SetActive(false);
    //        }
    //    }
    //    Debug.Log(activeFloor);
    //    floors[activeFloor].SetActive(true);
    //}
    //public void ToggleBuilding()
    //{
    //    for (int i = 0; i < floors.Length; i++)
    //    {
    //        floors[i].SetActive(!buildingToggle);
    //    }

    //    buildingToggle = !buildingToggle;
    //    if (floors[activeFloor].activeInHierarchy == false)
    //    {
    //        for (int i = 0; i < floors.Length; i++)
    //        {
    //            if (i <= activeFloor)
    //            {
    //                floors[i].SetActive(true);
    //            }
    //        }

    //    }

    //}
}
