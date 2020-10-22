using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[ExecuteInEditMode]
public class ObjectPool
{

    public List<GameObject> objectTiles = new List<GameObject>();
    public Dictionary<int, GameObject> buildings = new Dictionary<int, GameObject>();


    public List<GameObject> placedObjects = new List<GameObject>();
    public DataWrapper<BuildingData> myMapData = new DataWrapper<BuildingData>();//wrapper with all PlacedObjects

    public List<GameObject> placedFloors = new List<GameObject>();
    public List<GameObject> placedTiles = new List<GameObject>();
    public DataWrapper<TileData> myTileData = new DataWrapper<TileData>();//wrapper with all TileData


    public GameObject tempBuilding;
    public GameObject RotateImage;
    public Material buildingRotationMat;

    public ObjectPool()
    {
        buildingRotationMat = Resources.Load("Materials/PreviewRotation", typeof(Material)) as Material;
        myTileData.myData = new List<TileData>();
    }
   
    public void ReloadAssets()
    {
        Debug.Log("ReloadAssets");
        AssetDatabase.Refresh();
        objectTiles.Clear();
        Object[] myTiles = Resources.LoadAll("Prefabs/Tiles");
        Debug.Log("Tiles Found: " + myTiles.Length);

        objectTiles.Add(Resources.Load<GameObject>("Prefabs/Tiles/EmptyTile"));//0
        objectTiles.Add(Resources.Load<GameObject>("Prefabs/Tiles/FloorTile"));//1
        objectTiles.Add(Resources.Load<GameObject>("Prefabs/Tiles/WallTile"));//2



        foreach (Object foundObject in myTiles)
        {
            objectTiles.Add(foundObject as GameObject);
        }

        //generatedObjects.Clear();
        buildings.Clear();

        Object[] myBuildings = Resources.LoadAll("Prefabs/Buildings");

        foreach (Object foundObject in myBuildings)
        {
            //generatedObjects.Add(foundObject as GameObject);
            buildings.Add(buildings.Count, foundObject as GameObject);
        }

    }
    
    public void ReloadMap()
    {
        placedObjects.Clear();
        myMapData.myData.Clear();

        GameObject[] myPlacedObjects = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject foundPlacedObject in myPlacedObjects)
        {
            placedObjects.Add(foundPlacedObject);
        }

        for (int i = 0; i < placedObjects.Count; i++)
        {
            myMapData.myData.Add(new BuildingData());

            for (int j = 0; j < buildings.Count; j++)
            {

                if(placedObjects[i].name == buildings[j].name)
                {
                    myMapData.myData[i].ID = j;
                    Debug.Log(myMapData.myData[i].ID);

                    myMapData.myData[i].position = placedObjects[i].transform.position;
                    myMapData.myData[i].rotation = placedObjects[i].transform.rotation;
                }
            }

           


        }


    }
    public void ReloadTiles()
    {


        placedTiles.Clear();
        placedFloors.Clear();

        myTileData = new DataWrapper<TileData>();
        myTileData.myData = new List<TileData>();
        myTileData.myData.Clear();

        if (tempBuilding != null)
        {
            Transform[] myPlacedTiles = tempBuilding.gameObject.GetComponentsInChildren<Transform>(true);//floors - Tiles - Mesh

            //GameObject[] myPlacedTiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach (Transform foundPlacedObject in myPlacedTiles)
            {
                if (foundPlacedObject.transform.parent == tempBuilding.transform)
                {
                    placedFloors.Add(foundPlacedObject.gameObject);
                }

            }
            Debug.Log("Floor Count: " + placedFloors.Count);

            foreach (Transform foundPlacedObject in myPlacedTiles)
            {
                for (int k = 0; k < placedFloors.Count; k++)
                {
                    if (foundPlacedObject.transform.parent == placedFloors[k].transform)
                    {
                        placedTiles.Add(foundPlacedObject.gameObject);

                        
                    }
                }
                
            }

            for (int i = 0; i < placedTiles.Count; i++)
            {
                myTileData.myData.Add(new TileData());

                for (int j = 0; j < objectTiles.Count; j++)
                {

                    if (placedTiles[i].name == objectTiles[j].name)
                    {
                        myTileData.myData[i].ID = i;
                        myTileData.myData[i].ObjectID = j;
                        myTileData.myData[i].myObject = placedTiles[i].gameObject;

                        myTileData.myData[i].xArrayPos = (int)placedTiles[i].transform.position.x;
                        myTileData.myData[i].myFloorNum = (int)placedTiles[i].transform.position.y;
                        myTileData.myData[i].zArrayPos = (int)placedTiles[i].transform.position.z;
                        myTileData.myData[i].gridPos = new Vector3Int(myTileData.myData[i].xArrayPos, myTileData.myData[i].myFloorNum, myTileData.myData[i].zArrayPos);


                        myTileData.myData[i].myRotation = placedTiles[i].transform.rotation;
                    }

                }
                //Debug.Log(myTileData.myData[i].gridPos + " " + myTileData.myData[i].ID);

            }
        }


    }

}
