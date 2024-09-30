using System.Collections;
using System.Collections.Generic;
using DataManager;
using UnityEngine;

/// <summary>
/// 游戏模式控制器
/// </summary>
public class ModeController : MonoBehaviour
{
    public enum Mode
    {
        Error = 0,
        Replay = 1,
        Interact = 2,
    }
    
    private static Mode _mode = Mode.Error;
    // Start is called before the first frame update
    void Start()
    {
        // 下面这一段代码是本地测试回放用的，部署的时候请注释掉
        SwitchToReplayMode("path.json");
    }
    
    // 本地解析回放文件
    public void SwitchToReplayMode(string path)
    {
        SetMode(Constants.ReplayMode);
        var replayController = GetComponent<ReplayController>();
        replayController.ParseReplay(path);
        // 下面这一段代码是本地测试回放用的，部署的时候请注释掉
        
        var initialData = replayController.GetInitialData();
        if (initialData == null)
        {
            return;
        }
        StateController.StateInitialize(initialData);
        GetComponent<MapController>().MapInitialize(initialData);
    }

    public void SwitchToInteractionMode()
    {
        SetMode(Constants.InteractMode);
    }
    
    
    public static bool IsReplayMode()
    {
        return _mode == Mode.Replay;
    }

    public static bool IsInteractMode()
    {
        return _mode == Mode.Interact;
    }

    private static void SetMode(int mode)
    {
        _mode = (Mode)mode;
    }
}
