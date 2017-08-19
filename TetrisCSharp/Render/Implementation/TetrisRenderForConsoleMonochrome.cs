using TetrisCSharp.GameStatus;
using TetrisCSharp.Output.Interface;
using TetrisCSharp.Output.Implementation;
using TetrisCSharp.Render.Interface;

namespace TetrisCSharp.Render.Implementation
{
    public class TetrisRenderForConsoleMonochrome : ITetrisRender
    {
        Game game;
        IOutput outputMethod;
        char[][] gameRender;

        private const int HEIGHT = 60;
        private const int WIDTH = 60;

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

        public void render()
        {
            renderGameBoard();
            renderRightColumn();
        }

        public void output()
        {
            outputMethod.printImage();
        }


    }
}
