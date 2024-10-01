using JetBrains.Annotations;

namespace Weapons
{
    public class EquippedWeaponManager
    {
        private int _equippedSlot = 0;
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

        public WeaponSettings GetCurrentGun()
        {
            var slot = _equippedSlot;
            return GetGunAtSlot(slot);
        }

        private WeaponSettings GetGunAtSlot(int slot)
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
            return GetGunAtSlot(slot);
        }

        public WeaponSettings[] GetSlots()
        {
            return _weaponSlots;
        }

        public int GetEquippedSlot()
        {
            return _equippedSlot;
        }
    }
}