using GameLibrary.Core;
using GameLibrary.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryTests.Components
{
    public class WalletComponentTests
    {
        [Test]
        public void GetMoney_OnConstruction()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            Assert.That(wallet.GetMoney(), Is.EqualTo(100));
        }

        [Test]
        public void AddMoney_IncreasesMoney()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            wallet.AddMoney(100);

            Assert.That(wallet.GetMoney(), Is.EqualTo(200));
        }

        [Test]
        public void AddMoney_ZeroAmountDoNotChange()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            wallet.AddMoney(0);

            Assert.That(wallet.GetMoney(), Is.EqualTo(100));
        }

        [Test]
        public void UseMoney_EnoughMoney()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            bool is_money_use = wallet.UseMoney(50);

            Assert.That(is_money_use, Is.True);
            Assert.That(wallet.GetMoney(), Is.EqualTo(50));
        }

        [Test]
        public void UseMoney_NotEnoughMoneyReturnsFalseAndKeepsMoney()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            bool is_money_use = wallet.UseMoney(150);

            Assert.That(is_money_use, Is.False);
            Assert.That(wallet.GetMoney(), Is.EqualTo(100));
        }

        [Test]
        public void AddMoney_NegativeAmountThrowsException()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            Assert.Throws<ArgumentException>(() => wallet.AddMoney(-100));
        }

        [Test]
        public void UseMoney_NegativeAmountThrowsException()
        {
            WalletComponent wallet = new WalletComponent(new GameObject("wallet_test"), 100);

            Assert.Throws<ArgumentException>(() => wallet.UseMoney(-100));
        }

    }
}
