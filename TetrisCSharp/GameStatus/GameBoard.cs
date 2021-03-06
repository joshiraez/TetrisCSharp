﻿using TetrisCSharp.GameStatus.TetrisPieces;

namespace TetrisCSharp.GameStatus
{
    public class GameBoard
    {
        public static readonly Position SPAWN = new Position(0, 4);
        private const byte ROW_SIZE = 20;
        private const byte COL_SIZE = 10;
        private Block[][] board;
        

        public GameBoard()
        {
            board = new Block[ROW_SIZE][];

            byte rowIterator;
            
            for(rowIterator=0; rowIterator<ROW_SIZE; rowIterator++)
            {
                board[rowIterator] = new Block[COL_SIZE];
            }
        }

        public Position getSpawn()
        {
            return SPAWN;
        }

        public byte getHeight()
        {
            return ROW_SIZE;
        }

        public byte getWidth()
        {
            return COL_SIZE;
        }

        public bool isOnBoard(Position pos)
        {
            return pos.row >= 0 && pos.row < ROW_SIZE && pos.column >= 0 && pos.column < COL_SIZE;
        }

        public bool isBlockFree(Position pos)
        {
            //We don't care about something going over the board (there is no ceil), so if one piece wants to be over it, it can.
            if (pos.row < ROW_SIZE && pos.column >= 0 && pos.column < COL_SIZE)
            {
                return getBlock(pos).type == TetrisPieceEnum.EMPTY;
            }else
            {
                return false;
            }
        }

        public void setBlock(Position pos, TetrisPieceEnum type)
        {
            if(isOnBoard(pos))
                board[pos.row][pos.column] = new Block(type);
        }

        public Block getBlock(Position pos)
        {
            if (isOnBoard(pos))
            {
                return board[pos.row][pos.column];
            }else
            {
                return new Block(TetrisPieceEnum.EMPTY);
            }
        }

        public byte clearLines()
        {
            byte linesCleared = 0;
            sbyte rowIterator;
            byte colIterator;

            rowIterator = ROW_SIZE -1;

            while(rowIterator >= 0)
            {
                colIterator = 0;
                while(colIterator < COL_SIZE && getBlock(new Position(rowIterator, colIterator)).type != TetrisPieceEnum.EMPTY)
                {
                    colIterator++;
                }

                if(colIterator!= COL_SIZE)
                {
                    rowIterator--;
                }else
                {
                    moveRowsDown(rowIterator);
                    linesCleared++;
                }
            }
            
            return linesCleared;
        }

        public void addRow(Block[] newBlocks, sbyte row = ROW_SIZE -1)
        {
            if(row>=0 && row<ROW_SIZE)
                board[row] = newBlocks;
        }

        public void moveRowsDown(sbyte fromRow)
        {
            sbyte rowIterator;

            for(rowIterator=fromRow; rowIterator>0; rowIterator--)
            {
                board[rowIterator] = (Block[])board[rowIterator-1].Clone();
            }
        }

        public void moveRowsUp(sbyte fromRow = ROW_SIZE - 1)
        {
            sbyte rowIterator;

            //We lose one lane if the first row is full. Alas, is a prototype of tetris. We could make it with one extra row in top of it? :P
            for (rowIterator = 0; rowIterator <fromRow; rowIterator++)
            {
                board[rowIterator] = (Block[])board[rowIterator+1].Clone();
            }

            addRow(new Block[COL_SIZE], rowIterator);
        }


    }
}
