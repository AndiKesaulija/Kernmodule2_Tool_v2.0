using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Temp
public enum EventType
{
    ON_MOUSE_DOWN = 0,
    ON_MOUSE_UP = 1,
    ON_MOUSE_DRAG = 2,
}
public static class EventHandler
{
    public delegate void MyEventDelegate();


    private static Dictionary<EventType, MyEventDelegate> eventDictionary = new Dictionary<EventType, MyEventDelegate>();

    public static void AddListner(EventType type, MyEventDelegate function)
    {
        if (!eventDictionary.ContainsKey(type))
        {
            eventDictionary.Add(type, null);
        }
        eventDictionary[type] += function;

    }

    public static void RemoveListner(EventType type, MyEventDelegate function)
    {
        if (eventDictionary.ContainsKey(type) && eventDictionary[type] != null)
        {
            eventDictionary[type] -= function;
        }
    }
    public static void RaiseEvent(EventType type)
    {
        eventDictionary[type]?.Invoke();
    }
}
public static class EventHandler<T>
{
    public delegate void MyEventDelegate(T input);

    
    private static Dictionary<EventType, MyEventDelegate> eventDictionary = new Dictionary<EventType, MyEventDelegate>();

    public static void AddListner(EventType type, MyEventDelegate function)
    {
        if (!eventDictionary.ContainsKey(type))
        {
            eventDictionary.Add(type, null);
        }
        eventDictionary[type] += function;

    }

    public static void RemoveListner(EventType type, MyEventDelegate function)
    {
        if (eventDictionary.ContainsKey(type) && eventDictionary[type] != null)
        {
            eventDictionary[type] -= function;
        }
    }
    public static void RaiseEvent(EventType type, T arg)
    {
        eventDictionary[type]?.Invoke(arg);
    }
}
