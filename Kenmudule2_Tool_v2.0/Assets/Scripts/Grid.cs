using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int depth;
    private int [,,]gridArray;

    private GameObject parentObject;

    

    public Grid(int width, int height, int depth)
    {
        parentObject = new GameObject("Grid");

        this.width = width;
        this.height = height;
        this.depth = depth;

        gridArray = new int[width, height, depth];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                for (int k = 0; k < gridArray.GetLength(2); k++)
                {
                    Tile(gridArray[i,j,k].ToString(), parentObject.transform, new Vector3(i, j, k));
                    Debug.Log("TileMade");

                }
            }
        }
    }

    public TextMesh Tile(string text, Transform parent, Vector3 localPos)
    {
        GameObject textObject = new GameObject("Tile", typeof(TextMesh));
        textObject.transform.parent = parent;
        textObject.transform.localPosition = localPos;
        textObject.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        TextMesh myTile = textObject.GetComponent<TextMesh>();
        myTile.color = Color.red;
        myTile.text = text;

        return myTile;

    }
}
