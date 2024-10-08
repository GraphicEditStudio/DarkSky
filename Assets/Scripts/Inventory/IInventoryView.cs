using System.Collections.Generic;
using Inventory.Items;
using Items;

namespace Inventory
{
    public interface IInventoryView
    {
        public void Initialize();
        public void ItemAdded(Item item);
        public void ItemRemoved(Item item);
        public void ItemListCleared();
    }
}