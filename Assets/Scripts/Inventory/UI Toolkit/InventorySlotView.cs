using System;
using System.Collections;
using System.Numerics;
using Extensions;
using Inventory.Items;
using UnityEngine.UIElements;

namespace Inventory.UI_Toolkit
{
    public class InventorySlotView : VisualElement
    {
        public Item Item { get; private set; }
        public int Quantity => Item.Quantity;
        public event Action<Vector2, Item> OnStartDrag = delegate { };
        
        private InventoryManager InventoryManager => InventoryManager.Instance;
        private Label label;
        private Image Icon;

        public InventorySlotView(ref Item item)
        {
            Item = item;
            this.AddClass("item-slot");
            InitializeView();
        }

        private void InitializeView()
        {
            SetItemImage();
            SetItemLabel();
        }

        private void SetItemLabel()
        {
            label ??= this.AddChild<Label>("item-label");
            label.text = $"{InventoryManager.GetItemName(Item.Id)} x{Quantity}";
        }

        private void SetItemImage()
        {
            Icon ??= this.AddChild<Image>("item-image");
            Icon.image = InventoryManager.GetItemTexture(Item.Id);
        }
        
        
        void OnPointerDown(PointerDownEvent evt) {
            // if (evt.button != 0 || string.IsNullOrEmpty(Item.Id)) return;
            //
            // OnStartDrag.Invoke(evt.position, this);
            // evt.StopPropagation();
        }
    }
}