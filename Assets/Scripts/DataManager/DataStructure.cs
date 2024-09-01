using System;
using System.Collections.Generic;

namespace DataManager
{
    
    public struct Block
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
            Row = list[0];
            Col = list[1];
            Type = (BlockType)list[2];
        }
    }
    
    public struct Operation
    {
        public Block Block1 { get; set; }
        public Block Block2 { get; set; }

        public Operation(List<List<int>> list)
        {
            Block1 = new Block(list[0]);
            Block2 = new Block(list[1]);
        }
    }
    
    public struct StateChange
    {
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

    public class Map
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public List<List<Block>> Blocks { get; set; }

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
                    Blocks[i].Add(new Block(i, j));
                }
            }
        }

        private static void _SwapType(Block block1, Block block2)
        {
            (block1.Type, block2.Type) = (block2.Type, block1.Type);
        }

        public int SwapBlock(Operation operation)
        {
            var oldBlock = Blocks[operation.Block1.Row][operation.Block1.Col];
            var newBlock = Blocks[operation.Block2.Row][operation.Block2.Col];
            _SwapType(oldBlock, newBlock);
            return (int)ReturnType.Correct;
        }

        public int UpdateMap(List<StateChange> stateChanges)
        {
            foreach (var stateChange in stateChanges)
            {
                var eliminateResult = EliminateSomeBlocks(stateChange.EliminateBlocks);
                if (eliminateResult != 0)
                {
                    return eliminateResult;
                }
                var updateResult = UpdateSomeBlocks(stateChange.NewBlocks);
                if (updateResult != 0)
                {
                    return updateResult;
                }
            }
            return 0;
        }

        private int EliminateSomeBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.Row >= Row || block.Col >= Col || block.Row < 0 || block.Col < 0)
                {
                    return (int)ReturnType.IndexOutOfRange;
                }
                var theBlock = Blocks[block.Row][block.Col];
                if (theBlock.Type != block.Type)
                {
                    return (int)ReturnType.InvalidBlockType;
                }
                theBlock.Type = BlockType.TypeZero;
            }
            return (int)ReturnType.Correct;
        }

        private int UpdateSomeBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.Row >= Row || block.Col >= Col || block.Row < 0 || block.Col < 0)
                {
                    return (int)ReturnType.IndexOutOfRange;
                }
                var theBlock = Blocks[block.Row][block.Col];
                if (theBlock.Type != block.Type)
                {
                    return (int)ReturnType.InvalidBlockType;
                }
                theBlock.Type = block.Type;
            }
            return (int)ReturnType.Correct;
        }
    }
}