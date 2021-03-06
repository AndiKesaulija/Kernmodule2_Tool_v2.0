﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    private Dictionary<int, State> states = new Dictionary<int, State>();

    public MapMaker myMapMaker;

    public StateMachine(MapMaker myMapMaker)
    {
        this.myMapMaker = myMapMaker;
    }
    public void OnStart()
    {
        AddState(new StateDisableMode(this), 0);
        AddState(new StatePlaceMode(this), 1);
        AddState(new StateBuilderMode(this), 2);

    }

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }
    public void OnGUI()
    {
        currentState?.OnGUI();
    }

    public void SwithState(int type)
    {
        currentState?.OnExit();
        currentState = states[type];
        myMapMaker.myObjectPool.ReloadAssets();
        currentState?.OnEnter();
    }
    public void AddState(State state, int name)//Add new state to Dictionary
    {
        states.Add(name, state);
    }
    public void Load()
    {
        currentState?.OnPopUp(0);
    }
    public void Save()
    {
        currentState?.OnPopUp(1);
    }
    




}
