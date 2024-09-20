using UnityEngine;

/// <summary>
/// UI元素控制器
/// </summary>
public class UIController: MonoBehaviour
{
    public GameObject gameInfo;
    public GameObject stopReason;
    
    public void UpdateGameInfo()
    {
        gameInfo.GetComponent<GameInfoController>().UpdateGameInfo();
    }

    public void GameStop(string reason)
    {
        stopReason.GetComponent<StopController>().ShowStopReason(reason);
    }
}