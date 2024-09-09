using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectController : MonoBehaviour
{
    public float bias;
    public GameObject[] prefabs;
    List<List<GameObject>> objectList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
