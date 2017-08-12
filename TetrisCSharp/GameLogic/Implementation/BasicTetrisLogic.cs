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
        private DateTime lastUpdateTime;
        private TimeSpan timeFromLastDrop;

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
            initializeTime();
        }

        public void Update(Game game, ITetrisControl controller)
        {
            bool piecePlaced;
            
            if (controller.isFirePressed())
            {
                rotatePiece(game);
            }

            if (controller.isRightPressed())
            {
                move(game, Position.right);
            }
            if (controller.isLeftPressed())
            {
                move(game, Position.left);
            }

            if (controller.isUpPressed())
            {
                instafall(game);
                piecePlaced = true;
            }
            else
            {
                if (controller.isDownPressed())
                {
                    dropSpace(game, piecePlaced);
                    
                }
                else
                {
                    checkDropTime(game, piecePlaced);
                }
            }

            if (piecePlaced)
            {
                checkLines();
                spawnNewPiece();
            }

        }

        private void initializeGame(Game game)
        {
            game.movingPiece = new ActivePiece(randomPieceGenerator(), game.board.getSpawn());
            game.nextPiece = randomPieceGenerator();
            game.score = 0;
            game.level = 1;
            game.toNextLevel = DEFAULT_LINES_FOR_LEVEL[game.level];
        }
        
        private void initializeTime()
        {
            lastUpdateTime = DateTime.Now;
            timeFromLastDrop = TimeSpan.Zero;
        }

        private TetrisPieceEnum randomPieceGenerator()
        {
            return (TetrisPieceEnum)rng.Next(1, 7);
        }

        private void rotatePiece(Game game)
        {
            Position[] wallkickingStrategy =
            {
                new Position(0,0),
                new Position(0,1),
                new Position(0,-1),
                new Position(0,2),
                new Position(0,-2)
            };

            byte wallKickingTry = 0;

            Position[] peekRotationBlocks = game.movingPiece.peekNextRotationBlockPosition(wallkickingStrategy[wallKickingTry]);

            while (!checkIfItsValidPosition(game, peekRotationBlocks) && wallKickingTry<wallkickingStrategy.Length)
            {
                wallKickingTry++;
                peekRotationBlocks = game.movingPiece.peekNextRotationBlockPosition(wallkickingStrategy[wallKickingTry]);
            }

            if (wallKickingTry < wallkickingStrategy.Length)
            {
                game.movingPiece.move(wallkickingStrategy[wallKickingTry]);
                game.movingPiece.rotate();
            }
           
            
        }

        private void move(Game game, Position moveDirection)
        {
            Position[] peekMoveBlocks = game.movingPiece.getBlockPositions(moveDirection);

            if(checkIfItsValidPosition(game, peekMoveBlocks){
                game.movingPiece.move(moveDirection);
            }
        }



        private bool checkIfItsValidPosition(Game game, Position[] blockPositions)
        {
            for(int block=0; block<blockPositions.Length; block++)
            {
                if (!game.board.isBlockFree(blockPositions[block]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
