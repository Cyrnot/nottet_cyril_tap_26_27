using GameLibrary.Core;
using GameLibrary.Core.Components;
using Spectre.Console;

namespace GameSpectre.Render
{
    public static class SpectreHeroPanel
    {
        public static Panel Create(GameObject hero)
        {
            HeroComponent heroComponent = hero.GetComponent<HeroComponent>();
            HealthComponent healthComponent = hero.GetComponent<HealthComponent>();
            WalletComponent walletComponent = hero.GetComponent<WalletComponent>();
            InventoryComponent inventoryComponent = hero.GetComponent<InventoryComponent>();

            int healthBarWidth = 20;
            int filledHealth = 0;
            int maxHealth = healthComponent.GetMaxHealth();
            int currentHealth = healthComponent.GetHealth();

            if (maxHealth > 0)
            {
                filledHealth = (int)Math.Round(
                    (double)currentHealth / maxHealth * healthBarWidth);
            }

            filledHealth = Math.Clamp(filledHealth, 0, healthBarWidth);

            string healthBar =
                new string('█', filledHealth)
                + new string('░', healthBarWidth - filledHealth);

            Markup playerInfo = new Markup(
                $"[bold]{Markup.Escape(heroComponent.GetName())}[/]\n" +
                $"[red]{healthBar}[/] {currentHealth}/{maxHealth} HP\n" +
                $"Gold: {walletComponent.GetMoney()}");

            Table inventoryTable = new Table
            {
                Border = TableBorder.Rounded,
                Expand = true
            };

            inventoryTable.AddColumn("Item");
            inventoryTable.AddColumn("Quantity");

            for (int itemIndex = 0;
                 itemIndex < inventoryComponent.GetEntryCount();
                 itemIndex++)
            {
                var entry = inventoryComponent.GetEntryAtIndex(itemIndex);

                inventoryTable.AddRow(
                    new Markup(Markup.Escape(entry.GetItem()._name)),
                    new Markup(entry.GetQuantity().ToString()));
            }

            if (inventoryComponent.GetEntryCount() == 0)
            {
                inventoryTable.AddRow(
                    new Markup("[grey]Empty[/]"),
                    new Markup("-"));
            }

            return new Panel(new Rows(playerInfo, inventoryTable))
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("Player"),
                BorderStyle = new Style(Color.White)
            }.Expand();
        }
    }
}