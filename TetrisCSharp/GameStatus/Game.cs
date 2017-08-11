using TetrisCSharp.GameStatus.TetrisPieces;

namespace TetrisCSharp.GameStatus
{
    public class Game
    {

        public GameBoard board { get; } 
        public ActivePiece movingPiece { get; set; }
        public TetrisPieceEnum nextPiece { get; set; }
        public int score { get; set; }
        public byte level { get; set; }
        public byte toNextLevel { get; set; }

        public Game()
        {
            board = new GameBoard();

        }
    }
}
