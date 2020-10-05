using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ObjectPool 
{
    //public List<GameObject> objectTiles = new List<GameObject>();
    // public List<GameObject> generatedObjects = new List<GameObject>();


    public List<GameObject> objectTiles = new List<GameObject>();
    public List<GameObject> generatedObjects = new List<GameObject>();

    //public Dictionary<int, GameObject> objectTile = new Dictionary<int, GameObject>();

    public List<GameObject> placedObjects = new List<GameObject>();
    public DataWrapper myData = new DataWrapper();


    public void ReloadAssets()
    {
        objectTiles.Clear();
        Object[] myTiles = Resources.LoadAll("Prefabs/Tiles");
        foreach (Object foundObject in myTiles)
        {
            objectTiles.Add(foundObject as GameObject);
        }

        generatedObjects.Clear();
        Object[] myBuildings = Resources.LoadAll("Prefabs/Buildings");

        foreach (Object foundObject in myBuildings)
        {
            generatedObjects.Add(foundObject as GameObject);

        }
    }
    public void Reload()
    {
       

        placedObjects.Clear();
        myData.myBuildingData.Clear();

        GameObject[] myPlacedObjects = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject foundPlacedObject in myPlacedObjects)
        {
            placedObjects.Add(foundPlacedObject);
        }

        for (int i = 0; i < placedObjects.Count; i++)
        {
            //myData.myBuildingData.Add(placedObjects[i].GetComponent<Building>().myData);
        }
    }
}
