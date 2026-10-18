using GameConsole.Presentation;
using GameLibrary.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Render
{
    public interface IScreenRenderer
    {
        void Render(IState state, ConsoleRenderManager render_manager);
    }
}
