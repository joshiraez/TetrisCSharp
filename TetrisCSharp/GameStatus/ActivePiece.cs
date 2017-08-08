using TetrisCSharp.GameStatus.TetrisPieces;

namespace TetrisCSharp.GameStatus
{
    public struct ActivePiece
    {
        private TetrisPiece piece { get; }
        private Position position;

        public ActivePiece (TetrisPieceEnum pieceType, Position pos)
        {
            this.piece = new TetrisPiece(pieceType);
            position = pos;
        }

        public ActivePiece(TetrisPiece piece, Position pos)
        {
            this.piece = piece;
            position = pos;
        }

        public Position[] getBlockPositions()
        {
            return piece.getCurrentRotation();
        }

        public Position[] getBlockPositions(Position afterMovingTo)
        {
            Position[] currentPosition = getBlockPositions();
            Position[] result = new Position[currentPosition.Length];

            for (int i = 0; i<result.Length; i++)
            {
                result[i] = afterMovingTo + currentPosition[i];
            }

            return result;
        }

        public void move(Position move)
        {
            position += move;
        }

        public Position[] peekNextRotationBlockPosition()
        {
            return piece.peekNextRotation();
        }

        public Position[] peekNextRotationBlockPosition(Position afterMovingTo)
        {
            Position[] currentPosition = peekNextRotationBlockPosition();
            Position[] result = new Position[currentPosition.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = afterMovingTo + currentPosition[i];
            }

            return result;
        }

        public void rotate()
        {
            piece.doNextRotation();
        }
    }
}
