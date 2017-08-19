using System;
using TetrisCSharp.Output.Interface;

namespace TetrisCSharp.Output.Implementation
{
    public class ConsolePrinter : IOutput
    {
        private char[][] gameImage;

        private ConsolePrinter() { }

        public ConsolePrinter(char[][] gameImage)
        {
            this.gameImage = gameImage;
        }

        public void printImage()
        {
            Console.Clear();
            for(int row=0; row<gameImage.Length; row++)
            {
                for(int column=0; column<gameImage[0].Length; column++)
                {
                    printChar(row, column, gameImage[row][column]);
                }
            }
        }

        private void printChar(int row, int column, char toPrint)
        {
            Console.SetCursorPosition(row, column);
            Console.Write(toPrint);
        }
    }

}
