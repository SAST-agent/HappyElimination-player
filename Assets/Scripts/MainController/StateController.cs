using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    GameState gameState = new GameState();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StateInitialize(JsonData initialData)
    {
        gameState.Round = initialData.Round;
        gameState.Player = initialData.Player;
        gameState.Steps = initialData.Steps;
        gameState.Scores = initialData.Scores;
        gameState.Map.UpdateMap(initialData.StateChanges);
    }

    public List<List<List<int>>> MapStateUpdateStep(StateChange stateChange)
    {
        return gameState.Map.UpdateMapForOneStep(stateChange);
    }

    public void DoOperation(Operation operation)
    {
        gameState.Map.SwapBlock(operation);
    }

    public void UpdateInformation(JsonData roundToPlay)
    {
        gameState.Round = roundToPlay.Round;
        gameState.Player = roundToPlay.Player;
        gameState.Steps = roundToPlay.Steps;
        gameState.Scores = roundToPlay.Scores;
    }
    public Map GetMap()
    {
        return gameState.Map;
    }

    public void changeMap() // 测试用的
    {
        Debug.Log(gameState.Map.Blocks[0][0].Type);
        Debug.Log(gameState.Map.Blocks[1][0].Type);
        Debug.Log(gameState.Map.Blocks[2][0].Type);
        Debug.Log(gameState.Map.Blocks[3][0].Type);
        Debug.Log(gameState.Map.Blocks[4][0].Type);
        var back_info = JsonParse.ReplayFileParse("init.json").Datas[0];
        var json = BackendData.Convert(back_info);
        gameState.Map.UpdateMap(json.StateChanges);
        Debug.Log(gameState.Map.Blocks[0][0].Type);
        Debug.Log(gameState.Map.Blocks[1][0].Type);
        Debug.Log(gameState.Map.Blocks[2][0].Type);
        Debug.Log(gameState.Map.Blocks[3][0].Type);
        Debug.Log(gameState.Map.Blocks[4][0].Type);
    }
}
