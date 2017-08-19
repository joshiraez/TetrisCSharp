using TetrisCSharp.GameStatus;
using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.GameLogic.Interface
{
    public interface ITetrisGameLogic
    {
        void Start();
        void Update();
        bool HasFinished();
    }
}
