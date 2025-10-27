using UnityEngine;
using InventorySystem.ScriptableObjects;

namespace InventorySystem.Model
{
    [System.Serializable]
    public class InventoryItem
    {
        [field: SerializeField] public ItemDefinition Definition { get;  set; }
        [field: SerializeField] public int Quantity { get;  set; }

        public bool IsStackable => Definition != null && Definition.isStackable;

        public InventoryItem(ItemDefinition definition, int quantity)
        {
            Definition = definition;
            Quantity = quantity;
        }
    }
}
