using System;
using UnityEngine;

namespace Inventory.Items
{
    public class ItemBehavior : MonoBehaviour
    {
        [SerializeField] private ItemScriptable itemScriptable;

        private bool collected;

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (collected)
                return;

            collected = true;
            InventoryManager.Instance.ItemCollected(itemScriptable);
            Destroy(gameObject);
        }
    }
}