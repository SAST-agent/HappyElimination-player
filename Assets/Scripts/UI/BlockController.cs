using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    public Button blockButton;
    private LineRenderer lineRenderer;
    public float bias;
    public Vector3 direction;
    float moveTime;
    float timer;

    public int row;
    public int col;

    GameObject mapObject;
    
    // Start is called before the first frame update
    void Start()
    {
        mapObject = GameObject.Find("MapObjects");
        if (blockButton == null)
        {
            Debug.Log("Block Button is null");
        }
        
        lineRenderer = GetComponent<LineRenderer>();

        // 启用世界坐标模式还是本地坐标模式
        lineRenderer.useWorldSpace = false;

        // 设置 LineRenderer 属性，如颜色和宽度
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.05f;  // 边框线条的宽度
        lineRenderer.endWidth = 0.05f;

        // 设置边框点 (假设正方形边长为 1，且物体中心在原点)
        lineRenderer.positionCount = 5; // 四个角 + 1 个闭合点

        // 设置正方形的四个角的本地坐标（2D 正方形，位于 XY 平面）
        lineRenderer.SetPosition(0, new Vector3(-0.25f, -0.25f, 0));  // 左下角
        lineRenderer.SetPosition(1, new Vector3(0.25f, -0.25f, 0));   // 右下角
        lineRenderer.SetPosition(2, new Vector3(0.25f, 0.25f, 0));    // 右上角
        lineRenderer.SetPosition(3, new Vector3(-0.25f, 0.25f, 0));   // 左上角
        lineRenderer.SetPosition(4, new Vector3(-0.25f, -0.25f, 0));  // 回到左下角，闭合正方形
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < moveTime)
        {
            transform.position += direction * Time.deltaTime;
            timer += Time.deltaTime;
        }
        else
        {
            transform.position = new Vector3(col * bias + 0.25f, -row * bias - 0.25f, 0) + mapObject.transform.position;
        }
    }

    public void SetMove(Vector3 targetDirection, float moveTotalTime)
    {
        direction = targetDirection / moveTotalTime;
        moveTime = moveTotalTime;
        timer = 0;
    }

    public void SetPosition(int _row, int _col)
    {
        row = _row; col = _col;
    }

    public List<int> GetPosition()
    {
        return new List<int> { row, col };
    }
    
    private void OnMouseDown()
    {
        Debug.Log( row + "," + col );
        lineRenderer.enabled =
            GameObject.Find("Main Controller").GetComponent<ClickController>().SetChosenBlock(row, col);
    }
}
