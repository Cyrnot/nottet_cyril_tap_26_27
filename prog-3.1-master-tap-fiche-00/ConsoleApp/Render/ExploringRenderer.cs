using GameConsole.Presentation;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Render
{
    public class ExploringRenderer : IScreenRenderer
    {
        public void Render(IState state, ConsoleRenderManager render_manager)
        {
            ExploringState exploring_state = state as ExploringState;

            string text = "";
            int line = 0;

            if (exploring_state.GetIsReplaying())
            {
                text = $" - REPLAY:{exploring_state.GetReplaySpeed()}x";
            }

            render_manager.Draw(0, line++, $"Game in progress...{text}\n", ConsoleColor.Magenta, ConsoleColor.Black);

            LocationComponent current_location = exploring_state.GetCurrentLocation();

            GameObject hero = exploring_state.GetHeroObject();
            HeroComponent hero_component = hero.GetComponent<HeroComponent>();
            HealthComponent health_component = hero.GetComponent<HealthComponent>();
            WalletComponent wallet_component = hero.GetComponent<WalletComponent>();

            render_manager.Draw(0, line++, $"{hero_component.GetName()} - {health_component.GetHealth()} HP - Money: {wallet_component.GetMoney()}", ConsoleColor.Red, ConsoleColor.Black);

            InventoryComponent inventory_component = hero.GetComponent<InventoryComponent>();

            for (int item_index = 0; item_index < inventory_component.GetEntryCount(); item_index++)
            {
                render_manager.Draw(0, line++, $"{inventory_component.GetEntryAtIndex(item_index).GetItem()._name}", ConsoleColor.Red, ConsoleColor.Black);
            }

            render_manager.Draw(0, line++, $"Exploring {current_location.GetName()}\n", ConsoleColor.Cyan, ConsoleColor.Black);

            for (int location_index = 0; location_index < current_location.GetChildLocationCount(); location_index++)
            {
                Connection current_connection = current_location.GetChildLocationAtIndex(location_index);

                if (location_index == exploring_state.GetSelectedDestinationIndex())
                {
                    render_manager.Draw(0, line++ + location_index, $"{location_index + 1}. {current_connection.GetDestinationLocation().GetName()}: Duration: {current_connection.GetTimeToReachDestination()}", ConsoleColor.Black, ConsoleColor.Green);
                }
                else
                {
                    render_manager.Draw(0, line++ + location_index, $"{location_index + 1}. {current_connection.GetDestinationLocation().GetName()}: Duration: {current_connection.GetTimeToReachDestination()}", ConsoleColor.Green, ConsoleColor.Black);
                }
            }
        }
    }
}
