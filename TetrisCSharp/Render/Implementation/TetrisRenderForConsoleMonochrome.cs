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
            gameRender = new char[HEIGHT][];

            for(int row=0; row<HEIGHT; row++)
            {
                gameRender[row] = new char[WIDTH];
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

        public void renderGameBoard()
        {
            renderWalls();
            renderBoard();
            renderActivePiece();
        }

        public void renderRightColumn()
        {
            renderBorder();
            renderNextPiece();
            renderScore();
            renderLevel();
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
09║█║┌─┼─┼─┼─┼─┼─┼─┼─┼─┼─┐║█║▄▀  99999999   ▀▄
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