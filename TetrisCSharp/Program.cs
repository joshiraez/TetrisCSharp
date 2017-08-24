using System;
using TetrisCSharp.GameLogic.Implementation;
using TetrisCSharp.GameLogic.Interface;
using TetrisCSharp.GameStatus;
using TetrisCSharp.Render.Implementation;
using TetrisCSharp.Render.Interface;

namespace TetrisCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Game gameState = new Game();
            ITetrisGameLogic gameLogic = new BasicTetrisLogic(gameState);
            ITetrisRender gameRender = new TetrisRenderForConsoleMonochrome(gameState);

            gameLogic.Start();
            gameRender.render();
            gameRender.output();
            while (!gameLogic.HasFinished())
            {
                gameLogic.Update();
                gameRender.render();
                gameRender.output();
            }
            Console.ReadLine();

        }
    }
}
