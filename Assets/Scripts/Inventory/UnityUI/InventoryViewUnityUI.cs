using System;
using Inventory.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory.UnityUI
{
    public class InventoryViewUnityUI : MonoBehaviour, IInventoryView
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private GameObject uiInventoryView;
        [SerializeField] private GameObject uiSlotPrefab;
        [SerializeField] private Transform uiSlotContainer;

        public InputActionReference toggleInventoryAction;

        private InventoryManager InventoryManager => InventoryManager.Instance;
        public void Initialize()
        {
            if (toggleInventoryAction == null)
            {
                Debug.LogError($"Toggle Input Action is missing", gameObject);
                return;
            }
        }

        public void OnEnable()
        {
            if (toggleInventoryAction != null)
            {
                toggleInventoryAction.action.Enable();
                toggleInventoryAction.action.performed += OnToggleInventoryPerformed;
            }
           
        }
        
        public void OnDisable()
        {
            if (toggleInventoryAction)
            {
                toggleInventoryAction.action.Disable();
                toggleInventoryAction.action.performed -= OnToggleInventoryPerformed;
            }
        }
        public void ItemAdded(Item item)
        {
            var slot = Instantiate(uiSlotPrefab, uiSlotContainer);
            var itemName = InventoryManager.GetItemName(item.Id);
            slot.name = itemName;
            var label = slot.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
            label.text = itemName;
            var image = slot.GetComponentInChildren<Image>(true);
            image.overrideSprite = InventoryManager.GetItemSprite(item.Id);
            slot.SetActive(true);
           
        }
        public void ItemRemoved(Item item)
        {
            throw new System.NotImplementedException();
        }
        public void ItemListCleared()
        {
            throw new System.NotImplementedException();
        }
        
        private void OnToggleInventoryPerformed(InputAction.CallbackContext ctx)
        {
            uiInventoryView.SetActive(!uiInventoryView.activeSelf);
        }
    }
}