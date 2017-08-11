using TetrisCSharp.GameStatus;
using TetrisCSharp.Input.Interface;

namespace TetrisCSharp.GameLogic.Interface
{
    public interface ITetrisGameLogic
    {
        void Start(Game game);
        void Update(Game game, ITetrisControl controller);
        bool HasFinished(Game game);
    }
}
