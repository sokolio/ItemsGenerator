using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace ItemsGenerator
{
    public class Testlet
    {     
        public const int TOTAL_ITEMS_COUNT = 10;
        public const int TOTAL_PRETEST_ITEMS = 4;
        public const int TOTAL_OPERATIONAL_ITEMS = TOTAL_ITEMS_COUNT - TOTAL_PRETEST_ITEMS;
        public const int SECTION_ONE_ITEMS_COUNT = 2;
        public const int SECTION_TWO_ITEMS_COUNT = TOTAL_ITEMS_COUNT - SECTION_ONE_ITEMS_COUNT;

        public string TestletId { get; private set; }

        private List<Item> Items;

        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;
            ValidateItems();
        }
        public List<Item> Randomize()
        {
            var reorderedItems = ReorderItems(Items);

            var pretestItems = reorderedItems.Where(item => item.ItemType == ItemTypeEnum.Pretest).ToList();

            var section1 = pretestItems.Take(SECTION_ONE_ITEMS_COUNT).ToList();

            var section1Ids = new HashSet<string>(section1.Select(item => item.ItemId));

            var section2 = reorderedItems.Where(item=>!section1Ids.Contains(item.ItemId)).ToList();

            var resutSet = section1.Concat(section2).ToList();

            return resutSet;
        }

        private void ValidateItems()
        {
            var pretestItemsCount = Items.Where(item => item.ItemType == ItemTypeEnum.Pretest).Count();

            if ( Items.Count != TOTAL_ITEMS_COUNT || pretestItemsCount != TOTAL_PRETEST_ITEMS) 
            {
                throw new ArgumentException($"Items set should contain {TOTAL_PRETEST_ITEMS} pretest items and {TOTAL_OPERATIONAL_ITEMS} operational items");
            }
        }
        private List<Item> ReorderItems(List<Item> items)
        {
            var rand = new Random();
            var remainingItems = Items.ToList();
            var reorderedItems = new List<Item>();

            for (int i = 0; i < Items.Count; i++)
            {
                var randomId = rand.Next(remainingItems.Count);
                var item = remainingItems[randomId];
                reorderedItems.Add(item);
                remainingItems.RemoveAt(randomId);
            }

            return reorderedItems;
        }
    }
}
