using Inventory.Items;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "DarkSky/Ammo Settings")]
    public class AmmoSettings : ItemScriptable
    {
        [SerializeField] private WeaponSettings weapon;
        public WeaponSettings Weapon => weapon;

        public void Collected()
        {
            weapon.AmmoHandler.AddAmmo(Amount);
        }
    }
}