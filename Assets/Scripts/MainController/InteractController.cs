using DataManager;
using UnityEngine;

public class InteractController: MonoBehaviour 
{
    // 将后端传递过来的信息显示在游戏上
    public void Interact(JsonData data)
    {
        if (!StateController.IsInteractMode())
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
        var arg = new FunctionManager.Arg(data);
        GetComponent<FunctionManager>().AddMessage(func, arg);
    }
    
    // 处理每一回合的操作
    private void _HandleChange(JsonData data)
    {
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
    }
}