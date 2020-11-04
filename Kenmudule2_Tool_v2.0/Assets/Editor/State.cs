using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public State(StateMachine owner)
    {
        this.owner = owner;
    }
    public StateMachine owner;

    
    public abstract List<Object> myItems { get; }
    public abstract TextAsset[] mySaves { get;} 
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnGUI();

    public abstract void OnPopUp(int windowType);
    public abstract void OnSave(string myString);
    public abstract void OnLoad(string myString);


}
