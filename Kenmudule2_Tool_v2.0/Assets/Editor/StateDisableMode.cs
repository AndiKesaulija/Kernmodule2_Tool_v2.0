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
    public override List<Object> myItems { get { return null; } }

    public override TextAsset[] mySaves { get{return null;} }
    public override void OnEnter()
    {
        Debug.Log("Enter DisableMode");
    }
    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    public override void OnGUI()
    {
        throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
    public override void OnPopUp(int windowType)
    {
        throw new System.NotImplementedException();
    }
    public override void OnSave(string myString)
    {
        throw new System.NotImplementedException();
    }
    public override void OnLoad(string myString)
    {
        throw new System.NotImplementedException(); 
    }


}
