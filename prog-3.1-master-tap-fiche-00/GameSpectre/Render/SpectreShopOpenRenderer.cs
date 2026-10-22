using GameHost.Render;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using Spectre.Console;

namespace GameSpectre.Render
{
    public class SpectreShopOpenRenderer : IScreenRenderer
    {
        private readonly SpectreRenderManager _render_manager;

        public SpectreShopOpenRenderer(SpectreRenderManager render_manager)
        {
            _render_manager = render_manager;
        }

        public void Render(IState state)
        {
            AnsiConsole.Clear();

            ShopOpenState shopState = (ShopOpenState)state;
            ShopComponent shop = shopState.GetShopComponent();
            InventoryComponent stock = shopState.GetInventoryComponent();

            Table stockTable = new Table
            {
                Border = TableBorder.Rounded,
                Expand = true
            };

            stockTable.AddColumn("Object");
            stockTable.AddColumn("Price");
            stockTable.AddColumn("Stock");

            for (int itemIndex = 0; itemIndex < stock.GetEntryCount(); itemIndex++)
            {
                var entry = stock.GetEntryAtIndex(itemIndex);
                var item = entry.GetItem();

                string itemName = Markup.Escape(item._name);
                string price = $"{item._price} gold";
                string quantity = entry.GetQuantity().ToString();

                if (entry.GetQuantity() == 0)
                {
                    itemName = $"[red]{itemName}[/]";
                    price = $"[red]{price}[/]";
                    quantity = "[red]out of stock[/]";
                }
                else if (itemIndex == shopState.GetSelectedItemIndex())
                {
                    itemName = $"[black on yellow]{itemName}[/]";
                    price = $"[black on yellow]{price}[/]";
                    quantity = $"[black on yellow]{quantity}[/]";
                }

                stockTable.AddRow(
                    new Markup(itemName),
                    new Markup(price),
                    new Markup(quantity));
            }

            int openingMinute = shop.GetShopData()._openingHour * 60;
            int closingMinute = shop.GetShopData()._closingHour * 60;
            int currentMinute = shop.GetHour() * 60 + shop.GetMinute();

            int totalMinutes = closingMinute - openingMinute;

            if (totalMinutes < 1)
            {
                totalMinutes = 1;
            }

            int remainingMinutes = closingMinute - currentMinute;

            if (remainingMinutes < 0)
            {
                remainingMinutes = 0;
            }

            if (remainingMinutes > totalMinutes)
            {
                remainingMinutes = totalMinutes;
            }

            int barWidth = 20;
            int filledWidth = (int)Math.Round((double)remainingMinutes / totalMinutes * barWidth);

            string timeBar =new string('█', filledWidth) + new string('░', barWidth - filledWidth);

            LocationComponent location = shop.GetGameObject().GetComponent<LocationComponent>();

            Panel shopPanel = new Panel(new Rows(
                new Markup($"Time: {shop.GetHour():00}:{shop.GetMinute():00}  " +$"Closing: {shop.GetShopData()._closingHour:00}:00"),
                new Markup($"[yellow]{timeBar}[/]  {remainingMinutes} min remaining"),
                stockTable))
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader(Markup.Escape(location.GetName())),
                BorderStyle = new Style(Color.White)
            }.Expand();

            Grid layout = new Grid();
            layout.AddColumn();
            layout.AddColumn();
            layout.AddRow(shopPanel, SpectreHeroPanel.Create(shopState.GetHeroObject()));

            AnsiConsole.Write(layout);
        }


    }
}