using System;
using System.Collections.Generic;
using DataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractController: MonoBehaviour
{
    private List<StateChange> _stateChanges;
    private int _nowStep;
    
    public GameObject GameInfo;
    public GameObject finishSceen;
    public Text winText;
    public Text OperateText;
    // 将后端传递过来的信息显示在游戏上
    public void Interact(JsonData data)
    {
        Debug.Log($"Round: {data.Round}, Player: {data.Player}, Operation: {data.Operation}");
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
        StateController.BeginPlaying();
        ClickController.SetClickable(false);
        OperateText.text = data.Player == PlatformFuncController.PlayerID ? "你的回合" : "对方回合";
        var operation = data.Operation;
        StateController.UpdateInformation(data);
        StateController.DoOperation(operation);
        GetComponent<MapController>().DoOperationOnMap(data.Operation);
        _stateChanges = data.StateChanges;
        _nowStep = 0;
        Debug.Log("Begin Update Map Step 0");
        Invoke(nameof(UpdateMapStep), 0.5f);
        Debug.Log("End Update Map Step 0");
    }

    private void UpdateMapStep()
    {
        Debug.Log($"Begin Update Map Step {_nowStep}");
        if (_nowStep >= _stateChanges.Count)
        {
            CancelInvoke();
            StateController.EndPlaying();
            return;
        }
        StateController.MapStateUpdateStep(_stateChanges[_nowStep]);
        GetComponent<MapController>().UpdateMap(_stateChanges[_nowStep]);
        _nowStep += 1;
        Invoke(nameof(UpdateMapStep), 1f);
        Debug.Log($"End Update Map Step {_nowStep}");
        GameInfo.GetComponent<GameInfoController>().SetGameInfo(data.Round, data.Scores[PlatformFuncController.PlayerID],data.Scores[1 - PlatformFuncController.PlayerID]);
        ClickController.SetClickable(true);
    }
}