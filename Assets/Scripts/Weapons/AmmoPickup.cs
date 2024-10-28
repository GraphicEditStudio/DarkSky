using System;
using UnityEngine;

namespace Weapons
{
    public class AmmoPickup : MonoBehaviour
    {
        [SerializeField] private WeaponSettings weapon;
        [SerializeField] private int AmmoAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var left = weapon.AmmoHandler.AddAmmo(AmmoAmount);
            AmmoAmount = left;
            if (left == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}