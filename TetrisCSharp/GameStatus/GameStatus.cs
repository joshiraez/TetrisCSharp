
namespace TetrisCSharp.GameStatus
{
    public class GameStatus
    {

        public GameBoard board { get; set; } 
        public ActivePiece movingPiece { get; set; }
        public TetrisPieceEnum nextPiece { get; set; }
        public int score { get; set; }
        public byte level { get; set; }
        public byte toNextLevel { get; set; }

        //Constructor todo
    }
}
