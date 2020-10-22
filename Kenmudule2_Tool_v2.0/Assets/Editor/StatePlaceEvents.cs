using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class StatePlaceEvents : Events
{
    
    public StatePlaceEvents(MapMaker myMapMaker)
    {
        this.myMapMaker = myMapMaker;
    }
    private MapMaker myMapMaker;

    //GUI
    public List<bool> selectedList = new List<bool>();
    public int mySelectedBuidlingID;
    private GameObject mySelectedBuilding;
    public void PlaceBuilding()
    {


        Vector3Int LookDirection = Vector3Int.FloorToInt(tempStartPos - tempEndPos);
        Quaternion Lookrotation = Quaternion.LookRotation(LookDirection);

        //Debug.Log("SelectedBuilding: " + mySelectedBuidlingID + "StartPos: " + tempStartPos + " EndPos: " + tempEndPos + " Lookrotation: " + Lookrotation);



        GameObject newBuilding = PlaceObject(myMapMaker.myObjectPool.buildings[mySelectedBuidlingID], tempStartPos, Lookrotation, null);

        myMapMaker.myObjectPool.placedObjects.Add(newBuilding);

        tempStartPos = Vector3Int.zero;
        tempEndPos = Vector3Int.zero;
    }
    public GameObject ShowPreview(GameObject selectedBuilding)
    {
        selectedBuilding = UnityEngine.Object.Instantiate(selectedBuilding, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));


       
        if (myParentObject != null)
        {
            selectedBuilding.transform.SetParent(myParentObject.transform);
        }

        return selectedBuilding;
    }

    public void DestroyPlacedObjects(List<GameObject> myObjects)
    {
        foreach (GameObject building in myObjects)
        {
            UnityEngine.Object.DestroyImmediate(building);
        }
    }
    public void ShowObjectList(Dictionary<int, GameObject> ObjectList)
    {

        if (ObjectList != null)
        {
            for (int i = 0; i < ObjectList.Count; i++)
            {
                selectedList.Add(false);
                GUILayout.BeginHorizontal();

                selectedList[i] = GUILayout.Toggle(selectedList[i], "" + ObjectList[i].name, "Button");
                if (GUI.Button(new Rect(20, 300 + (20 * i), 20, 20), "Edit"))
                {
                    //Clear Map

                    string path = "Assets/Resources/Saves/Buildings/" + ObjectList[i].name + ".txt";

                    string myFile = File.ReadAllText(path);

                    myMapMaker.myObjectPool.myTileData = JsonUtility.FromJson<DataWrapper<TileData>>(myFile);

                    myMapMaker.myStateMachine.SwithState(2);
                    Debug.Log("Edit");
                }
                GUILayout.EndHorizontal();

                if (selectedList[i] == true)
                {

                    if (mySelectedBuilding != ObjectList[i])
                    {
                        mySelectedBuidlingID = i;
                        mySelectedBuilding = ObjectList[i];
                    }


                    for (int k = 0; k < selectedList.Count; k++)
                    {
                        if (k != i)
                        {
                            selectedList[k] = false;
                        }
                    }


                }
            }

        }


    }
    public void SaveMap(string saveName, string folder, DataWrapper<BuildingData> Data)
    {

        AssetDatabase.Refresh();
        Object[] mySaves = Resources.LoadAll<TextAsset>("Saves/" + folder + "/");

        string path = "Assets/Resources/Saves/" + folder + "/" + saveName + ".txt";
        StreamWriter writer = new StreamWriter(path, false);


        string myJson = JsonUtility.ToJson(Data, false);

        bool CheckObjectList(string name)
        {

            for (int i = 0; i < mySaves.Length; i++)
            {
                if (mySaves[i].name == name)
                {
                    return true;
                }
            }
            return false;

        }

        if (CheckObjectList(saveName) == true)//Replace with error message/are you sure
        {
            Debug.Log("OverWrite: " + saveName);
            writer.Write(myJson);
        }
        else
        {
            writer.Write(myJson);
        }

        writer.Close();
        AssetDatabase.Refresh();
    }
}
