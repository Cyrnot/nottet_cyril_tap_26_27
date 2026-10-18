using GameConsole.Presentation;
using GameLibrary.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Render
{
    public class MainMenuRenderer : IScreenRenderer
    {
        public void Render(IState state, ConsoleRenderManager render_manager)
        {
            render_manager.Draw(0, 0, "Enter to start", ConsoleColor.Green, ConsoleColor.Black);
            render_manager.Draw(0, 1, "Esc to quit", ConsoleColor.Green, ConsoleColor.Black);
        }
    }
}
