using UnityEngine;
using System;

[Serializable]
public class BuildingData 
{
    public GameObject myObject;
    public int myID;
    public int prefabID;

    public int xArrayPos;
    public int myFloorNum;
    public int zArrayPos;
    public Vector3Int gridPos;

    public Vector3 myPosition;
    public Quaternion buildingRot;
    

}
