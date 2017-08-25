using System;
using System.Security;
using TetrisCSharp.Output.Interface;

namespace TetrisCSharp.Output.Implementation
{
    public class ConsolePrinter : IOutput
    {
        private char[][] gameImage;
        private char[][] previousImage;

        private ConsolePrinter() { }

        public ConsolePrinter(char[][] gameImage)
        {
            this.gameImage = gameImage;
            initializePreviousImage();
            try
            {
                Console.CursorVisible = false;
            }catch(SecurityException exc)
            {
                //It will show so no problem.
            }
        }

        public void printImage()
        {
            for(int row=0; row<gameImage.Length; row++)
            {
                for(int column=0; column<gameImage[0].Length; column++)
                {
                    if (gameImage[row][column] != previousImage[row][column])
                    {
                        printChar(row, column, gameImage[row][column]);
                        previousImage[row][column] = gameImage[row][column];
                    }
                }
            }
        }

        private void printChar(int row, int column, char toPrint)
        {
            Console.SetCursorPosition(column, row);
            Console.Write(toPrint);
        }

        private void initializePreviousImage()
        {
            previousImage = new char[gameImage.Length][];
            for(byte row=0; row<gameImage.Length; row++)
            {
                previousImage[row] = new char[gameImage[row].Length];
            }
        }
    }

}
