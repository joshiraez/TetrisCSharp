
namespace TetrisCSharp.GameStatus
{
    public struct Position
    {
        public byte row {get; set;}
        public byte column {get; set;}

        public Position(byte row, byte column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
