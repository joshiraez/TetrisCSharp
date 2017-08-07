namespace TetrisCSharp.GameStatus.TetrisPieces
{
    abstract public class TetrisPiece
    {
        Position[][] rotations;
        byte rotation;
        TetrisPieceEnum type;

        private byte getNextRotation()
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
