using System;
using System.Collections.Generic;
using System.Threading;
using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.Input.Implementation
{
    public class KeyboardController : ITetrisControl
    {
        //I have been looking for a way to handle the key input presses from console without events or
        //another thread, but sadly I haven't found one without using libraries which defeats the purpose of this project.
        //So for now, the only posibility I have is to implement a Thread who will be getting
        //all the asynchronous inputs.
        //I also find this thread design just TERRIBLE. I'll have to look into it in the future, this is just a quick fix.
        Thread inputReader;

        private Dictionary<ActionEnum, bool> inputsFlags =
            new Dictionary<ActionEnum, bool>
            {
                { ActionEnum.UP, false },
                { ActionEnum.DOWN, false },
                { ActionEnum.LEFT, false },
                { ActionEnum.RIGHT, false },
                { ActionEnum.FIRE, false },
            };

        private enum ActionEnum
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            FIRE
        }

        public KeyboardController()
        {
            inputReader = new Thread(readInputs);
            inputReader.Start();
        }
        
        private void readInputs()
        {
            while (true)
            {
                 while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            inputsFlags[ActionEnum.UP] = true;
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            inputsFlags[ActionEnum.DOWN] = true;
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            inputsFlags[ActionEnum.LEFT] = true;
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            inputsFlags[ActionEnum.RIGHT] = true;
                            break;
                        case ConsoleKey.E:
                        case ConsoleKey.Spacebar:
                            inputsFlags[ActionEnum.FIRE] = true;
                            break;
                    }
                }
                Thread.Yield();
            }
        }

        public bool isUpPressed()
        {
            if (inputsFlags[ActionEnum.UP])
            {
                inputsFlags[ActionEnum.UP] = false;
                return true;
            }else
            {
                return false;
            }
        }
        public bool isDownPressed()
        {
            if (inputsFlags[ActionEnum.DOWN])
            {
                inputsFlags[ActionEnum.DOWN] = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isLeftPressed()
        {

            if (inputsFlags[ActionEnum.LEFT])
            {
                inputsFlags[ActionEnum.LEFT] = false;
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool isRightPressed()
        {
            if (inputsFlags[ActionEnum.RIGHT])
            {
                inputsFlags[ActionEnum.RIGHT] = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isFirePressed()
        {
            if (inputsFlags[ActionEnum.FIRE])
            {
                inputsFlags[ActionEnum.FIRE] = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
