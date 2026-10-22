using GameConsole.Presentation;
using GameConsole.Render;
using GameHost.Presentation;
using GameHost.Render;
using GameLibrary.Core.States;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string game_data_path = AppContext.BaseDirectory + "../../../../GameData/";

            ConsoleRenderManager render_manager = new ConsoleRenderManager();

            Dictionary<Type, IScreenRenderer> Renderer_Table = new Dictionary<Type, IScreenRenderer>
            {
                [typeof(MainMenuState)] = new MainMenuRenderer(render_manager),
                [typeof(ExploringState)] = new ExploringRenderer(render_manager),
                [typeof(ShopClosedState)] = new ShopClosedRenderer(render_manager),
                [typeof(ShopOpenState)] = new ShopOpenRenderer(render_manager)
            };

            new GameEngine(Renderer_Table, game_data_path).Run();
        }
    }
}