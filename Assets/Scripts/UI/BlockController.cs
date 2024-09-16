using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
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
}
