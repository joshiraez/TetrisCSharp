namespace TetrisCSharp.GameStatus.TetrisPieces
{
    public class TetrisPiece
    {
        private readonly Position[][] rotations;
        byte rotation;
        public TetrisPieceEnum type { get;  }

        public TetrisPiece(TetrisPieceEnum type)
        {
            rotation = 0;
            this.type = type;
            rotations = TetrisPieceRotationData.getRotations(type);
        }

        protected byte getNextRotation()
        {
            return (byte)((rotation + 1) % rotations.Length);
        }

        public Position[] getCurrentRotation()
        {
            //Returning a clone makes us be able to work directly with the function result
            return rotations[rotation].Clone() as Position[];
        }

        public Position[] peekNextRotation()
        {
            return rotations[getNextRotation()].Clone() as Position[]; 
        }

        public void doNextRotation()
        {
            rotation++;
        }

        
    }
}
