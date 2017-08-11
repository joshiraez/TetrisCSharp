using System;
using TetrisCSharp.GameLogic.Interface;
using TetrisCSharp.GameStatus;
using TetrisCSharp.GameStatus.TetrisPieces;
using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.GameLogic.Implementation
{
    public class BasicTetrisLogic : ITetrisGameLogic
    {
        private bool gameOverFlag;
        private Random rng;
        private static readonly byte[] DEFAULT_LINES_FOR_LEVEL = { 0, 5, 10, 20, 40, 60, 80, 100, 120, 140, 160 };
        private static readonly int[] DEFAULT_SCORES_FOR_LINES_CLEARED = { 40, 100, 300, 1200 };
        private const int SCORE_FOR_DROP = 1;


        public BasicTetrisLogic()
        {
            gameOverFlag = false;
            rng = new Random();
        }

        public bool HasFinished(Game game)
        {
            return gameOverFlag;
        }

        public void Start(Game game)
        {
            game = new Game();
            initializeGame(game);
        }

        public void Update(Game game, ITetrisControl controller)
        {
            //todo
            throw new NotImplementedException();
        }

        private void initializeGame(Game game)
        {
            game.movingPiece = new ActivePiece(randomPieceGenerator(), game.board.getSpawn());
            game.nextPiece = randomPieceGenerator();
            game.score = 0;
            game.level = 1;
            game.toNextLevel = DEFAULT_LINES_FOR_LEVEL[game.level];
        }

        private TetrisPieceEnum randomPieceGenerator()
        {
            return (TetrisPieceEnum)rng.Next(1, 7);
        }
    }
}
