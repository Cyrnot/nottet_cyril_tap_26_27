using GameConsole.Presentation;
using GameHost.Render;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using System;

namespace GameConsole.Render
{
    public class ShopOpenRenderer : IScreenRenderer
    {
        private readonly ConsoleRenderManager _renderManager;

        public ShopOpenRenderer(ConsoleRenderManager render_manager)
        {
            _renderManager = render_manager;
        }

        public void Render(IState state)
        {
            ShopOpenState shop_open_state = state as ShopOpenState;
            InventoryComponent inventory_component = shop_open_state.GetInventoryComponent();

            string text = "";

            if (shop_open_state.GetIsReplaying())
            {
                text = $" - REPLAY:{shop_open_state.GetReplaySpeed()}x";
            }

            _renderManager.Draw(0, 0, $"{shop_open_state.GetShopComponent().GetGameObject().GetComponent<LocationComponent>().GetName()} close at {shop_open_state.GetShopComponent().GetShopData()._closingHour}:00{text}", ConsoleColor.Yellow, ConsoleColor.Black);
            _renderManager.Draw(0, 1, $"{shop_open_state.GetShopComponent().GetHour()}:{shop_open_state.GetShopComponent().GetMinute()}", ConsoleColor.White, ConsoleColor.Black);
            _renderManager.Draw(0, 2, $"Esc to return", ConsoleColor.Cyan, ConsoleColor.Black);

            _renderManager.Draw(0, 4, $"{shop_open_state.GetText()}", ConsoleColor.Black, ConsoleColor.White);

            int item_display_index = 6;

            for (int item_index = 0; item_index < inventory_component.GetEntryCount(); item_index++)
            {
                int quantity_at_index = inventory_component.GetEntryAtIndex(item_index).GetQuantity();
                bool is_item_selected = shop_open_state.GetSelectedItemIndex() == item_index;

                if (quantity_at_index > 0)
                {
                    if (is_item_selected)
                    {
                        _renderManager.Draw(0, item_display_index + item_index, $"{inventory_component.GetEntryAtIndex(item_index).GetItem()._name} ({inventory_component.GetEntryAtIndex(item_index).GetItem()._price}$) - qty: {quantity_at_index}", ConsoleColor.Black, ConsoleColor.Green);
                    }
                    else
                    {
                        _renderManager.Draw(0, item_display_index + item_index, $"{inventory_component.GetEntryAtIndex(item_index).GetItem()._name} ({inventory_component.GetEntryAtIndex(item_index).GetItem()._price}$) - qty: {quantity_at_index}", ConsoleColor.Green, ConsoleColor.Black);
                    }
                }
                else
                {
                    if (is_item_selected)
                    {
                        _renderManager.Draw(0, item_display_index + item_index, $"{inventory_component.GetEntryAtIndex(item_index).GetItem()._name} ({inventory_component.GetEntryAtIndex(item_index).GetItem()._price}$) - qty: out of stock", ConsoleColor.Black, ConsoleColor.DarkRed);
                    }
                    else
                    {
                        _renderManager.Draw(0, item_display_index + item_index, $"{inventory_component.GetEntryAtIndex(item_index).GetItem()._name} (   {inventory_component.GetEntryAtIndex(item_index).GetItem()._price}$) - qty: out of stock", ConsoleColor.DarkRed, ConsoleColor.Black);
                    }
                }
            }

            _renderManager.Render();
        }
    }
}
