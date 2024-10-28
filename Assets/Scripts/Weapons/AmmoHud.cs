using System;
using TMPro;
using UnityEngine;

namespace Weapons
{
    public class AmmoHud : MonoBehaviour
    {
        [SerializeField] private GameObject AllAmmoText;
        [SerializeField] private TextMeshProUGUI ClipText;
        [SerializeField] private TextMeshProUGUI AmmoText;
        [SerializeField] private WeaponsController weaponsController;

        private void Awake()
        {
            weaponsController.OnAmmoUpdate += OnAmmoUpdate;
            AllAmmoText.SetActive(false);
        }

        private void OnDestroy()
        {
            weaponsController.OnAmmoUpdate -= OnAmmoUpdate;
        }

        private void OnAmmoUpdate(int clipSize, int ammoCapacity, int inClip, int ammo)
        {
            if (ammoCapacity == 0)
            {
                AllAmmoText.SetActive(false);
            }
            else
            {
                AllAmmoText.SetActive(true);
            }

            ClipText.text = inClip.ToString();
            AmmoText.text = ammo.ToString();
        }
    }
}