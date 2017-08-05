namespace TetrisCSharp.Input.Interface
{
    public interface ITetrisControl
    {
        bool isUpPressed();
        bool isDownPressed();
        bool isRightPressed();
        bool isLeftPressed();
        bool isFirePressed();
    }
}
