using System.Collections.Generic;
using System.IO;
using DataManager;
using Newtonsoft.Json;
public class JsonParse
{

    public static JsonData BackendInfoParse(string json)
    {
        return JsonConvert.DeserializeObject<JsonData>(json);
    }
    
    public static JsonFile ReplayFileParse(string filePath)
    {
        var datas = new JsonFile();
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = sr.ReadLine();
            while (line != null)
            {
                datas.Add(JsonConvert.DeserializeObject<JsonData>(line));
                line = sr.ReadLine();
            }
        }
        return datas;
    }
}
