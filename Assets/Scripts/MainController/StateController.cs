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
        gameState.Map.UpdateMap(initialData.StateChanges);
    }

    public Map GetMap()
    {
        return gameState.Map;
    }
}
