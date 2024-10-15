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
                if (currentGun)
                {
                    IEnumerable<(RaycastHit? CastHit, Vector3 HitPoint)> hits = currentGun.Shoot();
                    if (!currentGun.AutoFire)
                    {
                        isFiring = false;
                    }

                    if (hits.Any())
                    {
                        foreach ((RaycastHit? CastHit, Vector3 HitPoint) hit in hits)
                        {
                            if (hit.CastHit.HasValue)
                            {
                                var castHit = hit.CastHit.Value;
                                //Debug.Log($"Hit {castHit.transform.gameObject.name}");
                                var instance = Instantiate(defaultImpact);
                                instance.transform.position = hit.HitPoint;
                                instance.transform.forward = castHit.normal;
                            }
                            else
                            {
                                
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

            weaponManager.SwapToSlot(slot);
        }
    }
}