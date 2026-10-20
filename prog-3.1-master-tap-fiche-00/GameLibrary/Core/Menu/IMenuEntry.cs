namespace GameLibrary.Core.Menu
{
    public interface IMenuEntry
    {
        string GetText();

        void Execute();
    }
}
