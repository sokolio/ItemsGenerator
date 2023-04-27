using ItemsGenerator;

namespace ItemsGeneratorTests
{
    public class TestletTests
    {
        
        private List<Item> _itemsSet;

        [SetUp]
        public void Setup()
        {
            _itemsSet = new List<Item>();
            _itemsSet.AddRange(
                Enumerable.Range(0, Testlet.TOTAL_OPERATIONAL_ITEMS).Select(n =>
                    new Item { ItemId = $"Item_{n:D3}", ItemType = ItemTypeEnum.Operational }
                )
            );
            _itemsSet.AddRange(
                Enumerable.Range(Testlet.TOTAL_OPERATIONAL_ITEMS, Testlet.TOTAL_PRETEST_ITEMS).Select(n =>
                    new Item { ItemId = $"Item_{n:D3}", ItemType = ItemTypeEnum.Pretest }
                )
            );
        }

        [Test]
        public void GivenTestletWithCorrectSetOfItems_ThenRandomize_ShouldReturnCertainNumberOfItems()
        {
            // arrange
            var testlet = new Testlet("TestName", _itemsSet);

            // act
            var generatedItems = testlet.Randomize();

            Assert.That(generatedItems.Count, Is.EqualTo(Testlet.TOTAL_ITEMS_COUNT), 
                $"Testlet.Randomaze() should return {Testlet.TOTAL_ITEMS_COUNT} items");
        }

        [Test]
        public void GivenTestletWithCorrectSetOfItems_ThenRandomize_FirstSectionShouldContainPretestItems()
        {
            // arrange
            var testlet = new Testlet("TestName", _itemsSet);

            // act
            var generatedItems = testlet.Randomize();

            // assert
            foreach (var item in generatedItems.Take(Testlet.SECTION_ONE_ITEMS_COUNT))
            {
                Assert.That(item.ItemType, Is.EqualTo(ItemTypeEnum.Pretest), "Each of first two items shoud be pretest");
            }
        }

        [Test]
        [TestCase(0, Testlet.SECTION_ONE_ITEMS_COUNT)]
        [TestCase(Testlet.SECTION_ONE_ITEMS_COUNT, Testlet.SECTION_TWO_ITEMS_COUNT)]
        public void GivenTestletWithCorrectSetOfItems_ThenRandomize_OrderOfSelectedItemsShouldBeRandomized(int skip, int take)
        {
            // arrange
            var testlet = new Testlet("TestName", _itemsSet);
            var numberOfCalls = 3;

            // act
            var itemIdOrders = Enumerable.Range(0, numberOfCalls).Select(i =>
                String.Join(",", testlet.Randomize().Skip(skip).Take(take).Select(item => item.ItemId))
            ).ToList();

            var numberOfRepeats = itemIdOrders.Where(itemIds => itemIds.Equals(itemIdOrders[0])).Count();

            // assert
            Assert.That(numberOfRepeats, Is.LessThan(numberOfCalls), "Order of items should be randomized");
        }

        [Test]
        public void GivenTestletWithCorrectSetOfItems_ThenRandomize_ItemsShouldNotDuplicate()
        {
            // arrange
            var testlet = new Testlet("TestName", _itemsSet);

            // act
            var generatedItems = testlet.Randomize();
            var itemIdsSet = new HashSet<string>(generatedItems.Select(item => item.ItemId));

            // assert
            Assert.That(itemIdsSet.Count, Is.EqualTo(Testlet.TOTAL_ITEMS_COUNT), "Items should not duplicate");
        }

        [Test]
        public void CreateInstanceOfTestletWithIncorrectNumberOfItems_ShouldThrowAnException()
        {
            // arrange
            var incorrectItemsPool = _itemsSet.ToList();
            incorrectItemsPool.Add(
                new Item { ItemId = "Extra item", ItemType = ItemTypeEnum.Operational }
            );

            // act, assert
            var ex = Assert.Throws<ArgumentException>(() => {
                var testlet = new Testlet("TestName", incorrectItemsPool);
            });
        }

        [Test]
        public void CreateInstanceOfTestletWithIncorrectSetOfItems_ShouldThrowAnException()
        {
            // arrange
            var incorrectItemsPool = _itemsSet.ToList();
            incorrectItemsPool[0] = new Item { ItemId = "OperationalItem", ItemType = ItemTypeEnum.Pretest };

            // act, assert
            var ex = Assert.Throws<ArgumentException>(() => {
                var testlet = new Testlet("TestName", incorrectItemsPool);
            });
        }
    }
}