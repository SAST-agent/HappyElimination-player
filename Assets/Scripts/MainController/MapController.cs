using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public void MapInitialize(JsonData initialData)
    {
        var gameObjectController = GetComponent<GameObjectController>();
        gameObjectController.UpdateMapObject(initialData.StateChanges[0]);
    }

    public void DoOperationOnMap(Operation operation, float animationSpeed = 1.0f)
    {
        var gameObjectController = GetComponent<GameObjectController>();
        gameObjectController.SwapObject(
            operation.Block1.Row, operation.Block1.Col,
            operation.Block2.Row, operation.Block2.Col,
            animationSpeed
        );
    }

    public void UpdateMap(StateChange stateChange, float animationSpeed = 1.0f)
    {
        var objectController = GetComponent<GameObjectController>();
        objectController.UpdateMapObject(stateChange, animationSpeed);
    }

    public void ReGenerateMap()
    {
        var gameObjectController = GetComponent<GameObjectController>();
        gameObjectController.ClearBlocks();
        gameObjectController.GenerateObjByState();
    }
}
