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
            this.weaponModels = new GameObject[weaponSettings.Length];
            var i = 0;
            this.weaponManager = new EquippedWeaponManager(weaponSettings.Length);
            foreach (var weapon in weaponSettings)
            {
                var weaponInstance = Instantiate(weapon.Model);
                var instanceTransform = weaponInstance.transform;
                instanceTransform.SetParent(transform);
                instanceTransform.localPosition = weapon.PositionOffset;
                instanceTransform.localRotation = Quaternion.Euler(weapon.RotationOffset);
                weaponInstance.gameObject.SetActive(false);
                weaponModels[i] = weaponInstance;
                this.weaponManager.AddGun(i, weapon);
                i++;
            }

            EquipWeapon(0);
            for (var weaponSlot = 0; weaponSlot < weaponSettings.Length; weaponSlot++)
            {
                var slotIndex = weaponSlot;
                input.actions[$"Weapon {weaponSlot + 1}"].performed += (ctx) => EquipWeapon(slotIndex);
            }
        }

        private void EquipWeapon(int slot)
        {
            if (weaponModels[slot])
            {
                foreach (var weaponModel in weaponModels)
                {
                    weaponModel.SetActive(false);
                }
                weaponModels[slot].SetActive(true);
                weaponManager.SwapToSlot(slot);    
            }
        }
    }
}