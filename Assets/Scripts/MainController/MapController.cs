using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public void MapInitialize(JsonData initialData)
    {
        var gameObjectController = GetComponent<GameObjectController>();
        gameObjectController.UpdateMapObject(initialData.StateChanges[0], 1f);
    }
}
