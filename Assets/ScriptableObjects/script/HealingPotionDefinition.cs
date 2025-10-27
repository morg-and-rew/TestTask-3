using UnityEngine;

namespace InventorySystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory/Items/Healing Potion")]
    public class HealingPotionDefinition : ItemDefinition
    {
        public int healAmount = 20;

        public override void Use()
        {
            Debug.Log($"Выпито зелье! Восстановлено {healAmount} HP.");
        }
    }
}
