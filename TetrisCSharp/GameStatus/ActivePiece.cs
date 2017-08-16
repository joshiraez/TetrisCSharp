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
            return sumToPositionArray(piece.getCurrentRotation(), position);
        }

        public Position[] getBlockPositions(Position afterMoving)
        {
            return sumToPositionArray(piece.getCurrentRotation(), position + afterMoving);
        }

        public TetrisPieceEnum getTetrisPieceType()
        {
            return piece.type;
        }

        public void move(Position move)
        {
            position += move;
        }


        public Position[] peekNextRotationBlockPosition()
        {
            return sumToPositionArray(piece.peekNextRotation(), position);
        }
        
        public Position[] peekNextRotationBlockPosition(Position afterMovingTo)
        {
            return sumToPositionArray(piece.peekNextRotation(), position+afterMovingTo);
        }

        public void rotate()
        {
            piece.doNextRotation();
        }

        private Position[] sumToPositionArray(Position[] currentPosition, Position placement)
        {
            for (int i = 0; i < currentPosition.Length; i++)
            {
                currentPosition[i] = placement + currentPosition[i];
            }

            return currentPosition;
        }
    }
}
