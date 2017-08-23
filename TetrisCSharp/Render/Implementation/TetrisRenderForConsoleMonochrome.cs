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

        private const int HEIGHT = 22;
        private const int WIDTH = 44;

        private TetrisRenderForConsoleMonochrome() { }
        public TetrisRenderForConsoleMonochrome(Game game)
        {
            createGameRender();
            outputMethod = new ConsolePrinter(gameRender);
            this.game = game;
        }

        private void createGameRender()
        {
            gameRender = (char[][])staticBackgroundRender.Clone();
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

        public void renderGameBoard()
        {
            renderBoard();
            renderActivePiece();
        }

        public void renderRightColumn()
        {
            renderNextPiece();
            renderScore();
            renderLevel();
        }

        public void renderBoard()
        {
            Position pivotPosition = new Position(0, 3);
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
                gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
            }else
            {
                gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
            }
            
            for(column=1; column<game.board.getWidth()-1; column++)
            {
                pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
                pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column-1))).type != TetrisPieceEnum.EMPTY;

                if (pieceOnPlace)
                {
                    if (pieceOnPreviousPlace)
                    {
                        gameRender[row + pivotPosition.row][column*2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];

                    }else
                    {
                        gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                    }

                    gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                }
                else
                {
                    if (pieceOnPreviousPlace)
                    {
                        gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];

                    }
                    else
                    {
                        gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                    }

                    gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                }
            }

            pieceOnPlace = board.getBlock(new Position(row, column)).type != TetrisPieceEnum.EMPTY;
            pieceOnPreviousPlace = board.getBlock(new Position(row, (byte)(column-1))).type != TetrisPieceEnum.EMPTY;

            if (pieceOnPlace)
            {
                if (pieceOnPreviousPlace)
                {
                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];

                }
                else
                {
                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                }

                gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
            }
            else
            {
                if (pieceOnPreviousPlace)
                {
                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];

                }
                else
                {
                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                }

                gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
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
                        gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                    }
                    else
                    {
                        gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                    }
                    gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                }
                else
                {
                    if (pieceOnPreviousRowSamePlace)
                    {
                        gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                        gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                    }
                    else
                    {
                        gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                        gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
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
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                        }
                        else
                        {
                            if (pieceOnPreviousRowSamePlace)
                            {
                                if (pieceOnPreviousPlace)
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                                }
                                else
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                                }
                            }else
                            {
                                if (pieceOnPreviousPlace)
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                                }
                                else
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                                }
                            }
                        }

                        gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                    }
                    else
                    {
                        if (pieceOnPreviousRowSamePlace)
                        {
                            if (pieceOnPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                            }
                            else
                            {
                                if (pieceOnPreviousRowPreviousPlace)
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                                }
                                else
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                                }
                            }
                            gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                        }
                        else
                        {
                            if (pieceOnPreviousPlace)
                            {
                                if (pieceOnPreviousRowPreviousPlace)
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                                }
                                else
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                                }
                            }
                            else
                            {
                                if (pieceOnPreviousRowPreviousPlace)
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                                }
                                else
                                {
                                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                                }
                            }
                            gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
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
                        gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];

                        if (pieceOnPreviousRowSamePlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                        }
                        else
                        {
                            gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                        }
                    }
                    else
                    {
                        if (pieceOnPreviousRowSamePlace)
                        {
                            if (pieceOnPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                            }
                            gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                        }
                        else
                        {
                            if (pieceOnPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                            }
                            gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                        }
                        
                    }
                    gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                }
                else
                {
                    if (pieceOnPreviousRowSamePlace)
                    {
                        if (pieceOnPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                        }
                        else
                        {
                            if (pieceOnPreviousRowPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                            }
                        }
                        gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                        gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                    }
                    else
                    {
                        if (pieceOnPreviousPlace)
                        {
                            if (pieceOnPreviousRowPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                            }
                        }
                        else
                        {
                            if (pieceOnPreviousRowPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                            }
                        }
                        gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                        gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
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
                    gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                }
                else
                {
                    gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                }
                gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                gameRender[row + 1 + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                gameRender[row + 1 + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
            }
            else
            {
                if (pieceOnPreviousRowSamePlace)
                {
                    gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                    gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                }
                else
                {
                    gameRender[row + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                    gameRender[row + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                }

                gameRender[row + 1 + pivotPosition.row][column + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                gameRender[row + 1 + pivotPosition.row][column + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
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
                        gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                        if (pieceOnPreviousPlace)
                        {
                            gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                        }
                        else
                        {
                            gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                        }
                    }
                    else
                    {
                        if (pieceOnPreviousRowSamePlace)
                        {
                            if (pieceOnPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                                gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                                gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                            }
                        }
                        else
                        {
                            if (pieceOnPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                                gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                                gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                            }
                        }
                    }

                    gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                    gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                } 
                else
                {
                    if (pieceOnPreviousRowSamePlace)
                    {
                        if (pieceOnPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];

                        }
                        else
                        {
                            if (pieceOnPreviousRowPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                            }
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                        }
                        gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                    }
                    else
                    {
                        if (pieceOnPreviousPlace)
                        {
                            if (pieceOnPreviousRowPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                            }
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                        }
                        else
                        {
                            if (pieceOnPreviousRowPreviousPlace)
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                            }
                            else
                            {
                                gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                            }
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                        }
                        gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                        
                    }
                    gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
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
                    gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];

                    if (pieceOnPreviousPlace)
                    {
                        gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                    }
                    else
                    {
                        gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                    }

                    if (pieceOnPreviousRowSamePlace)
                    {
                        gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                    }
                    else
                    {
                        gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                    }
                }
                else
                {
                    if (pieceOnPreviousRowSamePlace)
                    {
                        if (pieceOnPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                        }
                        else
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.LEFT_BETWEEN2PIECES];
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                        }
                        gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                    }
                    else
                    {
                        if (pieceOnPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_BETWEEN2PIECES];
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                        }
                        else
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_LEFT_CORNER_ALONEPIECE];
                            gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                        }
                        gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                    }
                }

                gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                gameRender[row + 1 + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
            }
            else
            {
                if (pieceOnPreviousRowSamePlace)
                {
                    if (pieceOnPreviousPlace)
                    {
                        gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES];
                        gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];

                    }
                    else
                    {
                        if (pieceOnPreviousRowPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_BETWEEN2PIECES];
                        }
                        else
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_LEFT_CORNER_ALONEPIECE];
                        }
                        gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                    }
                    gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.HORIZONTAL_ON_PIECE];
                    gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                }
                else
                {
                    if (pieceOnPreviousPlace)
                    {
                        if (pieceOnPreviousRowPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.RIGHT_BETWEEN2PIECES];
                        }
                        else
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.TOP_RIGHT_CORNER_ALONEPIECE];
                        }
                        gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                    }
                    else
                    {
                        if (pieceOnPreviousRowPreviousPlace)
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.BOTTOM_RIGHT_CORNER_ALONEPIECE];
                        }
                        else
                        {
                            gameRender[row + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                        }
                        gameRender[row + 1 + pivotPosition.row][column * 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                    }
                    gameRender[row + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                    gameRender[row + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];

                }
                gameRender[row + 1 + pivotPosition.row][column * 2 + 1 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
                gameRender[row + 1 + pivotPosition.row][column * 2 + 2 + pivotPosition.column] = boardPiecesRender[PieceRenderPosibilities.EMPTY];
            }
            
            //Incoming debugging madness :( I think it's very hard to optimize block reads and readability in this one. I went with the optimization, I don't want the game to feel laggy.
        }

        private enum PieceRenderPosibilities
        {
            HORIZONTAL_ON_PIECE,
            VERTICAL_ON_PIECE,
            TOP_LEFT_CORNER_ALONEPIECE,
            TOP_RIGHT_CORNER_ALONEPIECE,
            BOTTOM_LEFT_CORNER_ALONEPIECE,
            BOTTOM_RIGHT_CORNER_ALONEPIECE,
            TOP_BETWEEN2PIECES,
            BOTTOM_BETWEEN2PIECES,
            LEFT_BETWEEN2PIECES,
            RIGHT_BETWEEN2PIECES,
            MIDDLE_BETWEENLOTSPIECES,
            EMPTY
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
                { PieceRenderPosibilities.TOP_BETWEEN2PIECES,               '┬' },
                { PieceRenderPosibilities.TOP_BETWEEN2PIECES,               '┴' },
                { PieceRenderPosibilities.LEFT_BETWEEN2PIECES,              '├' },
                { PieceRenderPosibilities.RIGHT_BETWEEN2PIECES,             '┤' },
                { PieceRenderPosibilities.MIDDLE_BETWEENLOTSPIECES,         '┼' },
                { PieceRenderPosibilities.EMPTY,                            ' ' }
            };

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