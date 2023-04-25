using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace ItemsGenerator
{
    public class Testlet
    {
        public string TestletId { get; private set; }
        private List<Item> Items;
        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;
        }
        public List<Item> Randomize()
        {
            var rand = new Random();
            var remainingItems = Items.ToList();
            var randomizedItems = new List<Item>();

            for (int i = 0; i < Items.Count; i++)
            {
                var randomId = rand.Next(remainingItems.Count);
                var item = remainingItems[randomId];
                randomizedItems.Add(item);
                remainingItems.RemoveAt(randomId);
            }

            var pretestItems = randomizedItems.Where(item => item.ItemType == ItemTypeEnum.Pretest).ToList();

            var section1 = pretestItems.Take(2).ToList();

            var section1Ids = new HashSet<string>(section1.Select(item => item.ItemId));

            var section2 = randomizedItems.Where(item=>!section1Ids.Contains(item.ItemId)).ToList();

            var resutSet = section1.Concat(section2).ToList();

            return resutSet;
        }
    }
}
