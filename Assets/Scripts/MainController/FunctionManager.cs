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
    private static bool _isProcessing;

    private void Start()
    {
        _functionQueue = new Queue<Tuple<Delegate, Arg>>();
        _isProcessing = false;
    }
    
    private void Update()
    {
        // TODO 增加一个检测动画是否播完的标志量，在func里面修改这个标志量，这里只负责读
        if (!_isProcessing && _functionQueue.Count != 0)
        {
            Debug.Log($"bbb{_functionQueue.Count}");
            var tuple = _functionQueue.Dequeue();
            var func = tuple.Item1;
            var arg = tuple.Item2;
            LockAcquire();
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
            LockRelease();
        }
    }

    public void AddMessage(Delegate function, Arg arg)
    {
        Debug.Log("AddMessage");
        if (_functionQueue == null)
        {
            _functionQueue = new Queue<Tuple<Delegate, Arg>>();
        }
        _functionQueue.Enqueue(new Tuple<Delegate, Arg>(function, arg));
        Debug.Log($"aaa{_functionQueue.Count}");
    }

    private void LockRelease()
    {
        _isProcessing = false;
    }

    private void LockAcquire()
    {
        _isProcessing = true;
    }
}