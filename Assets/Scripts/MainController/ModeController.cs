using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public bool isReplayMode;
    // Start is called before the first frame update
    void Start()
    {
        SwitchToReplayMode("path.json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToReplayMode(string path)
    {
        var replayController = GetComponent<ReplayController>();
        replayController.ParseReplay(path);
        var initialData = replayController.GetInitialData();
        // Debug.Log(initialData);
        GetComponent<StateController>().StateInitialize(initialData);
        GetComponent<MapController>().MapInitialize(initialData);
    }

    public void SwitchToInteractionMode()
    {
        // TODO
    }
}
