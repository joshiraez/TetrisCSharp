using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.Input.Implementation
{
    public class KeyboardController : ITetrisControl
    {
        public bool isUpPressed()
        {
            return false;//Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W);
        }
        public bool isDownPressed()
        {
            return false;//Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S);
        }

        public bool isLeftPressed()
        {
            return false;//Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A);
        }

        public bool isRightPressed()
        {
            return false;//Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D);
        }
        public bool isFirePressed()
        {
            return true;//Keyboard.IsKeyDown(Key.Space) || Keyboard.IsKeyDown(Key.E);
        }

    }
}
