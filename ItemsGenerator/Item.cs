namespace ItemsGenerator
{
    public class Item
    {
        public string ItemId { get; set; }
        public ItemTypeEnum ItemType { get; set; }
    }

    public enum ItemTypeEnum
    {
        Pretest = 0,
        Operational = 1
    }
}