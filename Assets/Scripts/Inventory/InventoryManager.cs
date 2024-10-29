using System.Collections.Generic;
using System.Linq;
using Extensions;
using Inventory.Items;
using Items;
using UnityEngine;
using Utils;
using Weapons;

namespace Inventory
{
    public sealed class InventoryManager : MonoSingleton<InventoryManager>
    {
        [Tooltip("Has to be an object with an IInventory script attached")]
        [SerializeField]
        private GameObject viewReference;
        private IInventoryView view;
        
        public List<ItemScriptable> itemsData;
        public List<WeaponSettings> weaponsData;
        public List<AmmoSettings> ammoData;
        public ObservableList<Item> PlayerItems { get; private set; }

        public delegate void ItemCollectedDelegate(EItemType type, ItemScriptable data);
        public event ItemCollectedDelegate OnItemCollected;
        
        
        #region Unity Hooks
        protected override void OnAwake()
        {
            PlayerItems ??= new ObservableList<Item>();
            view = viewReference.GetComponent<IInventoryView>();
            
            if (view == null)
            {
                Debug.LogError($"View reference is not of type IInventoryView", gameObject);
                return;
            }

            view.Initialize();
            PlayerItems.OnItemAdded += view.ItemAdded;
            PlayerItems.OnItemRemoved += view.ItemRemoved;
            PlayerItems.OnListCleared += view.ItemListCleared;
            
            // //testing only
            // foreach (var data in itemsData)
            // {
            //     PlayerItems.Add(new Item(data));
            // }
        }

        private void OnDestroy()
        {
            if (view != null)
            {
                PlayerItems.OnItemAdded -= view.ItemAdded;
                PlayerItems.OnItemRemoved -= view.ItemRemoved;
                PlayerItems.OnListCleared -= view.ItemListCleared;
            }
        }
        
        #endregion Unity Hooks

        #region Item Details

        public ItemScriptable GetItem(string id)
        {
            var dataMerge = new List<ItemScriptable>(itemsData);
            dataMerge.AddRange(weaponsData);
            dataMerge.AddRange(ammoData);
            return dataMerge.FirstOrDefault(data => data.Id == id);
        }
        public string GetItemName(string id)
        {
            var data = GetItem(id);
            return data == null ? "" : data.Name;
        }

        public string GetItemDescription(string id)
        {
            var data = GetItem(id);
            return data == null ? "" : data.Description;
        }

        public Texture GetItemTexture(string id)
        {
            var data = GetItem(id);
            return data == null ? null : data.Sprite.texture;
        }

        public Sprite GetItemSprite(string id)
        {
            var data = GetItem(id);
            return data == null ? null : data.Sprite;
        }
        
        #endregion Item Details
        
        #region Item Behavior

        public void ItemCollected(ItemScriptable itemScriptable)
        {
            if (itemScriptable.IsStackable)
            {
                var item = PlayerItems.FirstOrDefault(playerItem =>
                    playerItem.Id == itemScriptable.Id &&
                    (playerItem.Quantity + itemScriptable.Amount) < itemScriptable.MaxStack);
                
                //if above returns no null it means we already have an item that is stackable and hasn't reached the max stack
                if (item != null)
                {
                    item.Add(itemScriptable.Amount);
                    
                    //check if we need to create a new stack
                    if (item.Quantity > itemScriptable.MaxStack)
                    {
                        var remainder = item.Quantity - itemScriptable.MaxStack;
                        item.SetQuantity(itemScriptable.MaxStack);
                        
                        //create new stack
                        var newStack = new Item(itemScriptable);
                        newStack.SetQuantity(remainder);
                        PlayerItems.Add(newStack);
                    }
                    OnItemCollected?.Invoke(itemScriptable.Type, itemScriptable);
                    return;
                }
            }

            var weapon = itemScriptable as WeaponSettings;
            if (weapon)
            {
                //if weapon we need to also collect the ammo
                var ammo = weapon.InitialAmmo;
                if(ammo != null)
                    ItemCollected(ammo);
            }
            
            PlayerItems.Add(new Item(itemScriptable));
            OnItemCollected?.Invoke(itemScriptable.Type, itemScriptable);
        }
        #endregion Item Behavior
    }
}