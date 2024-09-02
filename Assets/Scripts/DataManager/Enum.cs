namespace DataManager
{
    /*
     * 这里用来存储一些枚举类型
     */
    public enum BlockType
    {
        TypeError = -1,
        TypeZero = 0,
        TypeOne = 1,
        TypeTwo = 2,
        TypeThree = 3,
        TypeFour = 4,
        TypeFive = 5,
    }
    
    public enum MoveType
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public enum ReturnType
    {
        Correct = 0,
        IndexOutOfRange = -1,
        InvalidBlockType = -2
    }
}