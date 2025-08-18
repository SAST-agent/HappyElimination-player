using DataManager;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏物体控制器
/// </summary>
public class GameObjectController : MonoBehaviour
{
    public float bias;
    public GameObject[] prefabs;
    public List<List<GameObject>> ObjectList;

    public float swapTime;
    // Start is called before the first frame update
    void Start()
    {
        ObjectList = new List<List<GameObject>>();
        for (int i = 0; i < Constants.ROW; i++)
        {
            ObjectList.Add(new List<GameObject>());
            for (int j = 0; j < Constants.COL; j++)
            {
                ObjectList[i].Add(new GameObject());
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

    public void SwapObject(int row1, int col1, int row2, int col2, float animationSpeed = 1.0f)
    {
        var object1 = ObjectList[row1][col1];
        var object2 = ObjectList[row2][col2];
        object1.name = "Block" + row2 + "_" + col2;
        object2.name = "Block" + row1 + "_" + col1;

        ObjectList[row2][col2] = object1;
        ObjectList[row1][col1] = object2;

        var motion1 = object1.GetComponent<BlockController>();
        motion1.SetPosition(row2, col2);
        var motion2 = object2.GetComponent<BlockController>();
        motion2.SetPosition(row1, col1);

        // Debug.Log(motion1.GetPosition()[0] + " " + motion1.GetPosition()[1]);

        float adjustSwapTime = swapTime / animationSpeed;

        Vector3 direction = object1.transform.position - object2.transform.position;

        motion2.SetMove(direction, adjustSwapTime);
        motion1.SetMove(-direction, adjustSwapTime);
    }

    public void UpdateMapObject(StateChange stateChange, float animationSpeed = 1.0f)
    {
        // for (int j = 0; j < Constants.COL; j++)
        // {
        //     top[j] = 0;
        // }
        EliminateObject(stateChange.EliminateBlocks);
        Invoke(nameof(UpdateObject), 0.3f / animationSpeed);
    }

    void EliminateObject(List<Block> list)
    {
        if (list.Count < 3)
        {
            return;
        }
        foreach (var block in list)
        {
            // modify top to count eliminated blocks
            // if (block.Row == top[block.Col])
            // {
            //     top[block.Col] += 1;
            // }
            var tmpObject = ObjectList[block.Row][block.Col];
            ObjectList[block.Row][block.Col] = new GameObject();
            Destroy(tmpObject);
        }
    }

    void UpdateObject()
    {
        var mapObject = GameObject.Find("MapObjects");
        var map = StateController.GetMap();
        for (int i = 0; i < Constants.ROW; i++)
        {
            for (int j = 0; j < Constants.COL; j++)
            { 
                // Debug.Log(i + " " + j + " " + (int)map.Blocks[i][j].Type);
                var tmpObject = ObjectList[i][j];
                var newObject = Instantiate(prefabs[(int)map.Blocks[i][j].Type]);

                newObject.name = "Block" + i + "_" + j;
                var blockController = newObject.GetComponent<BlockController>();
                blockController.SetPosition(i, j);

                newObject.transform.parent = GameObject.Find("MapObjects").transform;
                /*
                if (tmpObject.CompareTag("Block"))
                {
                    // if the old one is real block
                    newObject.transform.position = tmpObject.transform.position;
                }
                else
                {*/
                    // if the old one is an empty gameobject
                    newObject.transform.position = new Vector3(j * bias + 0.25f, -i * bias - 0.25f, 0) + mapObject.transform.position;
                //}
                
                ObjectList[i][j] = newObject;
                Destroy(tmpObject);
            }
        }
    }

    public void ClearBlocks()
    {
        var map = StateController.GetMap();
        for (var i = 0; i < map.Row; i++)
        {
            for (var j = 0; j < map.Col; j++)
            {
                Destroy(ObjectList[i][j]);
            }
        }
    }

    public void GenerateObjByState()
    {
        UpdateObject();
    }
}
