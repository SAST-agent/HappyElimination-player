using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapInitialize(JsonData initialData)
    {
        Map map = GetComponent<StateController>().GetMap();
        var gameObjectController = GetComponent<GameObjectController>();
        for (int i = 0; i < map.Row; i++)
        {
            for (int j = 0; j < map.Col; j++)
            {
                gameObjectController.AddObject(i, map.Blocks[i][j].Type);
            }
        }
    }
}
