using System;
using Inventory.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Inventory.UnityUI
{
    public class InventoryAmmoSlotUI : InventoryItemSlotUI
    {
        private WeaponSettings weaponReference;
        
        public InventoryAmmoSlotUI(GameObject referenceGameObject, Item item, WeaponSettings weapon,
            TextMeshProUGUI label, Image image) : base(referenceGameObject, item, label, image)
        {
            weaponReference = weapon;
            weaponReference.AmmoHandler.OnUpdate += UpdateItemAmount;
        }

        ~InventoryAmmoSlotUI()
        {
            if (weaponReference != null && weaponReference.AmmoHandler != null)
            {
                weaponReference.AmmoHandler.OnUpdate -= UpdateItemAmount;
            }

        }
        private void UpdateItemAmount(int clipSize, int ammoSize, int inClip, int ammo)
        {
            itemReference.SetQuantity(inClip + ammo);
        }
    }
}