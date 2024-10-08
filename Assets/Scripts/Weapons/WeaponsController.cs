using System;
using System.Linq;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class WeaponsController : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        
        private WeaponSettings[] weaponSettings;
        private EquippedWeaponManager weaponManager;
        private GameObject[] weaponModels;

        private void Awake()
        {
            weaponSettings = Resources.LoadAll<WeaponSettings>("Weapons");
            this.weaponManager = new EquippedWeaponManager(weaponSettings.Length);
            for (var i = 0; i < weaponSettings.Length; i++)
            {
                var weapon = weaponSettings[i];
                weapon.Spawn(transform, this);
                this.weaponManager.AddGun(i, weapon);
                var slotIndex = i;
                input.actions[$"Weapon {slotIndex + 1}"].performed += (ctx) => EquipWeapon(slotIndex);
            }
            EquipWeapon(0);
        }

        private void EquipWeapon(int slot)
        {

            weaponManager.SwapToSlot(slot);
        }
    }
}