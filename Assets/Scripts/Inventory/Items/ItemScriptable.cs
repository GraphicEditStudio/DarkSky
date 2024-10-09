using Items;
using UnityEngine;
using Utils;

namespace Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Create", fileName = "New Item")]
    public class ItemScriptable : ScriptableId
    {
        [SerializeField]
        private EItemType itemType = EItemType.None;

        [SerializeField] private Sprite sprite;
        [SerializeField]
        private string itemName = "";
        [SerializeField]
        private string description = "";
        [SerializeField]
        private bool isStackable = false;

        public EItemType Type => itemType;
        public string Name => itemName;
        public Sprite Sprite => sprite;
        public string Description => description;
        public bool IsStackable => isStackable;
    }
}