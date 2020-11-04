using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[ExecuteInEditMode]
public class ObjectPool
{
    public GameObject tempBuilding;
    public GameObject rotateImage;
    public Material rotateImageMat;

    public List<GameObject> placedObjects = new List<GameObject>();
    public List<GameObject> placedFloors = new List<GameObject>();
    public List<GameObject> placedTiles = new List<GameObject>();

    public DataWrapper<TileData> myTileData = new DataWrapper<TileData>();//wrapper with all TileData
    public DataWrapper<BuildingData> myMapData = new DataWrapper<BuildingData>();//wrapper with all PlacedObjects

    public List<Object> myBuildings = new List<Object>();
    public List<Object> myTiles = new List<Object>();

    //Saves
    public TextAsset[] myMapSaves;
    public TextAsset[] myBuildingSaves;


    public void ReloadAssets()
    {
        rotateImageMat = Resources.Load("MapMaker/Materials/PreviewRotation", typeof(Material)) as Material;
        myMapSaves = Resources.LoadAll<TextAsset>("MapMaker/Saves/Maps/");
        myBuildingSaves = Resources.LoadAll<TextAsset>("MapMaker/Saves/Buildings/");

        myTiles.Clear();
        
        myTiles.Add(Resources.Load<GameObject>("MapMaker/Prefabs/Tiles/EmptyTile"));//1
        myTiles.Add(Resources.Load<GameObject>("MapMaker/Prefabs/Tiles/FloorTile"));//2
        myTiles.Add(Resources.Load<GameObject>("MapMaker/Prefabs/Tiles/WallTile"));//3

        foreach (Object foundObject in Resources.LoadAll("MapMaker/Prefabs/Tiles"))
        {
            if (!myTiles.Contains(foundObject))
            {
                myTiles.Add(foundObject as GameObject);
            }
        }

        myBuildings.Clear();
        foreach (Object foundObject in Resources.LoadAll("MapMaker/Prefabs/Buildings"))
        {
            if (!myBuildings.Contains(foundObject))
            {
                myBuildings.Add(foundObject as GameObject);
            }
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

            for (int j = 0; j < myBuildings.Count; j++)
            {

                if(placedObjects[i].name == myBuildings[j].name)
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

                for (int j = 0; j < myTiles.Count; j++)
                {

                    if (placedTiles[i].name == myTiles[j].name)
                    {
                        myTileData.myData[i].ID = i;
                        myTileData.myData[i].ObjectID = j;
                        myTileData.myData[i].myObject = placedTiles[i].gameObject;

                        myTileData.myData[i].xArrayPos = (int)placedTiles[i].transform.position.x;
                        myTileData.myData[i].myFloorNum = (int)placedTiles[i].transform.position.y;
                        myTileData.myData[i].zArrayPos = (int)placedTiles[i].transform.position.z;
                        //myTileData.myData[i].gridPos = new Vector3Int((int)myTileData.myData[i].xArrayPos, myTileData.myData[i].myFloorNum, (int)myTileData.myData[i].zArrayPos);


                        myTileData.myData[i].myRotation = placedTiles[i].transform.rotation;
                    }

                }
                //Debug.Log(myTileData.myData[i].gridPos + " " + myTileData.myData[i].ID);

            }
        }


    }

}
