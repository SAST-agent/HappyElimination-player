using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFuncController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToInteractionMode()
    {
        var modeController = GetComponent<ModeController>();
        modeController.isReplayMode = false;
        modeController.SwitchToInteractionMode();
    }

    public void SwitchToReplayMode(string path)
    {
        var modeController = GetComponent<ModeController>();
        modeController.isReplayMode = true;
        modeController.SwitchToReplayMode(path);
    }

    public void LoadNextFrame()
    {
        var replayController = gameObject.GetComponent<ReplayController>();
        replayController.PlayRound();
    }

    public void LoadFrame(int index)
    {
        var replayController = gameObject.GetComponent<ReplayController>();
        replayController.PlayRoundNumber(index);
    }

    public void SetAnimationSpeed(int speed)
    {
        GetComponent<ReplayController>().animationSpeed = speed;
    }
}
