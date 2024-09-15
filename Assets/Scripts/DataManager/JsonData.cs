using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;

namespace DataManager
{   
    /*
     * 这里存储一些需要序列化与反序列化的数据结构
     * 例如前后端通信使用的Json格式
     * 回放文件的Json格式等等
     */
    
    public class Operation
    {
        // 玩家操作类型
        public Block Block1 { get; set; }
        public Block Block2 { get; set; }

        public Operation(List<List<int>> list)
        {
            if (list is not { Count: 2 })
            {
                Block1 = null;
                Block2 = null;
            }
            else
            {
                Block1 = new Block(list[0]);
                Block2 = new Block(list[1]);
            }
        }

        public override string ToString()
        {
            return $"{Block1.Row} {Block1.Col} {Block2.Row} {Block2.Col}";
        }
    }
    
    public class StateChange
    {
        // 每次操作之后的状态改变
        public List<Block> NewBlocks { get; set; }
        public List<Block> EliminateBlocks { get; set; }

        public StateChange()
        {
            NewBlocks = new List<Block>();
            EliminateBlocks = new List<Block>();
        }
        
        public StateChange(List<Block> newBlocks, List<Block> eliminateBlocks)
        {
            NewBlocks = newBlocks;
            EliminateBlocks = eliminateBlocks;
        }

        public StateChange(List<List<int>> newBlocks, List<List<int>> eliminateBlocks)
        {
            NewBlocks = new List<Block>();
            EliminateBlocks = new List<Block>();
            foreach (var block in newBlocks)
            {
                NewBlocks.Add(new Block(block));
            }

            foreach (var block in eliminateBlocks)
            {
                EliminateBlocks.Add(new Block(block));
            }
        }
    }
    
    public class JsonData
    {
        // 回放文件每一行的格式
        public int Round { get; set; }
        public int Player { get; set; }
        public int Steps { get; set; }
        public List<int> Scores { get; set; }
        public Operation Operation { get; set; }
        public List<StateChange> StateChanges { get; set; }

        public JsonData()
        {
            Round = -1;
            Player = -1;
            Steps = -1;
            Scores = null;
            Operation = null;
            StateChanges = null;
        }
    }
    
    public class BackendData
    {
        public int Round { get; set; }
        public int Player { get; set; }
        public int Steps { get; set; }
        public List<int> Scores { get; set; }
        public List<List<int>> Operation { get; set; }
        public List<List<List<int>>> ManyTimesNewBlocks { get; set; }
        public List<List<List<int>>> ManyTimesEliminateBlocks { get; set; }
        public string StopReason { get; set; }

        public BackendData()
        {
            Round = -1;
            Player = -1;
            Steps = -1;
            Scores = null;
            Operation = null;
            ManyTimesNewBlocks = null;
            ManyTimesEliminateBlocks = null;
            StopReason = null;
        }
        
        // 将后端传过来的信息转换为前端可以解析的JsonData
        public static JsonData Convert(BackendData backendData)
        {
            if (backendData.ManyTimesNewBlocks.Count != backendData.ManyTimesEliminateBlocks.Count)
            {
                return null;
            }
            var jsonData = new JsonData
            {
                Round = backendData.Round, 
                Player = backendData.Player, 
                Steps = backendData.Steps, 
                Scores = backendData.Scores,
                Operation = new Operation(backendData.Operation)
            };
            var stateChanges = new List<StateChange>();
            var manyTimesBlockChanges = backendData.ManyTimesNewBlocks.Zip(
                    backendData.ManyTimesEliminateBlocks, (n, e) => new StateChange(n, e)
                );
            stateChanges.AddRange(manyTimesBlockChanges);
            jsonData.StateChanges = stateChanges;
            return jsonData;
        }
    }

    public class JsonFile
    {
        // 回放文件，也就是json的列表
        public List<BackendData> Datas { get; set; }

        public JsonFile()
        {
            Datas = new List<BackendData>();
        }
        
        public void Add(BackendData data)
        {
            Datas.Add(data);
        }
    }
    
    // 以下的命名都不符合C#命名规范
    // 目的是要与通信所需json的key相匹配以保证能正确解析
    public class Info
    {
        public string request { get; set; }
        public string token { get; set; }
        public string content { get; set; }
    }

    public class HistoryInfo
    {
        public string request { get; set; }
        public List<string> content { get; set; }
    }

    public class WatchInfo
    {
        public string request { get; set; }
    }

    public class JudgerData
    {
        public string request { get; set; }
        public string content { get; set; }
    }

    // 网页发送过来的信息
    [Serializable]
    public class FrontendData
    {
        public enum MsgType
        {
            init_player_player,
            init_replay_player,
            load_frame,
            load_next_frame,
            load_players,
            play_speed,
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public MsgType message { get; set; }
        public string payload { get; set; }
        public string token { get; set; }
        public int speed { get; set; }
        public List<BackendData> replay_data { get; set; }
        public int index { get; set; }
        public List<string> players { get; set; }
    }

    // 回复网页的信息
    [Serializable]
    public class FrontendReplyData
    {
        public enum MsgType
        {
            init_successfully,
            initialize_result,
            game_record,    
            error_marker,
            loaded // 表示当前unity已经初始化完成，可以开始接收评测机的信息
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public MsgType message { get; set; }
        public int number_of_frames { get; set; }
        public int height { get; set; }
        public bool init_result { get; set; }
        public string game_record { get; set; }
        public string err_msg { get; set; }
    }
}