using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    JsonFile replay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseReplay(string filePath)
    {
        replay = JsonParse.ReplayFileParse(filePath);
    }

    public JsonData GetInitialData()
    {
        return replay.Datas[0];
    }
}
