namespace Inventory.Items
{
    public class Item
    {
        public string Id { get; private set; }
        public int Quantity { get; private set; }

        public Item(string id)
        {
            Id = id;
            Quantity = 1;
        }

        public Item(string id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }

        public Item(ItemScriptable data)
        {
            Id = data.Id;
            Quantity = 1;
        }

        public void Add(int amount)
        {
            Quantity += amount;
        }

        public void Merge(Item item)
        {
            if(item.Id != Id)
                return;

            Quantity += item.Quantity;
        }
    }
}