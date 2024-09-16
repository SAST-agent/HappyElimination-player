using System;
using System.Collections.Generic;
using DataManager;
using UnityEngine;

#nullable enable
public class FunctionManager: MonoBehaviour
{
    public delegate void LoadNextFrameDelegate();
    public delegate void HandleChangeDelegate(JsonData data);
    
    private static Queue<Tuple<Delegate, JsonData?>> _functionQueue;
    public ReplayController replayController;

    private void Start()
    {
        _functionQueue = new Queue<Tuple<Delegate, JsonData?>>();
        replayController = GetComponent<ReplayController>();
    }

    private void Update()
    {
        if (!replayController.playing && _functionQueue.Count > 0)
        {
            HandleMessage();
        }
    }

    public void AddMessage(Delegate function, JsonData? arg)
    {
        if (_functionQueue == null)
        {
            _functionQueue = new Queue<Tuple<Delegate, JsonData?>>();
        }
        _functionQueue.Enqueue(new Tuple<Delegate, JsonData?>(function, arg));
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
        if (func == null)
        {
            return;
        }

        if (arg == null)
        {
            func.DynamicInvoke();
        }
        else
        {
            func.DynamicInvoke(arg);
        }
    }

    public void ClearQueue()
    {
        _functionQueue.Clear();
    }
}