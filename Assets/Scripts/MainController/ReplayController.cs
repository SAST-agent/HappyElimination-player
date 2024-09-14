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
    bool playing;

    public float animationSpeed;
    
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

    public void PlayRoundNumber(int index)
    {
        var initialData = GetInitialData();
        // Debug.Log(initialData);
        if (index >= replay.Datas.Count)
        {
            Debug.Log("frame index out of range");
            return;
        }
        var stateController = GetComponent<StateController>();
        stateController.StateInitialize(initialData);
        // update map
        for (int i = 1; i <= index; i++)
        {
            var data = BackendData.Convert(replay.Datas[i]);
            stateController.DoOperation(data.Operation);
            stateController.MapStateUpdate(data.StateChanges);
        }
        stateController.StateUpdate(BackendData.Convert(replay.Datas[index]));

        GetComponent<MapController>().MapInitialize(initialData);
    }

    public void PlayRound()
    {
        if (nowRound >= replay.Datas.Count || playing)
        {
            Debug.Log("there is a round playing or now round index is out of range");
            return;
        }
        playing = true;
        // swap and delete and new
        var roundToPlay = BackendData.Convert(replay.Datas[nowRound++]);
        var objectController = GetComponent<GameObjectController>();
        var stateController = GetComponent<StateController>();

        stateController.DoOperation(roundToPlay.Operation);
        objectController.SwapObject(roundToPlay.Operation.Block1.Row, roundToPlay.Operation.Block1.Col, roundToPlay.Operation.Block2.Row, roundToPlay.Operation.Block2.Col, animationSpeed);

        // stateController.StateInitialize(roundToPlay);
        nowEliminateStep = 0;
        // eliminateSteps = roundToPlay.StateChanges.Count;

        Invoke(nameof(UpdateMapStep), 0.5f / animationSpeed);
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
            playing = false;
            return;
        }
        var downingBlocks = stateController.MapStateUpdateStep(roundToPlay.StateChanges[nowEliminateStep]);
        var objectController = GetComponent<GameObjectController>();
        objectController.UpdateMapObject(roundToPlay.StateChanges[nowEliminateStep++], animationSpeed);
        Invoke(nameof(UpdateMapStep), 1f / animationSpeed);
    }
}
