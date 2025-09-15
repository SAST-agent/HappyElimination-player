using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//技能的控制器

public class SkillController : MonoBehaviour
{
    public int Type;
    public Outline lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<Outline>();
        if (lineRenderer == null)
        {
            // 如果没有 Outline 组件，就添加一个
            lineRenderer = gameObject.AddComponent<Outline>();
        }
        lineRenderer.enabled = false;
    }

    public void OnButtonDown()
    {
        if (GameObject.Find("Main Controller").GetComponent<ClickController>().ClickSkill(Type))
        {
            lineRenderer.enabled = true;
            Debug.Log("Skill " + Type + " activated");
        }
        else
        {
            lineRenderer.enabled = false;
            Debug.Log("Skill " + Type + " deactivated");
        }
    }

    public void AnotherSkillActivated()
    {
        lineRenderer.enabled = false;
        Debug.Log("Skill " + Type + " deactivated by another skill");
    }
}
