using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Inventory.Items
{
    public class ItemBehavior : MonoBehaviour
    {
        [SerializeField] private ItemScriptable itemScriptable;
        [SerializeField] private GameObject labelCanvas;
        [SerializeField] private bool showLabelOnFocus = true;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField, HideInInspector] private ItemScriptable currentItem;

        private bool collected;
        private Camera camera;
        void Awake()
        {
           camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (showLabelOnFocus && labelCanvas.activeSelf && camera != null) 
            {
                labelCanvas.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (collected)
                return;

            collected = true;
            InventoryManager.Instance.ItemCollected(itemScriptable);
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            if (itemScriptable == null || itemScriptable == currentItem)
                return;

            var childs = transform.GetComponentsInChildren<Transform>();
            var item = childs.FirstOrDefault(c => c.gameObject.name == currentItem.Name);

            if (item != null)
            {
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorApplication.delayCall += ()=>
                    {
                        DestroyImmediate(item.gameObject);
                    };
                }
                else
                {
                    Destroy(item.gameObject);
                }
               
            }
            
            var newItem = Instantiate(itemScriptable.Prefab, this.transform);
            newItem.name = itemScriptable.Name;

            foreach (var t in newItem.GetComponentsInChildren<Transform>())
            {
                t.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            
            currentItem = itemScriptable;

            if (label != null)
            {
                label.text = $"Press E to grab\n{currentItem?.Name}";
            }

        }
    }
}