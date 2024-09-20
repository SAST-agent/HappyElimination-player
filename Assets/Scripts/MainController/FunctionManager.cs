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

    private void Start()
    {
        _functionQueue = new Queue<Tuple<Delegate, JsonData?>>();
    }

    private void Update()
    {
        if (!StateController.IsPlaying() && _functionQueue.Count > 0)
        {
            HandleMessageQueue();
        }
    }

    public void AddMessageToQueue(Delegate function, JsonData? arg)
    {
        if (_functionQueue == null)
        {
            _functionQueue = new Queue<Tuple<Delegate, JsonData?>>();
        }
        _functionQueue.Enqueue(new Tuple<Delegate, JsonData?>(function, arg));
        // Debug.Log($"AddMessage");
    }
    
    // 注意！！！！！！！！！！！！！！！！！！！！！！！
    // 如果实现消息队列的话
    // 不要用HandleMessage作为函数名
    // HandleMessage是前端网页发送信息过来的时候
    // 被网页调用的函数
    // 很有可能导致冲突从而让消息队列失效
    public void HandleMessageQueue()
    {
        //Debug.Log("HandleMessageQueue");
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