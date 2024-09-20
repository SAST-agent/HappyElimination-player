using System;
using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态控制器
/// </summary>
public class StateController : MonoBehaviour
{
    // 隐藏游戏状态，只对外提供一部分接口
    private static GameState _gameState = new GameState();
    
    private static bool _playing = false;

    private static int _curRoundPlayedNum = 0;
    
    public static void StateInitialize(JsonData initialData)
    {
        _gameState.Round = initialData.Round + 1;
        _gameState.Steps = initialData.Steps;
        _gameState.Scores = initialData.Scores;
        _gameState.Map.ClearBlocks();
        _gameState.Map.UpdateMap(initialData.StateChanges);
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
        _gameState.Round = roundToPlay.Round + 1;
        _gameState.Steps = roundToPlay.Steps;
        _gameState.Scores = roundToPlay.Scores;
    }

    public static int getRound()
    {
        return _gameState.Round;
    }

    public static int getPlayer()
    {
        return _gameState.Player;
    }

    public static void setPlayer(int player)
    {
        _gameState.Player = player;
    }

    public static List<int> getScores()
    {
        return _gameState.Scores;
    }
    
    public static Map GetMap()
    {
        return _gameState.Map;
    }

    public static void BeginPlaying()
    {
        _playing = true;
    }

    public static void EndPlaying()
    {
        _playing = false;
    }

    public static bool IsPlaying()
    {
        return _playing;
    }

    public static int getPlayedNum()
    {
        return _curRoundPlayedNum;
    }

    public static void onePlay()
    {
        _curRoundPlayedNum += 1;
    }

    public static void resetRoundPlayedNum()
    {
        _curRoundPlayedNum = 0;
    }
}
