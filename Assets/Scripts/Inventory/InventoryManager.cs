using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Inventory.Items;
using UnityEngine;
using Utils;

namespace Inventory
{
    public sealed class InventoryManager : MonoSingleton<InventoryManager>
    {
        [Tooltip("Has to be an object with an IInventory script attached")]
        [SerializeField]
        private GameObject viewReference;
        private IInventoryView view;
        
        public List<ItemScriptable> itemsData;
        public ObservableList<Item> PlayerItems { get; private set; }
        
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
            
            //testing only
            foreach (var data in itemsData)
            {
                PlayerItems.Add(new Item(data));
            }
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
        public string GetItemName(string id)
        {
            var data = itemsData.FirstOrDefault(d => d.Id == id);
            return data == null ? "" : data.Name;
        }

        public string GetItemDescription(string id)
        {
            var data = itemsData.FirstOrDefault(d => d.Id == id);
            return data == null ? "" : data.Description;
        }

        public Texture GetItemTexture(string id)
        {
            var data = itemsData.FirstOrDefault(d => d.Id == id);
            return data == null ? null : data.Sprite.texture;
        }

        public Sprite GetItemSprite(string id)
        {
            var data = itemsData.FirstOrDefault(d => d.Id == id);
            return data == null ? null : data.Sprite;
        }
        
        #endregion Item Details
    }
}