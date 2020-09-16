using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TileHandler
{
    private Quaternion tempRotation = new Quaternion(0,0,0,0);
    protected GameObject previewObject;
    public GameObject PlaceObject(GameObject previewObject,Vector3 pos, GameObject parent)
    {

        GameObject myTile;

        if (parent == null)
        {
            myTile = PrefabUtility.InstantiatePrefab(previewObject) as GameObject;
        }
        else
        {
            myTile = PrefabUtility.InstantiatePrefab(previewObject, parent.transform) as GameObject;
        }

        myTile.transform.position = pos;
        myTile.transform.rotation = tempRotation;
        return myTile;
    }
    //Preview in Scene
    public void HandlePreviewPos(GameObject previewObject,Vector3 myPos)
    {
        previewObject.transform.position = myPos;
        previewObject.transform.rotation = tempRotation;

    }
    public void HandlePreview(GameObject previewObject,Vector3 pos)
    {
        //Set Position
        GameObject myTile = PlaceObject(previewObject, pos, null);
        myTile.transform.position = pos;

        
       
    }

    public void HandlePreviewRot(GameObject previewObject, Vector3 myPos, Vector3 target)
    {
        //Rotate to Target
        //lookAtPos.y is own y for axis rotation
        

        Vector3 lookAtPos = new Vector3 (target.x, previewObject.transform.position.y,target.z);
        Vector3 lookDirection = previewObject.transform.position - lookAtPos;
        Quaternion lookRotation = Quaternion.LookRotation(-lookDirection);

        lookRotation = new Quaternion(Mathf.Round(lookRotation.x), Mathf.Round(lookRotation.y), Mathf.Round(lookRotation.z), Mathf.Round(lookRotation.w));
        tempRotation = lookRotation;
        previewObject.transform.rotation = lookRotation;
    }
}
