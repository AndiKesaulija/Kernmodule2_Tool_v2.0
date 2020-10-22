using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StateDisableMode : State
{

    public StateDisableMode(StateMachine owner) : base(owner)
    {
        this.owner = owner;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter DisableMode");

    }
    public override void OnUpdate()
    {

    }
    public override void OnGUI()
    {

    }
    public override void OnExit()
    {

    }
    public override void OnPopUp()
    {

    }
    public override void OnSave(string myString)
    {

    }


}
