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
        if(ModeController.IsReplayMode())
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
        if (GameObject.Find("Main Controller").GetComponent<ClickController>().ClickSkill(Type)) //正常点的情况
        {
            lineRenderer.enabled = true;
            Debug.Log("Skill " + Type + " activated");
        }
        else //自己点掉自己的情况
        {
            if (StateController.getSkill()[player] == Type) return;
            lineRenderer.enabled = false;
            Debug.Log("Skill " + Type + " deactivated");
        }
    }

    public void AnotherSkillActivated() //被另一个技能点掉的情况
    {
        lineRenderer.enabled = false;
        Debug.Log("Skill " + Type + " deactivated by another skill");
    }
}
