using System;
using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    private enum Mode
    {
        Error = 0,
        Replay = 1,
        Interact = 2,
    }
    
    private static Mode _mode = Mode.Error;
    // 隐藏游戏状态，只对外提供一部分接口
    private static GameState _gameState = new GameState();

    public static void StateInitialize(JsonData initialData)
    {
        _gameState.Round = initialData.Round;
        _gameState.Player = initialData.Player;
        _gameState.Steps = initialData.Steps;
        _gameState.Scores = initialData.Scores;
        _gameState.Map.ClearBlocks();
        _gameState.Map.UpdateMap(initialData.StateChanges);
    }

    public static void StateUpdate(JsonData roundData)
    {
        _gameState.Round = roundData.Round;
        _gameState.Player = roundData.Player;
        _gameState.Steps = roundData.Steps;
        _gameState.Scores = roundData.Scores;
    }

    public static List<List<List<int>>> MapStateUpdateStep(StateChange stateChange)
    {
        return _gameState.Map.UpdateMapForOneStep(stateChange);
    }

    public static void MapStateUpdate(List<StateChange> stateChanges)
    {
        _gameState.Map.UpdateMap(stateChanges);
    }

    public static void DoOperation(Operation operation)
    {
        _gameState.Map.SwapBlock(operation);
    }

    public static void UpdateInformation(JsonData roundToPlay)
    {
        _gameState.Round = roundToPlay.Round;
        _gameState.Player = roundToPlay.Player;
        _gameState.Steps = roundToPlay.Steps;
        _gameState.Scores = roundToPlay.Scores;
    }
    public static Map GetMap()
    {
        return _gameState.Map;
    }

    public static bool IsReplayMode()
    {
        return _mode == Mode.Replay;
    }

    public static bool IsInteractMode()
    {
        return _mode == Mode.Interact;
    }

    public static void SetMode(int mode)
    {
        _mode = (Mode)mode;
    }
}
