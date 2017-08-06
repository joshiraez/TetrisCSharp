
namespace TetrisCSharp.GameStatus
{
    public struct Position
    {
        public byte row {get; }
        public byte column {get; }

        public Position(byte row, byte column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
