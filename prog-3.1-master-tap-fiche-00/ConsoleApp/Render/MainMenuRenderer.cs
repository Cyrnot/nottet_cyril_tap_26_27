using GameConsole.Presentation;
using GameHost.Render;
using GameLibrary.Core.States;
using System;

namespace GameConsole.Render
{
    public class MainMenuRenderer : IScreenRenderer
    {
        private readonly ConsoleRenderManager _renderManager;

        public MainMenuRenderer(ConsoleRenderManager render_manager)
        {
            _renderManager = render_manager;
        }

        public void Render(IState state)
        {
            MainMenuState main_menu_state = state as MainMenuState;

            for (int menu_index = 0; menu_index < main_menu_state.GetMenuItemTable().Count; menu_index++)
            {
                if (main_menu_state.GetSelectedItemIndex() != menu_index)
                {
                    _renderManager.Draw(0, menu_index, $"{main_menu_state.GetMenuItemTable()[menu_index].GetText()}", ConsoleColor.Green, ConsoleColor.Black);
                }
                else
                {
                    _renderManager.Draw(0, menu_index, $"{main_menu_state.GetMenuItemTable()[menu_index].GetText()}", ConsoleColor.Black, ConsoleColor.Green);
                }
            }

            _renderManager.Render();
        }
    }
}
