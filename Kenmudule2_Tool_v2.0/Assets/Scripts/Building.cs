using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : MonoBehaviour
{

    public BuildingData myData = new BuildingData();
    public void reloadMyData()
    {
        myData = SetData();
    }
    public BuildingData SetData()
    {
        BuildingData tempData = new BuildingData();
        //myData.myID = Set on StateBuilderMode.SaveObject()

        tempData.buildingPos = this.transform.position;
        tempData.buildingRot = this.transform.rotation;

        return tempData;
    }
}
