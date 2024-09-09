using System;
using System.Collections.Generic;
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
            if (list == null)
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
    
    public struct StateChange
    {
        // 每次操作之后的状态改变
        public List<Block> NewBlocks { get; set; }
        public List<Block> EliminateBlocks { get; set; }

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
        // 回放文件每一行的格式，同时也是后端发过来的格式
        public int Round { get; set; }
        public int Player { get; set; }
        public int Steps { get; set; }
        public Operation Operation { get; set; }
        public List<StateChange> StateChanges { get; set; }

        public JsonData()
        {
            Round = -1;
            Player = -1;
            Steps = -1;
            Operation = null;
            StateChanges = null;
        }
    }

    public class JsonFile
    {
        // 回放文件，也就是json的列表
        public List<JsonData> Datas { get; set; }

        public JsonFile()
        {
            Datas = new List<JsonData>();
        }
        
        public void Add(JsonData data)
        {
            Datas.Add(data);
        }
    }
}