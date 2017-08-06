
namespace TetrisCSharp.GameStatus
{
    public struct Block
    {
        public TetrisPieceEnum type { get;  }

        public Block(TetrisPieceEnum procedence)
        {
            this.type = procedence;
        }
    }
}
