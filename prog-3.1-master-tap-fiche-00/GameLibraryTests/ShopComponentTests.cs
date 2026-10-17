// using GameLibrary;
// using GameLibrary.Csv;
// using GameLibrary.Components;
//
// namespace GameLibraryTests
// {
//     // Ces tests supposent ShopComponent.GAME_MINUTES_PER_SECOND = 10.0f :
//     // FixedUpdate(1.0f) fait avancer de 10 minutes de jeu.
//     // Les pas utilisés (1.0f, 6.0f, 60.0f…) sont exacts en binaire : pas d'erreur d'arrondi.
//     public class ShopComponentTests
//     {
//         private ShopComponent CreateShop(int opening_hour, int closing_hour)
//         {
//             ShopData shop_data = new ShopData("GUN_SHOP", opening_hour, closing_hour);
//             List<ItemData> item_datas = new List<ItemData>();
//
//             item_datas.Add(new ItemData("COLT", "Colt Peacemaker", "GUN_SHOP", 50, 3));
//             item_datas.Add(new ItemData("RIFLE", "Winchester Rifle", "GUN_SHOP", 120, 0));
//
//             return new ShopComponent(shop_data, item_datas);
//         }
//
//         [Test]
//         public void GetHour_ReturnsEightAtCreation()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             int hour = shop.GetHour();
//
//             Assert.That(hour, Is.EqualTo(8));
//             Assert.That(shop.GetMinute(), Is.EqualTo(0));
//         }
//
//         [Test]
//         public void FixedUpdate_AdvancesTenGameMinutesPerRealSecond()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             shop.FixedUpdate(1.0f);
//
//             Assert.That(shop.GetHour(), Is.EqualTo(8));
//             Assert.That(shop.GetMinute(), Is.EqualTo(10));
//         }
//
//         [Test]
//         public void FixedUpdate_AccumulatesSmallSteps()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             shop.FixedUpdate(0.5f);
//             shop.FixedUpdate(0.5f);
//
//             Assert.That(shop.GetMinute(), Is.EqualTo(10));
//         }
//
//         [Test]
//         public void FixedUpdate_CanAdvanceSeveralHoursInOneCall()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             shop.FixedUpdate(30.0f);
//
//             Assert.That(shop.GetHour(), Is.EqualTo(13));
//             Assert.That(shop.GetMinute(), Is.EqualTo(0));
//         }
//
//         [Test]
//         public void FixedUpdate_GoesToMidnightAfterTwentyThreeFiftyNine()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             shop.FixedUpdate(96.0f);
//
//             Assert.That(shop.GetHour(), Is.EqualTo(0));
//             Assert.That(shop.GetMinute(), Is.EqualTo(0));
//         }
//
//         [Test]
//         public void FixedUpdate_ReturnsToEightAfterTwentyFourHours()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             shop.FixedUpdate(144.0f);
//
//             Assert.That(shop.GetHour(), Is.EqualTo(8));
//             Assert.That(shop.GetMinute(), Is.EqualTo(0));
//         }
//
//         [Test]
//         public void GetIsOpen_ReturnsTrueAtOpeningHour()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             bool is_open = shop.GetIsOpen();
//
//             Assert.That(is_open, Is.True);
//         }
//
//         [Test]
//         public void GetIsOpen_ReturnsFalseAtClosingHour()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             shop.FixedUpdate(60.0f);
//
//             Assert.That(shop.GetHour(), Is.EqualTo(18));
//             Assert.That(shop.GetIsOpen(), Is.False);
//         }
//
//         [Test]
//         public void GetIsOpen_ReturnsFalseBeforeOpeningHour()
//         {
//             ShopComponent shop = CreateShop(9, 17);
//
//             bool is_open = shop.GetIsOpen();
//
//             Assert.That(is_open, Is.False);
//         }
//
//         [Test]
//         public void GetIsOpen_ReturnsTrueOnceOpeningHourIsReached()
//         {
//             ShopComponent shop = CreateShop(9, 17);
//
//             shop.FixedUpdate(6.0f);
//
//             Assert.That(shop.GetHour(), Is.EqualTo(9));
//             Assert.That(shop.GetIsOpen(), Is.True);
//         }
//
//         [Test]
//         public void GetItemAtIndex_ReturnsItemsInFileOrder()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             int item_count = shop.GetItemCount();
//
//             Assert.That(item_count, Is.EqualTo(2));
//             Assert.That(shop.GetItemAtIndex(0)._id, Is.EqualTo("COLT"));
//             Assert.That(shop.GetItemAtIndex(1)._id, Is.EqualTo("RIFLE"));
//         }
//
//         [Test]
//         public void GetQuantityAtIndex_ReturnsQuantityFromData()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             int quantity = shop.GetQuantityAtIndex(0);
//
//             Assert.That(quantity, Is.EqualTo(3));
//         }
//
//         [Test]
//         public void TryRemoveOneAtIndex_DecreasesQuantityByOne()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             bool is_removed = shop.TryRemoveOneAtIndex(0);
//
//             Assert.That(is_removed, Is.True);
//             Assert.That(shop.GetQuantityAtIndex(0), Is.EqualTo(2));
//         }
//
//         [Test]
//         public void TryRemoveOneAtIndex_ReturnsFalseAndKeepsZeroWhenOutOfStock()
//         {
//             ShopComponent shop = CreateShop(8, 18);
//
//             bool is_removed = shop.TryRemoveOneAtIndex(1);
//
//             Assert.That(is_removed, Is.False);
//             Assert.That(shop.GetQuantityAtIndex(1), Is.EqualTo(0));
//         }
//     }
// }
