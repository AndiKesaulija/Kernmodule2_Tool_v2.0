using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : MonoBehaviour
{
    //public List<BuildingData> myData = new List<BuildingData>();

    public Building(int size)
    {
        Building.mySize = size;
    }
    private static int mySize =4;

    public BuildingData[,,] myData = new BuildingData[mySize, mySize, mySize];

    public void reloadMyData()
    {
        //myData = SetData();
    }
    public BuildingData SetData()
    {
        BuildingData tempData = new BuildingData();
        //myData.myID = Set on StateBuilderMode.SaveObject()

        //tempData.buildingPos = this.transform.position;
        tempData.buildingRot = this.transform.rotation;

        return tempData;
    }
}
