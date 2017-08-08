using TetrisCSharp.GameStatus.TetrisPieces;

namespace TetrisCSharp.GameStatus
{
    public class GameStatus
    {

        public GameBoard board { get; } 
        public ActivePiece movingPiece { get; set; }
        public TetrisPieceEnum nextPiece { get; set; }
        public int score { get; set; }
        public byte level { get; set; }
        public byte toNextLevel { get; set; }

        public GameStatus()
        {
            board = new GameBoard();
            movingPiece = new ActivePiece(TetrisPieceEnum.EMPTY, new Position(0,0));
            nextPiece = TetrisPieceEnum.EMPTY;
            score = 0;
            level = 1;
            toNextLevel = 99;            
        }
    }
}
