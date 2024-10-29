namespace Inventory.Items
{
    public class Item
    {
        public string Id { get; private set; }
        public int Quantity { get; private set; }
        
        public bool Stackable { get; private set; }

        public delegate void ItemUpdatedDelegate(Item item);
        public event ItemUpdatedDelegate ItemUpdatedEvent;

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
            Quantity = data.Amount > 0 ? data.Amount : 1;
            Stackable = data.IsStackable;
            OnItemUpdated();
        }

        public void Add(int amount)
        {
            Quantity += amount;
            OnItemUpdated();
        }

        public void SetQuantity(int amount)
        {
            Quantity = amount;
            OnItemUpdated();
        }

        public void Merge(Item item)
        {
            if(item.Id != Id)
                return;

            Quantity += item.Quantity;
            OnItemUpdated();
        }

        private void OnItemUpdated()
        {
            ItemUpdatedEvent?.Invoke(this);
        }
    }
}