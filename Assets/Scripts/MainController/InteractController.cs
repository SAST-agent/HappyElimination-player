using DataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractController: MonoBehaviour
{
    public GameObject GameInfo;
    public GameObject finishSceen;
    public Text winText;
    public Text OperateText;
    // 将后端传递过来的信息显示在游戏上
    public void Interact(JsonData data)
    {
        if (!ModeController.IsInteractMode())
        {
            return;
        }
        // 初始化地图
        if (data.Round == 0)
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
        ClickController.SetClickable(false);
        OperateText.text = data.Player == PlatformFuncController.PlayerID ? "你的回合" : "对方回合";
        var operation = data.Operation;
        StateController.DoOperation(operation);
        GetComponent<GameObjectController>().SwapObject(
            operation.Block1.Row, operation.Block1.Col,
            operation.Block2.Row, operation.Block2.Col
        );
        foreach (var stateChange in data.StateChanges)
        {
            StateController.MapStateUpdateStep(stateChange);
            GetComponent<GameObjectController>().UpdateMapObject(stateChange);
        }
        GameInfo.GetComponent<GameInfoController>().SetGameInfo(data.Round, data.Scores[PlatformFuncController.PlayerID],data.Scores[1 - PlatformFuncController.PlayerID]);
        ClickController.SetClickable(true);
    }
}