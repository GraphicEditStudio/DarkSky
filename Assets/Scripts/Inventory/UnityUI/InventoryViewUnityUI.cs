using System;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Weapons;

namespace Inventory.UnityUI
{
    public class InventoryViewUnityUI : MonoBehaviour, IInventoryView
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private GameObject uiInventoryView;
        [SerializeField] private GameObject uiSlotPrefab;
        [SerializeField] private Transform uiSlotContainer;

        private List<InventoryItemSlotUI> inventorySlots;
        public InputActionReference toggleInventoryAction;
        

        private InventoryManager InventoryManager => InventoryManager.Instance;
        public void Initialize()
        {
            if (toggleInventoryAction == null)
            {
                Debug.LogError($"Toggle Input Action is missing", gameObject);
                return;
            }

            inventorySlots = new List<InventoryItemSlotUI>();
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
            var data = InventoryManager.Instance.GetItem(item.Id);
            var slot = Instantiate(uiSlotPrefab, uiSlotContainer);
            var itemName = data.Name;
            slot.name = itemName;
            var label = slot.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
            var image = slot.GetComponentInChildren<Image>(true);
            
            var inventoryItemSlotUI = MakeSlot(data, slot, item, label, image);
            inventorySlots.Add(inventoryItemSlotUI);
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

        private InventoryItemSlotUI MakeSlot(ItemScriptable data, GameObject slot, Item item, TMPro.TextMeshProUGUI label, Image image)
        {
            var ammo = data as AmmoSettings;
            if (ammo)
                return new InventoryAmmoSlotUI(slot, item, ammo.Weapon, label, image);

            return new InventoryItemSlotUI(slot, item, label, image);
        }
    }
}