﻿using System;
using TetrisCSharp.GameLogic.Interface;
using TetrisCSharp.GameStatus;
using TetrisCSharp.GameStatus.TetrisPieces;
using TetrisCSharp.Input.Implementation;
using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.GameLogic.Implementation
{
    public class BasicTetrisLogic : ITetrisGameLogic
    {
        private Game game;
        private ITetrisControl controller;

        private bool gameOverFlag;
        private Random rng;

        private static readonly byte[] DEFAULT_LINES_FOR_LEVEL = { 0, 5, 10, 10, 20, 25, 30, 35, 40, 40, 50, 50, 50, 50, 50, 50, 60, 70, 80, 90, 100 };
        private static readonly int[] DEFAULT_SCORES_FOR_LINES_CLEARED = { 0, 40, 100, 300, 1200 };
        private const int SCORE_FOR_DROP = 1;
        private const int MULTIPLIER_SCORE_FOR_INSTAFALL = 2;
        private const int MAX_SCORE = 999999999;

        private const long BASE_MILLISECONDS_TO_DROP = 1000;
        private const long MILLISECONDS_REDUCTION_BY_LEVEL = 40;
        private DateTime timeToNextDrop;


        private BasicTetrisLogic()
        {
            
        }

        public BasicTetrisLogic(Game game)
        {
            gameOverFlag = false;
            rng = new Random();

            this.game = game;
            controller = new KeyboardController();
        }

        public BasicTetrisLogic(Game game, ITetrisControl controller)
        {
            gameOverFlag = false;
            rng = new Random();

            this.game = game;
            this.controller = controller;
        }


        public bool HasFinished()
        {
            return gameOverFlag;
        }

        public void Start()
        {
            initializeGame();
            startDropTime();
        }

        public void Update()
        {
            bool pieceLanded;
            
            if (controller.isFirePressed())
            {
                rotatePiece();
            }

            if (controller.isRightPressed())
            {
                move(Position.right);
            }
            if (controller.isLeftPressed())
            {
                move(Position.left);
            }

            if (controller.isUpPressed())
            {
                byte linesDropped = instafall();
                pieceLanded = true;
                startDropTime();
                addScore(linesDropped * SCORE_FOR_DROP * 2); 

            }
            else
            {
                if (controller.isDownPressed())
                {
                    pieceLanded=dropSpace();
                    startDropTime();
                    addScore(SCORE_FOR_DROP);
                }
                else
                {
                    pieceLanded=checkDropTime();
                }
            }

            if (pieceLanded)
            {
                placePiece();
                checkLinesClear();
                spawnNewPiece();
            }

        }

        private void initializeGame()
        {
            gameOverFlag = false;
            game.movingPiece = new ActivePiece(randomPieceGenerator(), game.board.getSpawn());
            game.nextPiece = randomPieceGenerator();
            game.score = 0;
            game.level = 1;
            game.toNextLevel = DEFAULT_LINES_FOR_LEVEL[game.level];
        }
       

        private TetrisPieceEnum randomPieceGenerator()
        {
            return (TetrisPieceEnum)rng.Next(1, 8);
        }

        private void rotatePiece()
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

            while (wallKickingTry < wallkickingStrategy.Length && checkWillBeCollisionAfterRotation(wallkickingStrategy[wallKickingTry]))
            {
                wallKickingTry++;
            }

            if (wallKickingTry < wallkickingStrategy.Length)
            {
                game.movingPiece.move(wallkickingStrategy[wallKickingTry]);
                game.movingPiece.rotate();
            }              
        }

        private void move(Position moveDirection)
        {
            Position[] peekMoveBlocks = game.movingPiece.getBlockPositions(moveDirection);

            if(checkIfItsValidPosition(peekMoveBlocks)){
                game.movingPiece.move(moveDirection);
            }
        }

        private byte instafall()
        {
            byte lines = 0;
            while (!dropSpace()){
                lines++;
            }
            return lines;
        }

        private bool dropSpace()
        {
            bool pieceLanded;

            pieceLanded = checkWillBeCollision(Position.down);

            if (!pieceLanded)
            {
                game.movingPiece.move(Position.down);
            }

            return pieceLanded;
        }

        private bool checkWillBeCollisionAfterRotation(Position afterMoving)
        {
            Position[] peekRotationBlocks = game.movingPiece.peekNextRotationBlockPosition(afterMoving);
            return !checkIfItsValidPosition(peekRotationBlocks);
        }

        private bool checkWillBeCollision(Position afterMoving)
        {
            Position[] peekBlocks = game.movingPiece.getBlockPositions(afterMoving);
            return !checkIfItsValidPosition(peekBlocks);
        }

        private bool checkIfItsValidPosition(Position[] blockPositions)
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

        private void startDropTime()
        {
            long milisecondsTimeToNextDrop = BASE_MILLISECONDS_TO_DROP - MILLISECONDS_REDUCTION_BY_LEVEL * game.level;
            timeToNextDrop = DateTime.Now + new TimeSpan(milisecondsTimeToNextDrop * TimeSpan.TicksPerMillisecond);
        }

        private bool checkDropTime()
        {
            if (timeToNextDrop < DateTime.Now) { 
                startDropTime();
                return dropSpace();
            }
            else
            {
                return false;
            }
            
        }

        private void placePiece()
        {
            Position[] blockPositions = game.movingPiece.getBlockPositions();

            for(int blockArrayIndex =0; blockArrayIndex<blockPositions.Length; blockArrayIndex++)
            {
                game.board.setBlock(blockPositions[blockArrayIndex], game.movingPiece.getTetrisPieceType());
            }
        }

        private void checkLinesClear()
        {
            byte linesCleared = game.board.clearLines();

            addScore(game.level * DEFAULT_SCORES_FOR_LINES_CLEARED[linesCleared]);

            if (game.level < DEFAULT_LINES_FOR_LEVEL.Length) {

                if(game.toNextLevel <= linesCleared)
                {
                    game.level++;
                    game.toNextLevel += DEFAULT_LINES_FOR_LEVEL[game.level];
                }
                game.toNextLevel -= linesCleared;
            }
        }

        private void spawnNewPiece()
        {
            game.movingPiece = new ActivePiece(game.nextPiece, game.board.getSpawn());

            game.nextPiece = randomPieceGenerator();

            gameOverFlag = checkWillBeCollision(new Position(0, 0));
        }

        private void addScore(int points)
        {
            game.score = Math.Min(game.score + points, MAX_SCORE);
        }
    }
}
