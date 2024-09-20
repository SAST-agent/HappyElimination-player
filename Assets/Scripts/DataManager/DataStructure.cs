using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DataManager
{
    /*
     * 这里用来存储与游戏相关的数据结构
     * 例如每一个块、游戏地图
     */
    
    public class Block
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public BlockType Type { get; set; }
        
        public Block(int row, int col, BlockType type = BlockType.TypeError)
        {
            Row = row;
            Col = col;
            Type = type;
        }

        public Block(List<int> list)
        {
            while (list.Count < 3)
            {
                list.Add(-1);
            }
            Row = list[0];
            Col = list[1];
            Type = (BlockType)list[2];
        }
    }

    public class Map
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public List<List<Block>> Blocks { get; set; }
        
        // 地图构造函数
        public Map(int row = Constants.ROW, int col = Constants.COL)
        {
            Row = row;
            Col = col;
            Blocks = new List<List<Block>>();
            for (var i = 0; i < row; i++)
            {
                Blocks.Add(new List<Block>());
                for (var j = 0; j < col; j++)
                {
                    Blocks[i].Add(new Block(i, j, BlockType.TypeZero));
                }
            }
        }
        
        // 清空一个地图中的块
        public void ClearBlocks()
        {
            foreach (var row in Blocks)
            {
                foreach (var block in row)
                {
                    block.Type = BlockType.TypeZero;
                }
            }
        }
        
        // 交换两个地块类型的内部函数
        private static void _SwapType(Block block1, Block block2)
        {
            (block1.Type, block2.Type) = (block2.Type, block1.Type);
        }
        
        // 交换两个地块
        public int SwapBlock(Operation operation)
        {
            var oldBlock = Blocks[operation.Block1.Row][operation.Block1.Col];
            var newBlock = Blocks[operation.Block2.Row][operation.Block2.Col];
            _SwapType(oldBlock, newBlock);
            return (int)ReturnType.Correct;
        }

        public List<List<List<int>>> UpdateMapForOneStep(StateChange stateChange)
        {
            var eliminateResult = EliminateSomeBlocks(stateChange.EliminateBlocks);
            if (eliminateResult != 0)
            {
                return null;
            }
            var changeResult = ChangeLeftBlocksPosition();
            if (changeResult.Item1 != 0)
            {
                return null;
            }
            var updateResult = UpdateSomeBlocks(stateChange.NewBlocks);
            if (updateResult != 0)
            {
                // Debug.Log(updateResult);
                return null;
            }
            return changeResult.Item2;
        }
        
        // 更新地图
        public int UpdateMap(List<StateChange> stateChanges)
        {
            foreach (var stateChange in stateChanges)
            {
                UpdateMapForOneStep(stateChange);
            }
            return 0;
        }
        
        // 消除一些地块
        private int EliminateSomeBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.Row >= Row || block.Col >= Col || block.Row < 0 || block.Col < 0)
                {
                    return (int)ReturnType.IndexOutOfRange;
                }
                Blocks[block.Row][block.Col].Type = BlockType.TypeZero;
            }
            return (int)ReturnType.Correct;
        }

        private (int, List<List<List<int>>>) ChangeLeftBlocksPosition()
        {
            var downingBlocks = new List<List<List<int>>>();
            var downs = new int[Row][];
            for (var i = 0; i < Row; i++)
            {
                downs[i] = new int[Col];
                for (var j = 0; j < Col; j++)
                {
                    downs[i][j] = 0;
                }
            }
            for (var i = Row - 2; i >= 0; i--)
            {
                for (var j = Col - 1; j >= 0; j--)
                {
                    downs[i][j] = downs[i + 1][j] + (Blocks[i + 1][j].Type == BlockType.TypeZero ? 1 : 0);
                }
            }

            for (var i = Row - 2; i >= 0; i--)
            {
                for (var j = Col - 1; j >= 0; j--)
                {
                    if (downs[i][j] == 0 || Blocks[i][j].Type == BlockType.TypeZero || Blocks[i][j].Type == BlockType.TypeError)
                    {
                        continue;
                    }
                    // Debug.Log(downs[i][j]);
                    if (downs[i][j] + i > Row)
                    {
                        return ((int)ReturnType.IndexOutOfRange, null);
                    }
                    _SwapType(Blocks[i][j], Blocks[i + downs[i][j]][j]);
                    downingBlocks.Add(new List<List<int>>
                    {
                        new List<int>{i, j},
                        new List<int>{i + downs[i][j], j}
                    });
                }
            }
            return ((int)ReturnType.Correct, downingBlocks);
        }
        
        // 更新一些地块
        private int UpdateSomeBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.Row >= Row || block.Col >= Col || block.Row < 0 || block.Col < 0)
                {
                    return (int)ReturnType.IndexOutOfRange;
                }
                var theBlock = Blocks[block.Row][block.Col];
                if (theBlock.Type != BlockType.TypeZero)
                {
                    return (int)ReturnType.InvalidBlockType;
                }
                theBlock.Type = block.Type;
            }
            return (int)ReturnType.Correct;
        }
    }

    public class GameState
    {
        public int Round { get; set; }
        public int Player { get; set; }
        public int Steps { get; set; }
        public List<int> Scores { get; set; }
        public Map Map { get; set; }

        public GameState()
        {
            Round = -1;
            Player = -1;
            Steps = -1;
            Scores = new List<int>(2);
            Map = new Map();
        }
    }
}