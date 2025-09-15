using System.Collections.Generic;
using DataManager;
using UnityEngine;

/// <summary>
/// 控制用户的点击事件
/// </summary>
public class ClickController : MonoBehaviour
{
    private bool _chooseOne, _chooseTwo;
    private int _firstX = -1, _firstY = -1;
    private int _secondX = -1, _secondY = -1;

    void Update()
    {
        // 监听主键盘的 Enter 键
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEnterKeyPressed();
        }

        // 监听小键盘的 Enter 键
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnEnterKeyPressed();
        }
    }
    
    public bool SetChosenBlock(int x, int y)
    {
        if (!ModeController.IsInteractMode() || StateController.IsPlaying())
            return false;
        if (!_chooseOne)
        {
            _firstX = x;
            _firstY = y;
            _chooseOne = true;
            return true;
        }
        if (_firstX == x && _firstY == y)
        { 
            if (!_chooseTwo)
            { 
                _chooseOne = false;
                _firstX = -1; 
                _firstY = -1;
            }
            else
            {
                _chooseTwo = false;
                _firstX = _secondX;
                _firstY = _secondY;
                _secondX = -1;
                _secondY = -1;
            }
            return false;
        }
        if (!_chooseTwo)
        {
            _secondX = x;
            _secondY = y;
            _chooseTwo = true;
            return true;
        }
        if (_secondX == x && _secondY == y)
        {
            _chooseTwo = false;
        }
        else
        {
            Debug.Log("Error, cannot choose three blocks");
        }
        return false;
    }

    public void ConfirmClick()
    {
        if (_chooseOne && _chooseTwo && !StateController.IsPlaying())
        {
            //发送操作
            GetComponentInParent<WebInteractionController>().SendAction(new Operation(new List<List<int>>() { new List<int>() { _firstX, _firstY }, new List<int>() { _secondX, _secondY } }));
            GameObject block1 = GetComponentInParent<GameObjectController>().ObjectList[_firstX][_firstY], block2 = GetComponentInParent<GameObjectController>().ObjectList[_secondX][_secondY];
            block1.GetComponent<BlockController>().lineRenderer.enabled = false;
            block1.GetComponent<BlockController>().spriteRenderer.color = new Color(1, 1, 1, 1);
            block2.GetComponent<BlockController>().lineRenderer.enabled = false;
            block2.GetComponent<BlockController>().spriteRenderer.color = new Color(1, 1, 1, 1);
            _chooseOne = false;
            _chooseTwo = false;
            _firstX = -1;
            _firstY = -1;
            _secondX = -1;
            _secondY = -1;
        }
    }
    
    public void DefeatClick()
    {
        if (ModeController.IsInteractMode() && !StateController.IsPlaying())
        {
            //发送操作
            GetComponentInParent<WebInteractionController>().SendAction(new Operation(new List<List<int>>() { new List<int>() { 100, 100 }, new List<int>() { 100, 100 } }));
            if (_chooseOne)
            {
                GameObject block1 = GetComponentInParent<GameObjectController>().ObjectList[_firstX][_firstY];
                block1.GetComponent<BlockController>().lineRenderer.enabled = false;
                block1.GetComponent<BlockController>().spriteRenderer.color = new Color(1, 1, 1, 1);
                _chooseOne = false;
                _firstX = -1;
                _firstY = -1;
            }
            if (_chooseTwo)
            {
                GameObject block2 = GetComponentInParent<GameObjectController>().ObjectList[_secondX][_secondY];
                block2.GetComponent<BlockController>().lineRenderer.enabled = false;
                block2.GetComponent<BlockController>().spriteRenderer.color = new Color(1, 1, 1, 1);
                _chooseTwo = false;
                _secondX = -1;
                _secondY = -1;
            }
        }
    }

    public void SkillClick1()
    {
        if(ModeController.IsInteractMode() && !StateController.IsPlaying())
        {
            //发送操作
            GetComponentInParent<WebInteractionController>().SendAction(new Operation(new List<List<int>>() { new List<int>() { 101, 101 }, new List<int>() { 101, 101 } }));
        }
    }

    public void OnEnterKeyPressed()
    {
        if (ModeController.IsInteractMode() && !StateController.IsPlaying())
        {
            int x1 = Random.Range(0, 20), y1 = Random.Range(0, 20);
            int x2 = Random.Range(0, 20), y2 = Random.Range(0, 20);
            while (x1 == x2 && y1 == y2)
            {
                x2 = Random.Range(0, 20);
                y2 = Random.Range(0, 20);
            }
            //发送操作
            GetComponentInParent<WebInteractionController>().SendAction(new Operation(new List<List<int>>() { new List<int>() { x1, y1 }, new List<int>() { x2, y2 } }));
            if (_chooseOne)
            {
                GameObject block1 = GetComponentInParent<GameObjectController>().ObjectList[_firstX][_firstY];
                block1.GetComponent<BlockController>().lineRenderer.enabled = false;
                block1.GetComponent<BlockController>().spriteRenderer.color = new Color(1, 1, 1, 1);
                _chooseOne = false;
                _firstX = -1;
                _firstY = -1;
            }
            if (_chooseTwo)
            {
                GameObject block2 = GetComponentInParent<GameObjectController>().ObjectList[_secondX][_secondY];
                block2.GetComponent<BlockController>().lineRenderer.enabled = false;
                block2.GetComponent<BlockController>().spriteRenderer.color = new Color(1, 1, 1, 1);
                _chooseTwo = false;
                _secondX = -1;
                _secondY = -1;
            }
        }
    }
    
}
