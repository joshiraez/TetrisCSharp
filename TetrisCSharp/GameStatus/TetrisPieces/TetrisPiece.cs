namespace TetrisCSharp.GameStatus.TetrisPieces
{
    public class TetrisPiece
    {
        private readonly Position[][] rotations;
        byte rotation;
        public TetrisPieceEnum type { get;  }

        protected TetrisPiece(TetrisPieceEnum type)
        {
            rotation = 0;
        }

        protected byte getNextRotation()
        {
            return (byte)((rotation + 1) % rotations.Length);
        }

        public Position[] getCurrentRotation()
        {
            return rotations[rotation];
        }

        public Position[] peekNextRotation()
        {
            return rotations[getNextRotation()];
        }

        public void doNextRotation()
        {
            rotation++;
        }

        
    }
}
