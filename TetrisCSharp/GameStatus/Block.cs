
namespace TetrisCSharp.GameStatus
{
    public struct Block
    {
        public TetrisPieceEnum procedence { get; private set; }

        public Block(TetrisPieceEnum procedence)
        {
            this.procedence = procedence;
        }
    }
}
