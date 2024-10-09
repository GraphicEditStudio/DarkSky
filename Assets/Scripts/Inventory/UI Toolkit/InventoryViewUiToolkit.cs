using System.Collections;
using Extensions;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory.UI_Toolkit
{
    public class InventoryViewUiToolkit : MonoBehaviour, IInventoryView
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private StyleSheet styleSheet;
        [SerializeField] private string containerName = "Inventory";

        private VisualElement root;
        private VisualElement container;
        private VisualElement slotsFrame;
        private VisualElement slotDetails;
        private VisualElement ghostIcon;

        static bool isDragging;
        private InventorySlotView originalItem;

        private bool IsInitialized = false;

        void Awake()
        {
        }

        public virtual void Initialize()
        {
            if (IsInitialized)
                return;

            root = uiDocument.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(styleSheet);

            container = root.AddChild("container");
            var inventory = container.AddChild("inventory");
            var inventoryFrame = inventory.AddChild("inventory-frame");

            var header = inventoryFrame.AddChild("header");
            var headerLabel = header.AddChild<Label>("header-label");
            headerLabel.text = containerName;

            var slotsContainer = inventoryFrame.AddChild("slots-container");

            slotsFrame = slotsContainer.AddChild("slots-frame");
            slotDetails = slotsContainer.AddChild("slot-details");

            IsInitialized = true;
        }

        public void ItemAdded(Item item)
        {
            var slot = new InventorySlotView(ref item);
            slot.AddTo(slotsFrame);
        }
        public void ItemRemoved(Item item)
        {
            throw new System.NotImplementedException();
        }
        public void ItemListCleared()
        {
            throw new System.NotImplementedException();
        }

        // void OnPointerDown(Vector2 position, InventorySlotView slot)
        // {
        //     isDragging = true;
        //     originalItem = slot;
        //
        //     SetGhostIconPosition(position);
        //
        //     ghostIcon.style.backgroundImage = originalItem.texture;
        //     originalItem.Icon.image = null;
        //     originalItem..visible = false;
        //
        //     ghostIcon.style.visibility = Visibility.Visible;
        //     // TODO show stack size on ghost icon
        // }
        //
        // void OnPointerMove(PointerMoveEvent evt) {
        //     if (!isDragging) return;
        //     
        //     SetGhostIconPosition(evt.position);
        // }
        //
        // void OnPointerUp(PointerUpEvent evt) {
        //     if (!isDragging) return;
        //     Slot closestSlot = Slots
        //         .Where(slot => slot.worldBound.Overlaps(ghostIcon.worldBound))
        //         .OrderBy(slot => Vector2.Distance(slot.worldBound.position, ghostIcon.worldBound.position))
        //         .FirstOrDefault();
        //
        //     if (closestSlot != null) {
        //         OnDrop?.Invoke(originalSlot, closestSlot);
        //     } else {
        //         originalSlot.Icon.image = originalSlot.BaseSprite.texture; 
        //     }
        //     
        //     isDragging = false;
        //     originalSlot = null;
        //     ghostIcon.style.visibility = Visibility.Hidden;
        // }
        //
        // static void SetGhostIconPosition(Vector2 position) {
        //     ghostIcon.style.top = position.y - ghostIcon.layout.height / 2;
        //     ghostIcon.style.left = position.x - ghostIcon.layout.width / 2;
        // }
    }
}