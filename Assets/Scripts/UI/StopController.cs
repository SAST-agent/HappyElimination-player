using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 结束条件显示控制器
/// </summary>
public class StopController : MonoBehaviour
{
    [SerializeField]
    private Text stopText;

    public void ShowStopReason(string stopReason)
    {
        gameObject.SetActive(true);
        if (stopText != null)
        {
            stopText.text = stopReason;
        }
    }
}
