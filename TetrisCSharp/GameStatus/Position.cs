
namespace TetrisCSharp.GameStatus
{
    public struct Position
    {
        public short row {get; }
        public short column {get; }

        public Position(short row, short column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
