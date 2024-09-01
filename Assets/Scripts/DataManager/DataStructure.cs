using System;
using System.Collections.Generic;

namespace DataManager
{
    public struct Operation
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public MoveType MoveType { get; set; }

        public Operation(int row, int col, MoveType moveType)
        {
            Row = row;
            Col = col;
            MoveType = moveType;
        }
        
        public (int, int) Move(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Up:
                    return (0, 1);
                case MoveType.Down:
                    return (0, -1);
                case MoveType.Left:
                    return (-1, 0);
                case MoveType.Right:
                    return (1, 0);
            }

            return (0, 0);
        }
    }
    
    public struct StateChange
    {
        public List<Block> NewBlocks { get; set; }
        public List<Block> ElimateBlocks { get; set; }

        public StateChange(List<Tuple<int, int, int>> newBlocks, List<Tuple<int, int, int>> eliminateBlocks)
        {
            NewBlocks = new List<Block>();
            ElimateBlocks = new List<Block>();
            foreach (var block in newBlocks)
            {
                NewBlocks.Add(new Block(block));
            }

            foreach (var block in eliminateBlocks)
            {
                ElimateBlocks.Add(new Block(block));
            }
        }
    }
    
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

        public Block(Tuple<int, int, int> tuple)
        {
            Row = tuple.Item1;
            Col = tuple.Item2;
            Type = (BlockType)tuple.Item3;
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

        private void _SwapType(Block block1, Block block2)
        {
            (block1.Type, block2.Type) = (block2.Type, block1.Type);
        }

        public int SwapBlock(Operation operation)
        {
            var (oldRow, oldCol) = (operation.Row, operation.Col);
            var (dx, dy) = operation.Move(operation.MoveType);
            var (newRow, newCol) = (oldRow + dx, oldCol + dy);
            if (!(0 <= oldRow && oldRow < Row && 0 <= oldCol && oldCol < Col
                && 0 <= newRow && newRow < Row && 0 <= newCol && newCol < Col))
            {
                return (int)ReturnType.IndexOutOfRange;
            }
            _SwapType(Blocks[oldRow][oldCol], Blocks[oldRow][newCol]);
            return (int)ReturnType.Correct;
        }

        public int UpdateMap(List<StateChange> stateChanges)
        {
            foreach (var stateChange in stateChanges)
            {
                var eliminateResult = EliminateSomeBlocks(stateChange.ElimateBlocks);
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