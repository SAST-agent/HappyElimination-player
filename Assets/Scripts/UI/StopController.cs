using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 结束条件显示控制器
/// </summary>
public class StopController : MonoBehaviour
{
    [SerializeField]
    private Text winnerText;
    [SerializeField]
    private Text stopText;

    public void ShowStopReason(int winner, string stopReason)
    {
        gameObject.SetActive(true);
        if (winnerText != null)
        {
            winnerText.text = "胜者：" + winner.ToString();
        }
        if (stopText != null)
        {
            if (stopReason.Contains("Invalid Operation 100 100 100 100"))
                stopText.text = "结束原因" + (1 - winner).ToString() + "号玩家认输";
            else
                stopText.text = "结束原因" + stopReason;
        }
    }
    
    public void HideStopReason(){
        gameObject.SetActive(false);    
    }
}
