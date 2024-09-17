using System;
using DataManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    public int nowRound;
    public int nowEliminateStep;
    // public int eliminateSteps;
    JsonFile replay;
    public static bool playing;

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
        try
        {
            replay = JsonParse.ReplayFileParse(filePath);
        }
        catch (Exception)
        {
            replay = new JsonFile();
        }
    }

    public void AddDataToReplay(BackendData backendData)
    {
        replay.Add(backendData);
    }

    public JsonData GetInitialData()
    {
        if (replay?.Datas == null || replay.Datas.Count == 0)
        {
            return null;
        }
        var data = replay.Datas[0];
        return BackendData.Convert(replay.Datas[0]);
    }

    public void PlayRoundNumber(int index)
    {
        Debug.Log($"Load the round {index}");
        if (index >= replay.Datas.Count)
        {
            Debug.Log($"frame index {index} out of range");
            return;
        }
        var initialData = GetInitialData();
        /*for (int i = 0; i < initialData.StateChanges[0].NewBlocks.Count; i++)
        {
            var block = initialData.StateChanges[0].NewBlocks[i];
            Debug.Log($"{block.Row}, {block.Col}: {block.Type}");
        }*/
        StateController.StateInitialize(initialData);
        // update map
        for (int i = 1; i < index; i++)
        {
            var data = BackendData.Convert(replay.Datas[i]);
            StateController.DoOperation(data.Operation);
            StateController.MapStateUpdate(data.StateChanges);
        }
        GetComponent<MapController>().ReGenerateMap();
        nowRound = index;
    }

    public void PlayRound()
    {
        if (nowRound >= replay.Datas.Count)
        {
            Debug.Log("now round index is out of range");
            return;
        }
        if (playing)
        {
            Debug.Log("there is a playing round");
            return;
        }
        playing = true;
        // swap and delete and new
        var roundToPlay = BackendData.Convert(replay.Datas[nowRound++]);

        StateController.DoOperation(roundToPlay.Operation);
        GetComponent<MapController>().DoOperationOnMap(roundToPlay.Operation, animationSpeed);

        // stateController.StateInitialize(roundToPlay);
        nowEliminateStep = 0;
        // eliminateSteps = roundToPlay.StateChanges.Count;

        Invoke(nameof(UpdateMapStep), 0.5f / animationSpeed);
        // TODO: finish round playing
    }
    // (swap) and (delete and new) in interaction controller

    void UpdateMapStep()
    {
        Debug.Log($"Eliminate step {nowEliminateStep}");
        var roundToPlay = BackendData.Convert(replay.Datas[nowRound - 1]);
        if (nowEliminateStep >= roundToPlay.StateChanges.Count)
        {
            CancelInvoke();
            StateController.UpdateInformation(roundToPlay);
            playing = false;
            return;
        }
        StateController.MapStateUpdateStep(roundToPlay.StateChanges[nowEliminateStep]);
        GetComponent<MapController>().UpdateMap(roundToPlay.StateChanges[nowEliminateStep], animationSpeed);
        nowEliminateStep += 1;
        Invoke(nameof(UpdateMapStep), 1f / animationSpeed);
    }
}
