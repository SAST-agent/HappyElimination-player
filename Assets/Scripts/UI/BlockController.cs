using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    public Button blockButton;
    public LineRenderer lineRenderer;
    public SpriteRenderer spriteRenderer;
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
        lineRenderer.enabled = false;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
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
        Debug.Log(row + "," + col);
        if (GameObject.Find("Main Controller").GetComponent<ClickController>().SetChosenBlock(row, col))
        {
            lineRenderer.enabled = true;
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1); // RGBA
        }
        else
        {
            lineRenderer.enabled = false;
            // 改变颜色为红色，完全不透明
            spriteRenderer.color = new Color(1, 1, 1, 1); // RGBA
        }
    }
}
