using GameLibrary.Core.States;

namespace GameHost.Render
{
    public interface IScreenRenderer
    {
        void Render(IState state);
    }
}
