using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BuildingData myData = new BuildingData();
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255,255,255,30);

        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));

    }
  


}
