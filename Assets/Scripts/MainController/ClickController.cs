using System.Collections.Generic;
using DataManager;
using UnityEngine;


public class ClickController : MonoBehaviour
{
    private bool ChooseOne, ChooseTwo;
    private int FirstX = -1, FirstY = -1;
    private int SecondX = -1, SecondY = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetChosenBlock(int x, int y)
    {
        if (!ChooseOne)
        {
            FirstX = x;
            FirstY = y;
            ChooseOne = true;
            return true;
        }
        if (FirstX == x && FirstY == y)
        { 
            if (!ChooseTwo)
            { 
                ChooseOne = false;
                FirstX = -1; 
                FirstY = -1;
            }
            else
            {
                ChooseTwo = false;
                FirstX = SecondX;
                FirstY = SecondY;
                SecondX = -1;
                SecondY = -1;
            }
            return false;
        }
        if (!ChooseTwo)
        {
            SecondX = x;
            SecondY = y;
            ChooseTwo = true;
            return true;
        }
        if (SecondX == x && SecondY == y)
        {
            ChooseTwo = false;
        }
        else
        {
            Debug.Log("Error, cannot choose three blocks");
        }
        return false;
    }

    public void ConfirmClick()
    {
        if (ChooseOne && ChooseTwo)
        {
            //发送操作
            GetComponentInParent<WebInteractionController>().SendAction( new Operation( new List<List<int>>(){ new List<int>(){FirstX, FirstY}, new List<int>(){SecondX, SecondY} }) );
            ChooseOne = false;
            ChooseTwo = false;
            FirstX = -1;
            FirstY = -1;
            SecondX = -1;
            SecondY = -1;
        }
    }
}
