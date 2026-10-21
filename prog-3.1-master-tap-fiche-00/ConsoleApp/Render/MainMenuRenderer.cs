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
            MainMenuState main_menu_state = state as MainMenuState;

            for (int menu_index = 0; menu_index < main_menu_state.GetMenuItemTable().Count; menu_index++)
            {
                if (main_menu_state.GetSelectedItemIndex() != menu_index)
                {
                    render_manager.Draw(0, menu_index, $"{main_menu_state.GetMenuItemTable()[menu_index].GetText()}", ConsoleColor.Green, ConsoleColor.Black);
                }
                else
                {
                    render_manager.Draw(0, menu_index, $"{main_menu_state.GetMenuItemTable()[menu_index].GetText()}", ConsoleColor.Black, ConsoleColor.Green);
                }
            }
        }
    }
}
