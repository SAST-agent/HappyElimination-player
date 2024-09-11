using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectController : MonoBehaviour
{
    public float bias;
    public GameObject[] prefabs;
    List<List<GameObject>> objectList;
    int[] top;

    public float swapTime;
    // Start is called before the first frame update
    void Start()
    {
        objectList = new List<List<GameObject>>();
        top = new int[Constants.COL];
        for (int j = 0; j < Constants.COL; j++)
        {
            top[j] = 0;
        }
        for (int i = 0; i < Constants.ROW; i++)
        {
            objectList.Add(new List<GameObject>());
            for (int j = 0; j < Constants.COL; j++)
            {
                objectList[i].Add(new GameObject());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void AddObject(int row, BlockType type)
    {
        while (row >= objectList.Count)
        {
            objectList.Add(new List<GameObject>());
        }
        var newObject = Instantiate(prefabs[(int)type]);
        objectList[row].Add(newObject);
        int col = objectList[row].Count - 1;
        newObject.name = "Block" + row + "_" + col;
        newObject.transform.parent = GameObject.Find("MapObjects").transform;
        newObject.transform.position = new Vector3(col * bias, - row * bias, 0);
    }
    */

    public void SwapObject(int row1, int col1, int row2, int col2)
    {
        var object1 = objectList[row1][col1];
        var object2 = objectList[row2][col2];
        object1.name = "Block" + row2 + "_" + col2;
        object2.name = "Block" + row1 + "_" + col1;

        objectList[row2][col2] = object1;
        objectList[row1][col1] = object2;

        var motion1 = object1.GetComponent<Move>();
        var collider1 = object1.GetComponent<BoxCollider2D>();
        collider1.enabled = false;
        var motion2 = object2.GetComponent<Move>();
        var collider2 = object2.GetComponent<BoxCollider2D>();
        collider2.enabled = false;

        if (row1 == row2 && col1 < col2)
        {
            motion1.SetMove(Vector3.right, swapTime);
            motion2.SetMove(Vector3.left, swapTime);
        }
        else if (row1 == row2 && col1 > col2)
        {
            motion1.SetMove(Vector3.left, swapTime);
            motion2.SetMove(Vector3.right, swapTime);
        }
        else if (col1 == col2 && row1 < row2)
        {
            motion1.SetMove(Vector3.down, swapTime);
            motion2.SetMove(Vector3.up, swapTime);
        }
        else if (col1 == col2 && row1 > row2)
        {
            motion1.SetMove(Vector3.up, swapTime);
            motion2.SetMove(Vector3.down, swapTime);
        }
        else
        {
            Debug.Log("illegal swap");
        }

        collider1.enabled = true;
        collider2.enabled = true;
    }

    public void UpdateMapObject(StateChange stateChange)
    {
        EliminateObject(stateChange.EliminateBlocks);
        UpdateObject(stateChange.NewBlocks);
    }

    void EliminateObject(List<Block> list)
    {
        if (list.Count < 3)
        {
            return;
        }
        foreach (var block in list)
        {
            var tmpObject = objectList[block.Row][block.Col];
            objectList[block.Row][block.Col] = new GameObject();
            Destroy(tmpObject);
        }
    }

    void UpdateObject(List<Block> list)
    {
        var mapObject = GameObject.Find("MapObjects");
        if (list.Count < 3)
        {
            return;
        }
        foreach (var block in list)
        {
            var tmpObject = objectList[block.Row][block.Col];
            var newObject = Instantiate(prefabs[(int)block.Type]);
            newObject.name = "Block" + block.Row + "_" + block.Col;
            newObject.transform.parent = GameObject.Find("MapObjects").transform;
            if (tmpObject.CompareTag("Block"))
            {
                newObject.transform.position = tmpObject.transform.position;
            }
            else
            {
                newObject.transform.position = new Vector3(block.Col * bias + 0.25f, -block.Row * bias - 0.25f, 0) + mapObject.transform.position;
            }
            objectList[block.Row][block.Col] = newObject;
            Destroy(tmpObject);
        }
    }
}
