using System;
using System.Collections.Generic;
using DataManager;
using UnityEngine;

public class FunctionManager: MonoBehaviour
{
    public delegate void LoadFrameDelegate(int index);
    public delegate void LoadNextFrameDelegate();
    public delegate void HandleChangeDelegate(JsonData data);

    public class Arg
    {
        public int Index = -1;
        public JsonData Data = new JsonData();

        public Arg(){}
        
        public Arg(JsonData data)
        {
            Index = -1;
            Data = data;
        }

        public Arg(int index)
        {
            Index = index;
            Data = new JsonData();
        }
        
    }
    
    private static Queue<Tuple<Delegate, Arg>> _functionQueue;

    private void Start()
    {
        _functionQueue = new Queue<Tuple<Delegate, Arg>>();
    }

    public void AddMessage(Delegate function, Arg arg)
    {
        if (_functionQueue == null)
        {
            _functionQueue = new Queue<Tuple<Delegate, Arg>>();
        }
        _functionQueue.Enqueue(new Tuple<Delegate, Arg>(function, arg));
        Debug.Log("AddMessage");
        Debug.Log($"aaa{_functionQueue.Count}");
    }

    public void HandleMessage()
    {
        Debug.Log("HandleMessage");
        if (_functionQueue == null || _functionQueue.Count == 0)
        {
            return;
        }
        var tuple = _functionQueue.Dequeue();
        var func = tuple.Item1;
        var arg = tuple.Item2;
        if (arg.Index > 0)
        {
            func.DynamicInvoke(arg.Index);
        }
        else if (arg.Data.Round > 0)
        {
            func.DynamicInvoke(arg.Data);
        }
        else
        {
            func.DynamicInvoke();
        }
    }
}