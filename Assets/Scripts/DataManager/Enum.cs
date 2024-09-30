namespace DataManager
{
    /*
     * 这里用来存储一些枚举类型
     */
    public enum BlockType
    {
        TypeError = -1,
        TypeOne = 0,
        TypeTwo = 1,
        TypeThree = 2,
        TypeFour = 3,
        TypeFive = 4,
        TypeZero = 5,
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