using TetrisCSharp.GameStatus.TetrisPieces;

namespace TetrisCSharp.GameStatus
{
    public struct ActivePiece
    {
        public TetrisPiece movingPiece { get; private set; }
        public Position position;

        public ActivePiece (TetrisPiece piece, Position pos)
        {
            movingPiece = piece;
            position = pos;
        }
    }
}
