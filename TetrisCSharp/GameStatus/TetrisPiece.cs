namespace TetrisCSharp.GameStatus
{
    abstract class TetrisPiece
    {
        Position[][] rotations { get; }
        byte rotation;

        private byte getNextRotation()
        {
            return (byte)((rotation + 1) % rotations.Length);
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
