using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UnityUI
{
    public class InventoryItemSlotUI
    {
        protected GameObject uiGameObjectReference;
        protected Item itemReference;
        protected TMPro.TextMeshProUGUI labelReference;
        protected Image imageReference;

        public InventoryItemSlotUI(GameObject referenceGameObject, Item item, TMPro.TextMeshProUGUI label, Image image)
        {
            uiGameObjectReference = referenceGameObject;
            itemReference = item;
            labelReference = label;
            imageReference = image;

            SetItemName();
            SetImage();
            
            referenceGameObject.SetActive(true);
            itemReference.ItemUpdatedEvent += OnItemUpdated;
        }

        ~InventoryItemSlotUI()
        {
            if (itemReference != null)
            {
                itemReference.ItemUpdatedEvent -= OnItemUpdated;
            }
        }

        private void SetItemName()
        {
            if (labelReference == null || itemReference == null)
            {
                return;
            }

            var name = InventoryManager.Instance.GetItemName(itemReference.Id);
            labelReference.text = itemReference.Stackable ? $"{name} x{itemReference.Quantity}" : $"{name}";
        }

        private void SetImage()
        {
            if (imageReference == null || itemReference == null)
            {
                return;
            }

            var image = InventoryManager.Instance.GetItemSprite(itemReference.Id);
            imageReference.overrideSprite = image;
        }

        private void OnItemUpdated(Item item)
        {
            if (item != itemReference)
                return;
            
            SetItemName();
            SetImage();
        }
    }
}