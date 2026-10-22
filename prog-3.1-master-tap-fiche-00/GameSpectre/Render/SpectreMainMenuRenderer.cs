using GameHost.Render;
using GameLibrary.Core.Menu;
using GameLibrary.Core.States;
using Spectre.Console;
using System.Buffers;

namespace GameSpectre.Render
{
    public class SpectreMainMenuRenderer : IScreenRenderer
    {
        private readonly SpectreRenderManager _render_manager;

        public SpectreMainMenuRenderer(SpectreRenderManager render_manager)
        {
            _render_manager = render_manager;
        }

        public void Render(IState state)
        {
            AnsiConsole.Clear();

            MainMenuState mainMenuState = (MainMenuState)state;
            List<IMenuEntry> entry_table = mainMenuState.GetMenuItemTable();
            int selectedIndex = mainMenuState.GetSelectedItemIndex();

            Table menu = new Table
            {
                Border = TableBorder.None,
                ShowHeaders = false,
                Expand = true
            };

            menu.AddColumn("");

            for (int entry_table_index = 0; entry_table_index < entry_table.Count; entry_table_index++)
            {
                string text = Markup.Escape(entry_table[entry_table_index].GetText());
                string row;

                if (entry_table_index == selectedIndex)
                {
                    row = $"[black on yellow] > {text} [/]";
                }
                else
                {
                    row = $"   {text}";
                }

                menu.AddRow(new Markup(row));
            }

            Panel panel = new Panel(menu)
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("Menu"),
                BorderStyle = new Style(Color.White)
            }.Expand();

            AnsiConsole.Write(panel);

        }
    }
}
