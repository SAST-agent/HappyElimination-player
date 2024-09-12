using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    public int nowRound;
    public int nowEliminateStep;
    // public int eliminateSteps;
    JsonFile replay;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseReplay(string filePath)
    {
        replay = JsonParse.ReplayFileParse(filePath);
    }

    public JsonData GetInitialData()
    {
        return BackendData.Convert(replay.Datas[0]);
    }

    public void PlayRound()
    {
        if (nowRound >= replay.Datas.Count)
        {
            return;
        }
        // swap and delete and new
        var roundToPlay = BackendData.Convert(replay.Datas[nowRound++]);
        var objectController = GetComponent<GameObjectController>();
        // var stateController = GetComponent<StateController>();

        objectController.SwapObject(roundToPlay.Operation.Block1.Row, roundToPlay.Operation.Block1.Col, roundToPlay.Operation.Block2.Row, roundToPlay.Operation.Block2.Col);

        // stateController.StateInitialize(roundToPlay);
        nowEliminateStep = 0;
        // eliminateSteps = roundToPlay.StateChanges.Count;

        InvokeRepeating(nameof(UpdateMapStep), 0.8f, 2.5f);
        // TODO: finish round playing
    }
    // (swap) and (delete and new) in interaction controller

    void UpdateMapStep()
    {
        var roundToPlay = BackendData.Convert(replay.Datas[nowRound - 1]);
        var stateController = GetComponent<StateController>();
        if (nowEliminateStep >= roundToPlay.StateChanges.Count)
        {
            CancelInvoke();
            stateController.UpdateInformation(roundToPlay);
            return;
        }
        stateController.MapStateUpdateStep(nowEliminateStep, roundToPlay);
        var objectController = GetComponent<GameObjectController>();
        objectController.UpdateMapObject(roundToPlay.StateChanges[nowEliminateStep++]);
    }
}
