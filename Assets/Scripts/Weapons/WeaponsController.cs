using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Weapons
{
    public class WeaponsController : MonoBehaviour
    {
        public event Action<int, int, int, int> OnAmmoUpdate;
        
        [SerializeField] private PlayerInput input;
        [SerializeField] private GameObject defaultImpact;

        [Header("Sway")] 
        [SerializeField] private Transform targetTransform;
        private Vector3 _offset;
        [SerializeField] private float intensity;
        [SerializeField] private float intensityX;
        [SerializeField] private float effectiveSpeed;
        [SerializeField] private float sprintEffectiveSpeed;
        private Vector3 _originalOffset;
        private float _sinTime;

        [Header("Rifle Follow")]
        [SerializeField] private float swayMultiplier;
        [SerializeField] private float smooth;
        
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
            _originalOffset = _offset;
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
            else
            {
                if (input.actions["Reload"].WasPressedThisFrame())
                {
                    var currentGun = weaponManager.GetCurrentGun();
                    if (currentGun && currentGun.AmmoHandler.ReloadAllowed())
                    {
                        currentGun.AmmoHandler.Reload();
                    }
                }
            }

            // Look sway
            var mouseRaw = input.actions["Look"].ReadValue<Vector2>();
            Quaternion rotationX = Quaternion.AngleAxis(-(mouseRaw.y * swayMultiplier), Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseRaw.x * swayMultiplier, Vector3.up);
            Quaternion targetRotation = rotationX * rotationY;
            transform.localRotation =
                Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

            
            
            var moveRaw = input.actions["Move"].ReadValue<Vector2>();
            var inputVector = new Vector3(moveRaw.y, 0f, moveRaw.x);
            var speed = input.actions["Sprint"].IsPressed() ? sprintEffectiveSpeed : effectiveSpeed;
            if (inputVector.magnitude > 0f)
            {
                _sinTime += Time.deltaTime * speed;
            }
            else
            {
                _sinTime = 0f;
            }
            
            float sinAmountY = -Mathf.Abs((intensity * Mathf.Sin(_sinTime)));
            Vector3 sinAmountX = transform.right * intensity * MathF.Cos(_sinTime) * intensityX;
            _offset = new Vector3
            {
                x = _originalOffset.x,
                y = _originalOffset.y + sinAmountY,
                z = _originalOffset.z
            };
            _offset += sinAmountX;
            
            transform.position = targetTransform.position + _offset;
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