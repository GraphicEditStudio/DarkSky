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
        }

        public void AddGun(int slot, [CanBeNull] WeaponSettings gun)
        {
            _weaponSlots[slot] = gun;
        }

        public int AddGun([CanBeNull] WeaponSettings gun)
        {
            var slot = weaponSlotsMaxIndex;
            _weaponSlots[weaponSlotsMaxIndex++] = gun;
            return slot;
        }

        public WeaponSettings GetCurrentGun()
        {
            var slot = _equippedSlot;
            return GetWeaponAtSlot(slot);
        }

        public WeaponSettings GetWeaponAtSlot(int slot)
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

        private void AmmoCollected(AmmoSettings data)
        {
            data.Collected();
        }
    }
}