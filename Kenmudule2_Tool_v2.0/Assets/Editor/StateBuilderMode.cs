using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class StateBuilderMode : State
{
    private EventHandler myEventHandler = new EventHandler();
    private MapMaker myMapMaker;


    private GameObject tempBuilding;
    private GameObject tempGrid;

    //UI Object List
    private List<bool> selectedTile = new List<bool>();
    public GameObject selectedBuilding;
    private int selectedNum;
    public StateBuilderMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }

    public override void OnEnter()
    {
        myMapMaker = EditorWindow.GetWindow<MapMaker>();
        myMapMaker.myObjectPool.Reload();

        GenerateGridObject(5);
    }
    public override void OnUpdate()
    {
        myEventHandler.MouseEvent(myEventHandler.TilePos(selectedBuilding), true, tempBuilding);
        
    }
    public override void OnGUI()
    {
        if (myEventHandler.CheckActive(selectedTile) == false && myEventHandler.previewObject != null)
        {
            myEventHandler.DisableTool(selectedTile);
            selectedBuilding = null;
        }

        if (GUI.Button(new Rect(10, 500, 300, 20), "Save"))
        {
            SaveObject("TempName");
        }

        myEventHandler.ShowObjectList(myMapMaker.myObjectPool.objectTiles);
       
    }


    public override void OnExit()
    {
        Debug.Log("Exit PlaceMode");
        UnityEngine.Object.DestroyImmediate(tempBuilding);
        UnityEngine.Object.DestroyImmediate(tempGrid);
        myEventHandler.DisableTool(selectedTile);
    }

    private void GenerateGridObject(int Size)
    {
        //ResetBuilding();
        tempGrid = new GameObject("Temp Grid");
        tempBuilding = new GameObject("Temp Building");
        GameObject gridTile = Resources.Load("Prefabs/MapMaker/GridTile") as GameObject;

        for (int i = 0; i < Size; i++)
        {
            for (int k = 0; k < Size; k++)
            {
                if (gridTile != null)
                {
                    //Instantiate(gridTile, new Vector3(i, j, k),Quaternion.Euler(0,0,0));
                    myEventHandler.PlaceObject(gridTile, new Vector3(i, -0.4f, k), Quaternion.Euler(0,0,0), tempGrid);
                }
            }
        }
    }
    private void SaveObject(string buildingName)
    {
        //myMapMakerWindow.generatedObjects.Add(tempBuilding);
        //PrefabUtility.CreatePrefab("Assets/Resources/Buildings", tempBuilding);

        tempBuilding.tag = "Building";

        bool CheckObjectList(string name)
        {

            foreach (GameObject building in myMapMaker.myObjectPool.generatedObjects)
            {
                if (building.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        if (CheckObjectList(buildingName) == true)//Replace with error message/are you sure
        {
            PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab");
        }
        else
        {
            myMapMaker.myObjectPool.generatedObjects.Add(PrefabUtility.SaveAsPrefabAsset(tempBuilding, "Assets/Resources/Prefabs/Buildings/" + buildingName + ".prefab"));
        }

        myMapMaker.myObjectPool.ReloadAssets();
    }

}
