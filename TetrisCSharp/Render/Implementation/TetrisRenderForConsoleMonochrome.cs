using TetrisCSharp.GameStatus;
using TetrisCSharp.Output.Interface;
using TetrisCSharp.Output.Implementation;
using TetrisCSharp.Render.Interface;
using TetrisCSharp.GameStatus.TetrisPieces;
using System.Collections.Generic;

namespace TetrisCSharp.Render.Implementation
{
    public class TetrisRenderForConsoleMonochrome : ITetrisRender
    {
        Game game;
        IOutput outputMethod;
        char[][] gameRender;
        PieceRenderPosibilities[][] boardRender;

        private const int HEIGHT = 22;
        private const int WIDTH = 44;
        private const int BOARDRENDER_HEIGHT = 21;
        private const int BOARDRENDER_WIDTH = 21;

        private TetrisRenderForConsoleMonochrome() { }
        public TetrisRenderForConsoleMonochrome(Game game)
        {
            createGameRender();
            createBoardRender();
            outputMethod = new ConsolePrinter(gameRender);
            this.game = game;
        }

        private void createGameRender()
        {
            gameRender = (char[][])staticBackgroundRender.Clone();
        }

        private void createBoardRender()
        {
            boardRender = new PieceRenderPosibilities[BOARDRENDER_HEIGHT][];

            for(int row=0; row < BOARDRENDER_HEIGHT; row++)
            {
                boardRender[row] = new PieceRenderPosibilities[BOARDRENDER_WIDTH];
            }
        }

        public void output()
        {
            outputMethod.printImage();
        }

        public void render()
        {
            renderGameBoard();
            renderRightColumn();
        }

        private void renderGameBoard()
        {
            renderBoard();
            renderActivePiece();
            renderBoardToGameRender();
        }

        private void renderRightColumn()
        {
            renderNextPiece();
            renderScore();
            renderLevel();
        }

        private void renderBoard()
        {
            Position atPosition;
            for(byte row=0; row<game.board.getHeight(); row++)
            {
                for(byte column=0; column<game.board.getWidth(); column++)
                {
                    atPosition = new Position(row, column);
                    if (game.board.getBlock(atPosition).type == TetrisPieceEnum.EMPTY)
                        renderBlocks(atPosition);
                }
            }
        }

        private void renderActivePiece()
        {
            Position[] activePieceBlocks = game.movingPiece.getBlockPositions();
            for(byte block=0; block<activePieceBlocks.Length; block++)
            {
                renderBlocks(activePieceBlocks[block]);
            }
        }

        private void renderBoardToGameRender()
        {
            Position pivotPosition = new Position(0, 3);

            for(byte row = 0; row < BOARDRENDER_HEIGHT; row++)
            {
                for(byte column = 0; column < BOARDRENDER_WIDTH; column++)
                {
                    gameRender[pivotPosition.row + row][pivotPosition.column + column] = boardPiecesRender[boardRender[row][column]];
                }
            }
        }
   
        private enum PieceRenderPosibilities : byte
        {
            HORIZONTAL_ON_PIECE = 0x9,
            VERTICAL_ON_PIECE = 0x5,
            TOP_LEFT_CORNER_ALONEPIECE = 0x6, 
            TOP_RIGHT_CORNER_ALONEPIECE = 0xC,
            BOTTOM_LEFT_CORNER_ALONEPIECE = 0x3,
            BOTTOM_RIGHT_CORNER_ALONEPIECE = 0xA,
            TOP_BETWEEN2PIECES = 0xE,
            BOTTOM_BETWEEN2PIECES = 0xB,
            LEFT_BETWEEN2PIECES = 0x7,
            RIGHT_BETWEEN2PIECES = 0xD,
            MIDDLE_BETWEENLOTSPIECES= 0xF,
            EMPTY = 0x0
        }

        //To simplify this, I used a bit trick, defining numbers for the pieces based if they have an edge in clockwise motion. Then I do a bitwise or and it will give use the addition of edges.
        //We will use this to translate the board, then draw the result. Although it's costly (800~ operations each frame? Multiple checks in some squares? ugh) but it's way better for upkeeping and readibility.
        //In case is too slow, the old algorithm is at the bottom of the file.
        private static PieceRenderPosibilities add(PieceRenderPosibilities whatIsThere, PieceRenderPosibilities whatIsAdded)
        {
            return whatIsThere | whatIsAdded;
        }

        private void renderBlocks(Position originalBoardPosition)
        {
            if (originalBoardPosition.row >= 0)
            {
                boardRender[originalBoardPosition.row][originalBoardPosition.column * 2] = add(boardRender[originalBoardPosition.row][originalBoardPosition.column * 2], PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE);
                boardRender[originalBoardPosition.row][originalBoardPosition.column * 2 + 1] = add(boardRender[originalBoardPosition.row][originalBoardPosition.column * 2], PieceRenderPosibilities.HORIZONTAL_ON_PIECE);
                boardRender[originalBoardPosition.row][originalBoardPosition.column * 2 + 2] = add(boardRender[originalBoardPosition.row][originalBoardPosition.column * 2], PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE);
            }
            if (originalBoardPosition.row >= -1)
            {
                boardRender[originalBoardPosition.row + 1][originalBoardPosition.column * 2] = add(boardRender[originalBoardPosition.row][originalBoardPosition.column * 2], PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE);
                boardRender[originalBoardPosition.row + 1][originalBoardPosition.column * 2 + 1] = add(boardRender[originalBoardPosition.row][originalBoardPosition.column * 2], PieceRenderPosibilities.HORIZONTAL_ON_PIECE);
                boardRender[originalBoardPosition.row + 1][originalBoardPosition.column * 2 + 2] = add(boardRender[originalBoardPosition.row][originalBoardPosition.column * 2], PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE);
            }
        }

        private static readonly Dictionary<PieceRenderPosibilities, char> boardPiecesRender =
            new Dictionary<PieceRenderPosibilities, char>
            {
                { PieceRenderPosibilities.HORIZONTAL_ON_PIECE,              '─' },
                { PieceRenderPosibilities.VERTICAL_ON_PIECE,                '│' },
                { PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE,       '┌' },
                { PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE,      '┐' },
                { PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE,    '└' },
                { PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE,   '└' },
                { PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES,            '┬' },
                { PieceRenderPosibilities.TOP_BETWEEN2PIECES,               '┴' },
                { PieceRenderPosibilities.LEFT_BETWEEN2PIECES,              '├' },
                { PieceRenderPosibilities.RIGHT_BETWEEN2PIECES,             '┤' },
                { PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES,         '┼' },
                { PieceRenderPosibilities.EMPTY,                            ' ' }
            };

        private void renderStringToGameRender(Position pivot, char[] charArray)
        {
            renderImageToGameRender(pivot, new char[1][] { charArray });
        }

        private void renderStringToGameRender(Position pivot, string stringToRender)
        {
            renderImageToGameRender(pivot, new char[1][] { stringToRender.ToCharArray() } );
        }

        private void renderImageToGameRender(Position pivot, char[][] charMatrix)
        {
            for (byte row = 0; row < charMatrix.Length; row++)
            {
                for (byte column = 0; column < charMatrix[row].Length; column++)
                {
                    gameRender[pivot.row + row][pivot.column + column] = charMatrix[row][column];
                }
            }
        }

        private void renderNextPiece()
        {
            Position pivotPosition = new Position(2, 31);
            char[][] pieceToPrint = nextPiecesRender[game.nextPiece];

            renderImageToGameRender(pivotPosition, pieceToPrint);
        }

        private void renderScore()
        {
            Position pivotPosition = new Position(9, 31);
            char[] score = string.Format("D9", game.score).ToCharArray();

            renderStringToGameRender(pivotPosition, score );
        }

        private void renderLevel()
        {
            Position tensDigitPivotPosition = new Position(14, 31);
            Position unitsDigitPivotPosition = new Position(14, 35);
            Position nextLevelPivotPosition = new Position(17, 38);

            if (game.level >= 10)
            {
                renderImageToGameRender(tensDigitPivotPosition, bigNumberRender[(byte)(game.level / 10)]);
            }
            renderImageToGameRender(unitsDigitPivotPosition, bigNumberRender[(byte)(game.level % 10)]);
            renderStringToGameRender(nextLevelPivotPosition, game.toNextLevel.ToString());
        }



        private static readonly Dictionary<TetrisPieceEnum, char[][]> nextPiecesRender =
            new Dictionary<TetrisPieceEnum, char[][]>
            {
                {
                    TetrisPieceEnum.EMPTY,
                    new char[][] { new char[] { } }
                },
                {
                    TetrisPieceEnum.GARBAGE,
                    new char[][] { new char[] { } }
                },
                {
                    TetrisPieceEnum.I,
                    new char[][]
                    {
                        "┌─┬─┬─┬─┐".ToCharArray(),
                        "└─┴─┴─┴─┘".ToCharArray()
                    }
                },
                {
                    TetrisPieceEnum.J,
                    new char[][]
                    {
                        "┌─┐".ToCharArray(),
                        "├─┼─┬─┐".ToCharArray(),
                        "└─┴─┴─┘".ToCharArray()
                    }
                },
                {
                    TetrisPieceEnum.L,
                    new char[][]
                    {
                        "    ┌─┐".ToCharArray(),
                        "┌─┬─┼─┤".ToCharArray(),
                        "└─┴─┴─┘".ToCharArray()
                    }
                },
                {
                    TetrisPieceEnum.O,
                    new char[][]
                    {
                        "┌─┬─┐".ToCharArray(),
                        "├─┼─┤".ToCharArray(),
                        "└─┴─┘".ToCharArray()
                    }
                },
                { TetrisPieceEnum.S,
                    new char[][]
                    {
                        "  ┌─┬─┐".ToCharArray(),
                        "┌─┼─┼─┘".ToCharArray(),
                        "└─┴─┘".ToCharArray()
                    }
                },
                { TetrisPieceEnum.T,
                    new char[][]
                    {
                        "┌─┬─┬─┐".ToCharArray(),
                        "└─┼─┼─┘".ToCharArray(),
                        "  └─┘".ToCharArray()
                    }
                },
                { TetrisPieceEnum.Z,
                    new char[][]
                    {
                        "┌─┬─┐".ToCharArray(),
                        "└─┼─┼─┐".ToCharArray(),
                        "  └─┴─┘".ToCharArray()
                    }
                }
            };
        private static readonly Dictionary<byte, char[][]> bigNumberRender =
            new Dictionary<byte, char[][]>
            {
                {
                    0,
                    new char[][]
                    {
                        "╔═╗".ToCharArray(),
                        "║ ║".ToCharArray(),
                        "╚═╝".ToCharArray()
                    }
                },
                {
                    1,
                    new char[][]
                    {
                        "  ║".ToCharArray(),
                        "  ║".ToCharArray(),
                        "  ║".ToCharArray()
                    }
                },
                {
                    2,
                    new char[][]
                    {
                        "══╗".ToCharArray(),
                        "╔═╝".ToCharArray(),
                        "╚══".ToCharArray()
                    }
                },
                {
                    3,
                    new char[][]
                    {
                        "══╗".ToCharArray(),
                        " ═╣".ToCharArray(),
                        "══╝".ToCharArray()
                    }
                },
                {
                    4,
                    new char[][]
                    {
                        "║ ║".ToCharArray(),
                        "╚═╣".ToCharArray(),
                        "  ║".ToCharArray()
                    }
                },
                {
                    5,
                    new char[][]
                    {
                        "╔══".ToCharArray(),
                        "╚═╗".ToCharArray(),
                        "══╝".ToCharArray()
                    }
                },
                {
                    6,
                    new char[][]
                    {
                        "╔══".ToCharArray(),
                        "╠═╗".ToCharArray(),
                        "╚═╝".ToCharArray()
                    }
                },
                {
                    7,
                    new char[][]
                    {
                        "══╗".ToCharArray(),
                        "  ║".ToCharArray(),
                        "  ║".ToCharArray()
                    }
                },
                {
                    8,
                    new char[][]
                    {
                        "╔═╗".ToCharArray(),
                        "╠═╣".ToCharArray(),
                        "╚═╝".ToCharArray()
                    }
                },
                {
                    9,
                    new char[][]
                    {
                        "╔═╗".ToCharArray(),
                        "╚═╣".ToCharArray(),
                        "  ║".ToCharArray()
                    }
                }
            };

        //It's possible to do it with loops and only saving the pattern, but for such a small board, 
        //I think we can just use it as the baseline and afford the memory loss of saving it. It's very readable, also.
        public static char[][] staticBackgroundRender =
            new char[][]
            {
                "╔═╗                     ╔═╗▄ ▄ ▄ ▄ ▄ ▄ ▄ ▄ ▄".ToCharArray(),
                "║█║                     ║█║▄▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀  >>   NEXT  ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀  >>  SCORE  ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀       /     ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀  >>  LEVEL  ▀▄".ToCharArray(),
                "║█║                     ║█║▄▀             ▀▄".ToCharArray(),
                "╚▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀╝▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄".ToCharArray()
            };
    }
}

/*
  00000000001111111111222222222233333333334444
  01234567890123456789012345678901234567890123
00╔═╗┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐╔═╗▄ ▄ ▄ ▄ ▄ ▄ ▄ ▄ ▄
01║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀▄
02║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  ┌─┬─┬─┬─┐  ▀▄
03║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  └─┴─┴─┴─┘  ▀▄
04║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  └─┴─┴─┘    ▀▄
05║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  >>   NEXT  ▀▄
06║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀             ▀▄
07║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄  
08║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀             ▀▄
09║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  999999999  ▀▄
10║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  >>  SCORE  ▀▄
11║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀             ▀▄
12║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄
13║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀             ▀▄
14║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  ══╗ ╔═╗    ▀▄
15║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  ╔═╝ ║ ║    ▀▄
16║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  ╚══ ╚═╝    ▀▄ 
17║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀       / XX  ▀▄
18║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀             ▀▄
19║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  >>  LEVEL  ▀▄
20║█║└─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘║█║▄▀             ▀▄
21╚▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀╝▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄

*/
/* Old version of the renderBoard, I think is faster because it avoids drawing the 6-8 blocks for each block, wich gets insane: 200 blocks board with 6/8 operations = a lot,
 *  but the readibility and upkeep was going to be disastrous. I'm going to try using the add function and in case it is too slow I will use this. That's why I've put it at the end.
   public void renderBoard()
   {
       GameBoard board = game.board;
       bool pieceOnPlace;
       bool pieceOnPreviousPlace;
       bool pieceOnPreviousRowSamePlace;
       bool pieceOnPreviousRowPreviousPlace;
       byte row;
       byte column;

       //First row
       row = 0;
       column = 0;

       pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
       if (pieceOnPlace)
       {
           gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
           gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
       }else
       {
           gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
       }

       for(column=1; column<game.board.getWidth()-1; column++)
       {
           pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column-1))).type != TetrisPieceEnum.EMPTY;

           if (pieceOnPlace)
           {
               if (pieceOnPreviousPlace)
               {
                   gameRender[row + boardPivotPosition.row][column*2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];

               }else
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
               }

               gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           }
           else
           {
               if (pieceOnPreviousPlace)
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];

               }
               else
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               }

               gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           }
       }

       pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
       pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column-1))).type != TetrisPieceEnum.EMPTY;

       if (pieceOnPlace)
       {
           if (pieceOnPreviousPlace)
           {
               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];

           }
           else
           {
               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
           }

           gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
       }
       else
       {
           if (pieceOnPreviousPlace)
           {
               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];

           }
           else
           {
               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           }

           gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
       }

       //Middle rows
       for (row = 1; row < game.board.getHeight() - 1; row++)
       {
           column = 0;

           pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousRowSamePlace = board.getBlock(new Position((byte)(row - 1), column)).type != TetrisPieceEnum.EMPTY;

           if (pieceOnPlace)
           {
               if (pieceOnPreviousRowSamePlace)
               {
                   gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
               }
               else
               {
                   gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
               }
               gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           }
           else
           {
               if (pieceOnPreviousRowSamePlace)
               {
                   gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                   gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
               }
               else
               {
                   gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                   gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               }
           }

           for (column = 1; column < game.board.getWidth() - 1; column++)
           {
               pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
               pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;
               pieceOnPreviousRowSamePlace = board.getBlock(new Position((byte)(row - 1), column)).type != TetrisPieceEnum.EMPTY;
               pieceOnPreviousRowPreviousPlace = board.getBlock(new Position((byte)(row - 1), (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;

               if (pieceOnPlace)
               {
                   if (pieceOnPreviousRowPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                   }
                   else
                   {
                       if (pieceOnPreviousRowSamePlace)
                       {
                           if (pieceOnPreviousPlace)
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                           }
                           else
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                           }
                       }else
                       {
                           if (pieceOnPreviousPlace)
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                           }
                           else
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                           }
                       }
                   }

                   gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
               }
               else
               {
                   if (pieceOnPreviousRowSamePlace)
                   {
                       if (pieceOnPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                       }
                       else
                       {
                           if (pieceOnPreviousRowPreviousPlace)
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                           }
                           else
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                           }
                       }
                       gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                   }
                   else
                   {
                       if (pieceOnPreviousPlace)
                       {
                           if (pieceOnPreviousRowPreviousPlace)
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                           }
                           else
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                           }
                       }
                       else
                       {
                           if (pieceOnPreviousRowPreviousPlace)
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                           }
                           else
                           {
                               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                           }
                       }
                       gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                   }
               }

           }

           pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousRowSamePlace = board.getBlock(new Position((byte)(row - 1), column)).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousRowPreviousPlace = board.getBlock(new Position((byte)(row - 1), (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;

           if (pieceOnPlace)
           {
               if (pieceOnPreviousRowPreviousPlace)
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];

                   if (pieceOnPreviousRowSamePlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                   }
                   else
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                   }
               }
               else
               {
                   if (pieceOnPreviousRowSamePlace)
                   {
                       if (pieceOnPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                       }
                       gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                   }
                   else
                   {
                       if (pieceOnPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                       }
                       gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                   }

               }
               gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           }
           else
           {
               if (pieceOnPreviousRowSamePlace)
               {
                   if (pieceOnPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                   }
                   else
                   {
                       if (pieceOnPreviousRowPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                       }
                   }
                   gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                   gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
               }
               else
               {
                   if (pieceOnPreviousPlace)
                   {
                       if (pieceOnPreviousRowPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                       }
                   }
                   else
                   {
                       if (pieceOnPreviousRowPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                       }
                   }
                   gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                   gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               }
           }
       }

       //Last row
       column = 0;

       pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
       pieceOnPreviousRowSamePlace = board.getBlock(new Position((byte)(row - 1), column)).type != TetrisPieceEnum.EMPTY;

       if (pieceOnPlace)
       {
           if (pieceOnPreviousRowSamePlace)
           {
               gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
           }
           else
           {
               gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
           }
           gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           gameRender[row + 1 + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
           gameRender[row + 1 + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
       }
       else
       {
           if (pieceOnPreviousRowSamePlace)
           {
               gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
               gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           }
           else
           {
               gameRender[row + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               gameRender[row + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           }

           gameRender[row + 1 + boardPivotPosition.row][column + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           gameRender[row + 1 + boardPivotPosition.row][column + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
       }

       for (column = 1; column < game.board.getWidth() - 1; column++)
       {
           pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousRowSamePlace = board.getBlock(new Position((byte)(row - 1), column)).type != TetrisPieceEnum.EMPTY;
           pieceOnPreviousRowPreviousPlace = board.getBlock(new Position((byte)(row - 1), (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;

           if (pieceOnPlace)
           {
               if (pieceOnPreviousRowPreviousPlace)
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                   if (pieceOnPreviousPlace)
                   {
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                   }
                   else
                   {
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                   }
               }
               else
               {
                   if (pieceOnPreviousRowSamePlace)
                   {
                       if (pieceOnPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                           gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                           gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                       }
                   }
                   else
                   {
                       if (pieceOnPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                           gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                           gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                       }
                   }
               }

               gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
               gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           } 
           else
           {
               if (pieceOnPreviousRowSamePlace)
               {
                   if (pieceOnPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];

                   }
                   else
                   {
                       if (pieceOnPreviousRowPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                       }
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                   }
                   gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
               }
               else
               {
                   if (pieceOnPreviousPlace)
                   {
                       if (pieceOnPreviousRowPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                       }
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                   }
                   else
                   {
                       if (pieceOnPreviousRowPreviousPlace)
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                       }
                       else
                       {
                           gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                       }
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                   }
                   gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];

               }
               gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           }

       }

       pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
       pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;
       pieceOnPreviousRowSamePlace = board.getBlock(new Position((byte)(row - 1), column)).type != TetrisPieceEnum.EMPTY;
       pieceOnPreviousRowPreviousPlace = board.getBlock(new Position((byte)(row - 1), (byte)(column - 1))).type != TetrisPieceEnum.EMPTY;

       if (pieceOnPlace)
       {
           if (pieceOnPreviousRowPreviousPlace)
           {
               gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];

               if (pieceOnPreviousPlace)
               {
                   gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
               }
               else
               {
                   gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
               }

               if (pieceOnPreviousRowSamePlace)
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
               }
               else
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
               }
           }
           else
           {
               if (pieceOnPreviousRowSamePlace)
               {
                   if (pieceOnPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                   }
                   else
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                   }
                   gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
               }
               else
               {
                   if (pieceOnPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                   }
                   else
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                       gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                   }
                   gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
               }
           }

           gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
           gameRender[row + 1 + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
       }
       else
       {
           if (pieceOnPreviousRowSamePlace)
           {
               if (pieceOnPreviousPlace)
               {
                   gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                   gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];

               }
               else
               {
                   if (pieceOnPreviousRowPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                   }
                   else
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                   }
                   gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               }
               gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
               gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
           }
           else
           {
               if (pieceOnPreviousPlace)
               {
                   if (pieceOnPreviousRowPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                   }
                   else
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                   }
                   gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
               }
               else
               {
                   if (pieceOnPreviousRowPreviousPlace)
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                   }
                   else
                   {
                       gameRender[row + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                   }
                   gameRender[row + 1 + boardPivotPosition.row][column * 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               }
               gameRender[row + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
               gameRender[row + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];

           }
           gameRender[row + 1 + boardPivotPosition.row][column * 2 + 1 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
           gameRender[row + 1 + boardPivotPosition.row][column * 2 + 2 + boardPivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
       }

       //Incoming debugging madness :( I think it's very hard to optimize block reads and readability in this one. I went with optimization, I don't want the game to feel laggy.
   }*/
