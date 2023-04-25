using ItemsGenerator;

namespace ItemsGeneratorTests
{
    public class TestletTests
    {
        private List<Item> _itemsPool;

        [SetUp]
        public void Setup()
        {
            _itemsPool = new List<Item>() { 
                new Item { ItemId = "Item_001", ItemType = ItemTypeEnum.Operational },
                new Item { ItemId = "Item_002", ItemType = ItemTypeEnum.Operational },
                new Item { ItemId = "Item_003", ItemType = ItemTypeEnum.Operational },
                new Item { ItemId = "Item_004", ItemType = ItemTypeEnum.Operational },
                new Item { ItemId = "Item_005", ItemType = ItemTypeEnum.Operational },
                new Item { ItemId = "Item_006", ItemType = ItemTypeEnum.Operational },
                new Item { ItemId = "Item_007", ItemType = ItemTypeEnum.Pretest },
                new Item { ItemId = "Item_008", ItemType = ItemTypeEnum.Pretest },
                new Item { ItemId = "Item_009", ItemType = ItemTypeEnum.Pretest },
                new Item { ItemId = "Item_010", ItemType = ItemTypeEnum.Pretest }
            };

        }

        [Test]
        public void GivenTestletWith10Items_ThenRandomize_ShouldReturn10Items()
        {
            var testlet = new Testlet("TestName", _itemsPool);

            var generatedItems = testlet.Randomize();

            Assert.That(generatedItems.Count, Is.EqualTo(10), "Testlet.Randomaze() should return 10 items");
        }

        [Test]
        public void GivenTestletWith10Items_ThenRandomize_FirstTwoItemsShouldBePretest()
        {
            var testlet = new Testlet("TestName", _itemsPool);

            var generatedItems = testlet.Randomize();

            foreach (var item in generatedItems.Take(2))
            {
                Assert.That(item.ItemType, Is.EqualTo(ItemTypeEnum.Pretest), "Each of first two items shoud be pretest");
            }
        }

        [Test]
        public void GivenTestletWith10Items_ThenRandomize_OrderOfFirst2ItemsShouldBeRandomized()
        {
            var testlet = new Testlet("TestName", _itemsPool);

            var itemsSet1Id = String.Join(",", testlet.Randomize().Take(2).Select(item => item.ItemId));
            var itemsSet2Id = String.Join(",", testlet.Randomize().Take(2).Select(item => item.ItemId));
            var itemsSet3Id = String.Join(",", testlet.Randomize().Take(2).Select(item => item.ItemId));

            Assert.That(itemsSet1Id.Equals(itemsSet2Id) && itemsSet1Id.Equals(itemsSet3Id)
                , Is.False, "Order of first two items should be randomized");
        }

        [Test]
        public void GivenTestletWith10Items_ThenRandomize_OrderOfNext8ItemsShouldBeRandomized()
        {
            var testlet = new Testlet("TestName", _itemsPool);

            var itemsSet1Id = String.Join(",", testlet.Randomize().Skip(2).Select(item => item.ItemId));
            var itemsSet2Id = String.Join(",", testlet.Randomize().Skip(2).Select(item => item.ItemId));
            var itemsSet3Id = String.Join(",", testlet.Randomize().Skip(2).Select(item => item.ItemId));

            Assert.That(itemsSet1Id.Equals(itemsSet2Id) && itemsSet1Id.Equals(itemsSet3Id),
                Is.False, "Order of next 8 items should be randomized");
        }

        [Test]
        public void GivenTestletWith10Items_ThenRandomize_ItemsShouldNotDuplicate()
        {
            var testlet = new Testlet("TestName", _itemsPool);

            var generatedItems = testlet.Randomize();
            var itemIdsSet = new HashSet<string>(generatedItems.Select(item => item.ItemId));

            Assert.That(itemIdsSet.Count, Is.EqualTo(10), "Items should not duplicate");
        }
    }
}