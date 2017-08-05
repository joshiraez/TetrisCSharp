using System;
using System.Windows.Input;
using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.Input.Implementation
{
    public class KeyboardController : ITetrisControl
    {
        public bool isUpPressed()
        {
            return Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W);
        }
        public bool isDownPressed()
        {
            return Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S);
        }

        public bool isLeftPressed()
        {
            return Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A);
        }

        public bool isRightPressed()
        {
            return Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D);
        }
        public bool isFirePressed()
        {
            return Keyboard.IsKeyDown(Key.Space) || Keyboard.IsKeyDown(Key.E);
        }

    }
}
