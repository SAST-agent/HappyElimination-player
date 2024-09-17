using System.Collections.Generic;
using System.IO;
using DataManager;
using Newtonsoft.Json;
using UnityEngine;

public class JsonParse
{

    public static BackendData BackendInfoParse(string json)
    {
        return JsonConvert.DeserializeObject<BackendData>(json);
    }
    
    public static JsonFile ReplayFileParse(string filePath)
    {
        var datas = new JsonFile();
        using (StreamReader sr = new StreamReader(filePath))
        {
            var line = sr.ReadLine();
            while (line != null)
            {
                datas.Add(BackendInfoParse(line));
                line = sr.ReadLine();
            }
        }
        return datas;
    }
}
