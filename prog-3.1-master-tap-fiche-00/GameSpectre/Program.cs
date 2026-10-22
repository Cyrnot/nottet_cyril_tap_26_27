using GameHost.Presentation;
using GameHost.Render;
using GameLibrary.Core.States;
using GameSpectre.Render;

namespace GameSpectre
{
    class Program
    {
        static void Main(string[] args)
        {
            string game_data_path = "../../../../GameData/";

            SpectreRenderManager render_manager = new SpectreRenderManager();

            Dictionary<Type, IScreenRenderer> renderer_table = new Dictionary<Type, IScreenRenderer>
                {
                    [typeof(MainMenuState)] = new SpectreMainMenuRenderer(render_manager),
                    [typeof(ExploringState)] = new SpectreExploringRenderer(render_manager),
                    [typeof(ShopClosedState)] = new SpectreShopClosedRenderer(render_manager),
                    [typeof(ShopOpenState)] = new SpectreShopOpenRenderer(render_manager)
                };

            new GameEngine(renderer_table, game_data_path).Run();
        }
    }
}