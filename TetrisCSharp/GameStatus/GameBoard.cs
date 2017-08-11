using TetrisCSharp.GameStatus.TetrisPieces;

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

        public bool isBlockFree(Position pos)
        {
            return board[pos.row][pos.column].type == TetrisPieceEnum.EMPTY;
        }

        public void setBlock(Position pos, TetrisPieceEnum type)
        {
            board[pos.row][pos.column] = new Block(type);
        }

        public Block getBlock(Position pos)
        {
            return board[pos.row][pos.column];
        }

        public byte clearLines()
        {
            byte linesCleared = 0;
            byte rowIterator;
            byte colIterator;

            rowIterator = ROW_SIZE -1;

            while(rowIterator >= 0)
            {
                colIterator = 0;
                while(colIterator < COL_SIZE && getBlock(new Position(rowIterator, colIterator)).type != TetrisPieceEnum.EMPTY)
                {
                    colIterator++;
                }

                if(colIterator== COL_SIZE)
                {
                    rowIterator--;
                }else
                {
                    moveDown(rowIterator);
                    linesCleared++;
                }
            }
            
            return linesCleared;
        }

        public void addRow(Block[] newBlocks, byte row = ROW_SIZE -1)
        {
            board[row] = newBlocks;
        }

        public void moveDown(byte fromRow)
        {
            byte rowIterator;

            for(rowIterator=fromRow; rowIterator>0; rowIterator--)
            {
                board[rowIterator] = board[rowIterator-1];
            }
        }

        public void moveUp(byte fromRow = ROW_SIZE - 1)
        {
            byte rowIterator;

            //We lose one lane if the first row is full. Alas, is a prototype of tetris. We could make it with one extra row in top of it? :P
            for (rowIterator = 0; rowIterator <fromRow; rowIterator++)
            {
                board[rowIterator] = board[rowIterator+1];
            }

            this.addRow(new Block[COL_SIZE], rowIterator);
        }

        public Position getSpawn()
        {
            return SPAWN;
        }
    }
}
