using Inventory.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory.UnityUI
{
    public class InventoryViewUnityUI : MonoBehaviour, IInventoryView
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private GameObject uiSlotPrefab;
        [SerializeField] private Transform uiSlotContainer;

        public InputActionReference toggleInventoryAction;

        private InventoryManager InventoryManager => InventoryManager.Instance;
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
        public void ItemAdded(Item item)
        {
            var slot = Instantiate(uiSlotPrefab, uiSlotContainer);
            var label = slot.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            label.text = InventoryManager.GetItemName(item.Id);
            var image = slot.GetComponentInChildren<Image>();
            image.sprite = InventoryManager.GetItemSprite(item.Id);
        }
        public void ItemRemoved(Item item)
        {
            throw new System.NotImplementedException();
        }
        public void ItemListCleared()
        {
            throw new System.NotImplementedException();
        }
    }
}