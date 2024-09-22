using UnityEngine;

/// <summary>
/// UI元素控制器
/// </summary>
public class UIController: MonoBehaviour
{
    public GameObject gameInfo;
    public GameObject stopReason;

    public void UpdateRound()
    {
        gameInfo.GetComponent<GameInfoController>().UpdateRound();
    }
    
    public void UpdateScore()
    {
        gameInfo.GetComponent<GameInfoController>().UpdateScore();
    }

    public void GameStop(string reason)
    {
        stopReason.GetComponent<StopController>().ShowStopReason(reason);
    }
    
    public void GameRestart(){
        stopReason.GetComponent<StopController>().HideStopReason();  
    }
}