using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public bool isReplayMode;
    // Start is called before the first frame update
    void Start()
    {
        if (isReplayMode)
        {
            var replayController = GetComponent<ReplayController>();
            replayController.ParseReplay("path.json");
            GetComponent<StateController>().StateInitialize(replayController.GetInitialData());
            GetComponent<MapController>().MapInitialize(replayController.GetInitialData());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
