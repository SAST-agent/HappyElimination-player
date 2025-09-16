using System;
using DataManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 回放模式控制器
/// </summary>
public class ReplayController : MonoBehaviour
{
    public int nowRound;
    public int nowEliminateStep;
    // public int eliminateSteps;
    JsonFile _replay;
    public float animationSpeed;

    public void ParseReplay(string filePath)
    {
        try
        {
            _replay = JsonParse.ReplayFileParse(filePath);
        }
        catch (Exception)
        {
            _replay = new JsonFile();
        }
    }

    public void AddDataToReplay(BackendData backendData)
    {
        _replay.Add(backendData);
    }

    public JsonData GetInitialData()
    {
        if (_replay?.Datas == null || _replay.Datas.Count == 0)
        {
            return null;
        }
        return BackendData.Convert(_replay.Datas[0]);
    }

    public void PlayRoundNumber(int index)
    {
        Debug.Log($"Load the round {index}");
        if (index >= _replay.Datas.Count)
        {
            Debug.Log($"frame index {index} out of range");
            return;
        }
        var initialData = GetInitialData();
        StateController.StateInitialize(initialData);
        GetComponent<UIController>().GameRestart();
        // update map
        for (int i = 1; i < index; i++)
        {
            var data = BackendData.Convert(_replay.Datas[i]);
            StateController.UpdateInformation(data);
            StateController.DoOperation(data.Operation);
            StateController.MapStateUpdate(data.StateChanges);
            if (data.StopReason != null)
            {
                GetComponent<UIController>().GameStop(data.Player, data.StopReason);
            }
        }
        GetComponent<UIController>().UpdateRound();
        GetComponent<UIController>().UpdateScore();
        GetComponent<UIController>().UpdateSkillRound();
        GetComponent<MapController>().ReGenerateMap();
        nowRound = index;
    }

    public void PlayRound()
    {
        if (nowRound >= _replay.Datas.Count)
        {
            Debug.Log("now round index is out of range");
            return;
        }
        if (StateController.IsPlaying())
        {
            Debug.Log("there is a playing round");
            return;
        }
        StateController.BeginPlaying();
        var roundToPlay = BackendData.Convert(_replay.Datas[nowRound++]);
        StateController.DoOperation(roundToPlay.Operation);
        StateController.UpdateInformation(roundToPlay);
        GetComponent<MapController>().DoOperationOnMap(roundToPlay.Operation, animationSpeed);
        nowEliminateStep = 0;
        Invoke(nameof(UpdateMapStep), 0.5f / animationSpeed);
    }

    void UpdateMapStep()
    {
        // Debug.Log($"Eliminate step {nowEliminateStep}");
        var roundToPlay = BackendData.Convert(_replay.Datas[nowRound - 1]);
        if (nowEliminateStep >= roundToPlay.StateChanges.Count)
        {
            CancelInvoke();
            GetComponent<UIController>().UpdateScore();
            GetComponent<UIController>().UpdateSkillRound();
            StateController.onePlay();
            if (StateController.getPlayedNum() == 2)
            {
                GetComponent<UIController>().UpdateRound();
                StateController.resetRoundPlayedNum();
            }
            if (roundToPlay.StopReason != null )
            {
                GetComponent<UIController>().GameStop(roundToPlay.Player, roundToPlay.StopReason);
            }
            StateController.EndPlaying();
            return;
        }
        Debug.Log($"Eliminate step {nowEliminateStep}");
        StateController.MapStateUpdateStep(roundToPlay.StateChanges[nowEliminateStep]);
        GetComponent<MapController>().UpdateMap(roundToPlay.StateChanges[nowEliminateStep], animationSpeed);
        nowEliminateStep += 1;
        Invoke(nameof(UpdateMapStep), 1f / animationSpeed);
    }
}
