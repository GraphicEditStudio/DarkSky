using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class WeaponsController : MonoBehaviour
    {
        public event Action<int, int, int, int> OnAmmoUpdate;
        
        [SerializeField] private PlayerInput input;
        [SerializeField] private GameObject defaultImpact;

        private WeaponSettings[] weaponSettings;
        private EquippedWeaponManager weaponManager;
        private GameObject[] weaponModels;
        private bool isFiring;

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
            isFiring = false;
            input.actions["Shoot"].performed += (ctx) => IsFiring(true);
            input.actions["Shoot"].canceled += (ctx) => IsFiring(false);
        }

        private void Update()
        {
            if (isFiring)
            {
                var currentGun = weaponManager.GetCurrentGun();
                if (!currentGun) return;
                
                (bool didShoot, IEnumerable<(RaycastHit? CastHit, Vector3 HitPoint)> hits) = currentGun.Shoot();
                
                if (!currentGun.AutoFire)
                {
                    isFiring = false;
                }
                
                if (didShoot)
                {
                    foreach ((RaycastHit? CastHit, Vector3 HitPoint) hit in hits)
                    {
                        if (hit.CastHit.HasValue)
                        {
                            var castHit = hit.CastHit.Value;

                            var effects = defaultImpact;
                            var hitbox = castHit.transform.GetComponent<Hitbox>();
                            if (hitbox)
                            {
                                hitbox.OnHit(currentGun.Damage, castHit);
                            }
                            else
                            {
                                var instance = Instantiate(effects);
                                instance.transform.position = hit.HitPoint;
                                instance.transform.forward = castHit.normal;    
                            }
                        }
                    }
                }
            }
        }

        private void IsFiring(bool isFiring)
        {
            this.isFiring = isFiring;
        }

        private void EquipWeapon(int slot)
        {
            if (slot == weaponManager.GetEquippedSlot())
                return;
            
            var currentWeapon = weaponManager.GetCurrentGun();
            if (currentWeapon)
            {
                currentWeapon.AmmoHandler.OnUpdate -= OnAmmoManagerUpdate;
            }
            var weaponEquipped = weaponManager.SwapToSlot(slot);
            if (weaponEquipped)
            {
                weaponEquipped.AmmoHandler.OnUpdate += OnAmmoManagerUpdate;
                weaponEquipped.AmmoHandler.ManualUpdate();
            }
            else
            {
                OnAmmoUpdate?.Invoke(0, 0, 0, 0);
            }
        }

        private void OnAmmoManagerUpdate(int clipSize, int ammoSize, int inClip, int ammo)
        {
            OnAmmoUpdate?.Invoke(clipSize, ammoSize, inClip, ammo);
        }
    }
}