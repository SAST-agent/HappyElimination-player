using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataManager;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 网页通信控制器
/// </summary>
public class PlatformFuncController : MonoBehaviour
{
    public static string PlayerName;
    // 设置为交互模式
    public void SwitchToInteractionMode()
    {
        Debug.Log("Switch to Interaction Mode");
        GetComponent<ModeController>().SwitchToInteractionMode();
    }
    
    // 设置为回放模式
    public void SwitchToReplayMode(string path)
    {
        GetComponent<ModeController>().SwitchToReplayMode(path);
    }

    // 回放解析完成，开始初始化地图
    public void ReplayPlayerInited()
    {
        var initRoundData = GetComponent<ReplayController>().GetInitialData();
        GetComponent<MapController>().MapInitialize(initRoundData);
        StateController.StateInitialize(initRoundData);
    }

    private void _LoadNextFrame()
    {
        Debug.Log("LoadNextFrame");
        var replayController = gameObject.GetComponent<ReplayController>();
        replayController.PlayRound();
    }
    
    // 加载下一帧
    public void LoadNextFrame()
    {
        var func = new FunctionManager.LoadNextFrameDelegate(_LoadNextFrame);
        GetComponent<FunctionManager>().AddMessageToQueue(func, null);
    }
    
    // 加载指定帧
    public void LoadFrame(int index)
    {
        GetComponent<FunctionManager>().ClearQueue();
        GetComponent<ReplayController>().PlayRoundNumber(index);
    }
    
    // 设置动画速度
    public void SetAnimationSpeed(int speed)
    {
        GetComponent<ReplayController>().animationSpeed = speed;
    }
    
    // 设置玩家ID
    public void SetPlayerId(int id)
    {
        StateController.setPlayer(id);
    }
    
    // 设置玩家名字
    public void SetPlayerNames(string name1, string name2)
    {
        PlayerName = name1;
    }
}
