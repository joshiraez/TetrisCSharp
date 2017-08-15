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
        private const int MULTIPLIER_SCORE_FOR_INSTAFALL = 2;
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
            bool pieceLanded;
            
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
                pieceLanded = true;
            }
            else
            {
                if (controller.isDownPressed())
                {
                    pieceLanded=dropSpace(game);
                    
                }
                else
                {
                    pieceLanded=checkDropTime(game);
                }
            }

            if (pieceLanded)
            {
                placePiece();
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
                Position.right,
                Position.left,
                Position.right*2,
                Position.left*2
            };

            byte wallKickingTry = 0;

            while (!willBeCollisionAfterRotation(game, wallkickingStrategy[wallKickingTry]) && wallKickingTry<wallkickingStrategy.Length)
            {
                wallKickingTry++;
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

            if(checkIfItsValidPosition(game, peekMoveBlocks)){
                game.movingPiece.move(moveDirection);
            }
        }

        private void instafall(Game game)
        {
            while (!dropSpace(game)){}
        }

        private bool dropSpace(Game game)
        {
            bool pieceLanded;

            pieceLanded = willBeCollision(game, Position.down);

            if (!pieceLanded)
            {
                game.movingPiece.move(Position.down);
            }

            return pieceLanded;
        }

        private bool willBeCollisionAfterRotation(Game game, Position afterMoving)
        {
            Position[] peekRotationBlocks = game.movingPiece.peekNextRotationBlockPosition(afterMoving);
            return checkIfItsValidPosition(game, peekRotationBlocks);
        }

        private bool willBeCollision(Game game, Position afterMoving)
        {
            Position[] peekBlocks = game.movingPiece.getBlockPositions(afterMoving);
            return checkIfItsValidPosition(game, peekBlocks);
        }

        private bool checkIfItsValidPosition(Game game, Position[] blockPositions)
        {
            for (int block = 0; block < blockPositions.Length; block++)
            {
                if (!game.board.isBlockFree(blockPositions[block]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool checkDropTime(Game game)
        {

        }
    }
}
