using TetrisCSharp.GameStatus.TetrisPieces;

namespace TetrisCSharp.GameStatus
{
    public class GameBoard
    {
        private const byte ROW_SIZE = 20;
        private const byte COL_SIZE = 10;     
        public Block[,] board { get; }
        public static readonly Position spawn = new Position(0, 4);

        public GameBoard()
        {
            board = new Block[ROW_SIZE, COL_SIZE];
        }

        public bool isBlockFree(Position pos)
        {
            return board[pos.row, pos.column].type == TetrisPieceEnum.EMPTY;
        }

        public void setBlock(Position pos, TetrisPieceEnum type)
        {
            board[pos.row, pos.column] = new Block(type);
        }

    }
}
