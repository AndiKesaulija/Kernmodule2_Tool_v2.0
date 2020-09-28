using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TileHandler
{
    /// <summary>
    /// All events used by MapMaker.
    /// </summary>
     

    public Quaternion tempRotation;
    //public GameObject previewObject;
    public GameObject PlaceObject(GameObject selectedBuilding, Vector3 pos,Quaternion rot, GameObject parent)
    {

        
        

        if (selectedBuilding != null)
        {
            

            if (parent == null)
            {
                selectedBuilding = UnityEngine.Object.Instantiate(selectedBuilding);
            }
            else
            {
                selectedBuilding = UnityEngine.Object.Instantiate(selectedBuilding,parent.transform);
            }

            //selectedBuilding.transform.position = pos;
            //selectedBuilding.transform.rotation = rot;

           
            if (selectedBuilding.GetComponent<Building>() == true)
            {
                selectedBuilding.GetComponent<Building>().reloadMyData();
            }


        }
        else
        {
            selectedBuilding = null;
        }

        return selectedBuilding;


    }
  
    public void HandlePreviewPos(GameObject selectedBuilding, Vector3 myPos)
    {
        if(selectedBuilding != null)
        {
            selectedBuilding.transform.position = myPos;
            selectedBuilding.transform.rotation = tempRotation;
        }
    }
    

    public void HandlePreviewRot(GameObject selectedBuilding, Vector3 myPos, Vector3 target, bool snapRot)
    {
        //Rotate to Target
        //lookAtPos.y is own y for axis rotation
        if (selectedBuilding != null)
        {
            selectedBuilding.transform.position = myPos;

            Vector3 lookAtPos = new Vector3(target.x, selectedBuilding.transform.position.y, target.z);
            Vector3 lookDirection = selectedBuilding.transform.position - lookAtPos;
            Quaternion lookRotation = Quaternion.LookRotation(-lookDirection);

            if (snapRot == true)
            {
                lookRotation = new Quaternion(Mathf.Round(lookRotation.x), Mathf.Round(lookRotation.y), Mathf.Round(lookRotation.z), Mathf.Round(lookRotation.w));
            }

            tempRotation = lookRotation;
            selectedBuilding.transform.rotation = lookRotation;
        }
    }
    public void DestroyPreview(GameObject selectedBuilding)
    {
        Debug.Log("Destroy");
        UnityEngine.Object.DestroyImmediate(selectedBuilding);
    }

    


}
