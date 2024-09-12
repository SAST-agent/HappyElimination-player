using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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

        public BackendData()
        {
            Round = -1;
            Player = -1;
            Steps = -1;
            Scores = null;
            Operation = null;
            ManyTimesNewBlocks = null;
            ManyTimesEliminateBlocks = null;
        }

        public BackendData(int round, int player, int steps, List<int> scores, List<List<int>> operation, 
            List<List<List<int>>> newBlocks, List<List<List<int>>> eliminateBlocks)
        {
            Round = round;
            Player = player;
            Steps = steps;
            Scores = scores;
            Operation = operation;
            ManyTimesNewBlocks = newBlocks;
            ManyTimesEliminateBlocks = eliminateBlocks;
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
}