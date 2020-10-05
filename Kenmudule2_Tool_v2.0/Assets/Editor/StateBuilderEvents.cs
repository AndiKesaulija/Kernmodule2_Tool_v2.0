using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBuilderEvents 
{

    private Vector3 tempPos;
    private bool mouseDown = false;
    public void MyMouseEvent(Vector3 pos)
    {
        Event e = Event.current;

        //LeftMouseDown
        if (mouseDown == false && e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
        {
            tempPos = pos;

            Debug.Log("Down");
            mouseDown = true;
        }
        //LeftMouseUp
        if (mouseDown == true && e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
        {

            Debug.Log(MyVectors(tempPos, pos)[0]);
            Debug.Log(MyVectors(tempPos, pos)[1]);
            Debug.Log("Up");
            mouseDown = false;

        }
    }
    public Vector3[] MyVectors(Vector3 posA, Vector3 posB)
    {
        Vector3[] myVectors = new Vector3[2];
        myVectors[0] = posA;
        myVectors[1] = posB;

        return myVectors;
    }

}
