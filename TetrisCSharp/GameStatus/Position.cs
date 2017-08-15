
namespace TetrisCSharp.GameStatus
{
    public struct Position
    {
        public short row {get; }
        public short column {get; }

        public static readonly Position up = new Position(-1, 0);
        public static readonly Position down = new Position(1, 0);
        public static readonly Position left = new Position(0, -1);
        public static readonly Position right = new Position(0, 1);

        public Position(short row, short column)
        {
            this.row = row;
            this.column = column;
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position((short)(p1.row + p2.row), (short)(p1.column + p2.column));
        }

        public static Position operator *(Position p1, int scale)
        {
            return new Position((short)(p1.row *scale), (short)(p1.column *scale));
        }
    }
}
