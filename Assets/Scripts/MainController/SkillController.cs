using System.Collections;
using System.Collections.Generic;
using DataManager;
using UnityEngine;
using UnityEngine.UI;

//技能的控制器

public class SkillController : MonoBehaviour
{
    public int Type;
    public Outline lineRenderer;
    private int player;
    // 确认当前技能是否有边框
    private bool isActive = false;

    void Start()
    {
        lineRenderer = GetComponent<Outline>();
        if (lineRenderer == null)
        {
            // 如果没有 Outline 组件，就添加一个
            lineRenderer = gameObject.AddComponent<Outline>();
        }
        lineRenderer.enabled = false;
        player = StateController.getPlayer();
    }

    void Update()
    {
        if(ModeController.IsReplayMode() && isActive)
        {
            if(StateController.getSkill()[player] == Type)
            {
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }

    public void OnButtonDown()
    {
        if(!ModeController.IsInteractMode() || StateController.IsPlaying())
            return;
        if (GameObject.Find("Main Controller").GetComponent<ClickController>().ClickSkill(Type)) //正常点的情况
        {
            lineRenderer.enabled = true;
            Debug.Log("Skill " + Type + " activated");
            isActive = true;
        }
        else //自己点掉自己的情况
        {
            if (StateController.getSkill()[player] == Type) return;  //如果当前技能是激活状态，点掉无效
            lineRenderer.enabled = false;
            Debug.Log("Skill " + Type + " deactivated");
            isActive = false;
        }
    }

    public void AnotherSkillActivated() //被另一个技能点掉的情况
    {
        lineRenderer.enabled = false;
        isActive = true;
        Debug.Log("Skill " + Type + " deactivated by another skill");
    }
}
