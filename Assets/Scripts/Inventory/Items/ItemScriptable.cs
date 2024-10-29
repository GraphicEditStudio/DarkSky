using Items;
using UnityEngine;
using Utils;

namespace Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Create", fileName = "New Item")]
    public class ItemScriptable : ScriptableId
    {
        [Header("Base Settings")] [SerializeField]
        protected EItemType itemType = EItemType.None;
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected string itemName = "";
        [SerializeField] protected string description = "";
        [SerializeField] protected bool isStackable = false;
        [SerializeField, Min(1)] protected int maxStack = 99;
        [SerializeField, Min(1)] protected int amount = 1;

        public EItemType Type => itemType;
        public string Name => itemName;
        public Sprite Sprite => sprite;
        public GameObject Prefab => prefab;
        public string Description => description;
        public bool IsStackable => isStackable;
        public int MaxStack => maxStack;
        public int Amount => amount;
    }
}