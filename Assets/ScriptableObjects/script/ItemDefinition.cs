using UnityEngine;

namespace InventorySystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        public string itemName;
        [TextArea] public string description;
        public Sprite icon;
        public ItemType itemType;
        public bool isStackable;
        public int maxStack = 99;

        public virtual void Use()
        {
            Debug.Log($"Использован предмет: {itemName}");
        }
    }
}
