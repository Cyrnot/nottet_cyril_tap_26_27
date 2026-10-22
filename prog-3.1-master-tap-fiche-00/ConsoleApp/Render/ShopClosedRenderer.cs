using GameConsole.Presentation;
using GameHost.Render;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using System;

namespace GameConsole.Render
{
    public class ShopClosedRenderer : IScreenRenderer
    {
        private readonly ConsoleRenderManager _renderManager;

        public ShopClosedRenderer(ConsoleRenderManager render_manager)
        {
            _renderManager = render_manager;
        }

        public void Render(IState state)
        {
            ShopClosedState shop_closed_state = state as ShopClosedState;

            string text = "";

            if (shop_closed_state.GetIsReplaying())
            {
                text = $" - REPLAY:{shop_closed_state.GetReplaySpeed()}x";
            }

            _renderManager.Draw(0, 0, $"{shop_closed_state.GetShopComponent().GetGameObject().GetComponent<LocationComponent>().GetName()}{text}", ConsoleColor.Yellow, ConsoleColor.Black);
            _renderManager.Draw(0, 1, $"{shop_closed_state.GetShopComponent().GetHour()}:{shop_closed_state.GetShopComponent().GetMinute()}", ConsoleColor.White, ConsoleColor.Black);
            _renderManager.Draw(0, 2, $"Closed. Open at {shop_closed_state.GetShopComponent().GetShopData()._openingHour}:00 ", ConsoleColor.Red, ConsoleColor.Black);
            _renderManager.Draw(0, 3, $"Esc to return", ConsoleColor.Cyan, ConsoleColor.Black);
            _renderManager.Render();
        }
    }
}
