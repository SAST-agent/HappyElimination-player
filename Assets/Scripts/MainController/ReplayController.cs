using DataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    public int nowRound;
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
        return BackendData.Convert(replay.Datas[0]);
    }

    public void PlayRound()
    {
        // swap and delete and new
        var roundToPlay = BackendData.Convert(replay.Datas[nowRound++]);
        // TODO: finish round playing
    }
    // (swap) and (delete and new) in interaction controller
}
