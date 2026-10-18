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

            render_manager.Draw(0, 0, "Game in progress...\n", ConsoleColor.Magenta, ConsoleColor.Black);

            LocationComponent current_location = exploring_state.GetCurrentLocation();

            render_manager.Draw(0, 1, $"Exploring {current_location.GetName()}\n", ConsoleColor.Cyan, ConsoleColor.Black);

            for (int location_index = 0; location_index < current_location.GetChildLocationCount(); location_index++)
            {
                Connection current_connection = current_location.GetChildLocationAtIndex(location_index);

                if (location_index == exploring_state.GetSelectedDestinationIndex())
                {
                    render_manager.Draw(0, 2 + location_index, $"{location_index + 1}. {current_connection.GetDestinationLocation().GetName()}: Duration: {current_connection.GetTimeToReachDestination()}", ConsoleColor.Black, ConsoleColor.Green);
                }
                else
                {
                    render_manager.Draw(0, 2 + location_index, $"{location_index + 1}. {current_connection.GetDestinationLocation().GetName()}: Duration: {current_connection.GetTimeToReachDestination()}", ConsoleColor.Green, ConsoleColor.Black);
                }
            }
        }
    }
}
