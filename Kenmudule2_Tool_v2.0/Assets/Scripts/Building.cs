using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    //public List TileData> myData = new List TileData>();
    private static int mySize;
    public TileData[,,] myData;
    public Building(int size)
    {
        Building.mySize = size;
        myData = new TileData[mySize, mySize, mySize];
    }

    
     

}
