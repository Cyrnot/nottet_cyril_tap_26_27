using GameConsole.Presentation;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Render
{
    public class ShopClosedRenderer : IScreenRenderer
    {
        public void Render(IState state, ConsoleRenderManager render_manager)
        {
            ShopClosedState shop_closed_state = state as ShopClosedState;

            string text = "";

            if (shop_closed_state.GetIsReplaying())
            {
                text = $" - REPLAY:{shop_closed_state.GetReplaySpeed()}x";
            }

            render_manager.Draw(0, 0,$"{shop_closed_state.GetShopComponent().GetGameObject().GetComponent<LocationComponent>().GetName()}{text}", ConsoleColor.Yellow, ConsoleColor.Black);
            render_manager.Draw(0, 1, $"{shop_closed_state.GetShopComponent().GetHour()}:{shop_closed_state.GetShopComponent().GetMinute()}", ConsoleColor.White, ConsoleColor.Black);
            render_manager.Draw(0, 2, $"Closed. Open at {shop_closed_state.GetShopComponent().GetShopData()._openingHour}:00 ", ConsoleColor.Red, ConsoleColor.Black);
            render_manager.Draw(0, 3, $"Esc to return", ConsoleColor.Cyan, ConsoleColor.Black);
        }
    }
}
