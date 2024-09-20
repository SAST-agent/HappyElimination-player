using System;
using System.Collections.Generic;
using DataManager;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 交互状态控制器
/// </summary>
public class InteractController: MonoBehaviour
{
    private List<StateChange> _stateChanges;
    private int _nowEliminateStep;
    private string _curStopReason;
    
    public GameObject finishScreen;
    public Text winText; 
    public Text operateText;
    // 将后端传递过来的信息显示在游戏上
    public void Interact(JsonData data)
    {
        //Debug.Log($"Round: {data.Round}, Player: {data.Player}, Operation: {data.Operation}");
        if (!ModeController.IsInteractMode())
        {
            return;
        }
        // 初始化地图
        if (data.Operation.Block1.Row == -1)
        {
            StateController.StateInitialize(data);
            GetComponent<MapController>().MapInitialize(data);
        }
        else
        {
            HandleChange(data);
        }
    }

    private void HandleChange(JsonData data)
    {
        var func = new FunctionManager.HandleChangeDelegate(_HandleChange);
        GetComponent<FunctionManager>().AddMessageToQueue(func, data);
    }
    
    // 处理每一回合的操作
    private void _HandleChange(JsonData data)
    {
        Debug.Log("HandleChange");
        Debug.Log(JsonConvert.SerializeObject(data));
        StateController.BeginPlaying();
        StateController.UpdateInformation(data);
        StateController.DoOperation(data.Operation);
        GetComponent<MapController>().DoOperationOnMap(data.Operation);
        _curStopReason = data.StopReason;
        operateText.text = data.Player == StateController.getPlayer() ? "你的回合" : "对方回合";
        _stateChanges = data.StateChanges;
        _nowEliminateStep = 0;
        Invoke(nameof(UpdateMapStep), Constants.TimeBeforeFirstFrame);
    }

    private void UpdateMapStep()
    {
        if (_nowEliminateStep >= _stateChanges.Count)
        {
            CancelInvoke();
            GetComponent<UIController>().UpdateGameInfo();
            if (_curStopReason != null)
            {
                GetComponent<UIController>().GameStop(_curStopReason);
                // TODO 结束后禁用交互
            }
            StateController.EndPlaying();
            return;
        }
        StateController.MapStateUpdateStep(_stateChanges[_nowEliminateStep]);
        GetComponent<MapController>().UpdateMap(_stateChanges[_nowEliminateStep]);
        _nowEliminateStep += 1;
        Invoke(nameof(UpdateMapStep), Constants.TimeBetweenFrames);
        
    }
}