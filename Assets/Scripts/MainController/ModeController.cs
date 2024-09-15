using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 本地测试用
public class ModeController : MonoBehaviour
{
    public bool isReplayMode;

    public bool isInteractMode;
    // Start is called before the first frame update
    void Start()
    {
        SwitchToReplayMode("path.json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // 本地解析回放文件
    public void SwitchToReplayMode(string path)
    {
        isReplayMode = true;
        isInteractMode = false;
        var replayController = GetComponent<ReplayController>();
        replayController.ParseReplay(path);
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
        isInteractMode = true;
        isReplayMode = false;
        // TODO
    }
}
