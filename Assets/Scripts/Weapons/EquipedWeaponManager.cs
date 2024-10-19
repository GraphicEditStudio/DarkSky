using Inventory;
using Inventory.Items;
using Items;
using JetBrains.Annotations;

namespace Weapons
{
    public class EquippedWeaponManager
    {
        private int _equippedSlot = -1;
        private int weaponSlotsMaxIndex = 0;
        private readonly WeaponSettings[] _weaponSlots;

        public EquippedWeaponManager(int numSlots)
        {
            _weaponSlots = new WeaponSettings[numSlots];
            for (var i = 0; i < numSlots; i++)
            {
                _weaponSlots[i] = null;
            }
            InventoryManager.Instance.OnItemCollected += OnWeaponCollected;
        }

        ~EquippedWeaponManager()
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.OnItemCollected -= OnWeaponCollected;
            }
        }
        
        public void AddGun(int slot, [CanBeNull] WeaponSettings gun)
        {
            _weaponSlots[slot] = gun;
        }

        public void AddGun([CanBeNull] WeaponSettings gun)
        {
            _weaponSlots[weaponSlotsMaxIndex++] = gun;
        }

        public WeaponSettings GetCurrentGun()
        {
            var slot = _equippedSlot;
            return GetWeaponAtSlot(slot);
        }

        private WeaponSettings GetWeaponAtSlot(int slot)
        {
            try
            {
                return _weaponSlots[slot];
            }
            catch
            {
                return null;
            }
        }

        public WeaponSettings SwapToSlot(int slot)
        {
            
            _equippedSlot = slot;
            DisableAllWeaponModels();
            var weaponSettings = GetWeaponAtSlot(slot);
            if (weaponSettings == null)
                return null;
            
            weaponSettings.EnableModel();
            return weaponSettings;
        }

        private void DisableAllWeaponModels()
        {
            foreach (var weapon in _weaponSlots)
            {
                if (weapon == null) continue;
                weapon.DisableModel();
            }
        }

        public WeaponSettings[] GetSlots()
        {
            return _weaponSlots;
        }

        public int GetEquippedSlot()
        {
            return _equippedSlot;
        }

        public void OnWeaponCollected(EItemType type, ItemScriptable data)
        {
            if (type != EItemType.Weapon)
                return;

            var weaponData = data as WeaponSettings;
            if (weaponData != null)
            {
                AddGun(weaponData);
            }
        }
    }
}