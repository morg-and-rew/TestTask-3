using InventorySystem.Model;
using InventorySystem.ScriptableObjects;
using System;
using System.Collections.Generic;

namespace InventorySystem.Core
{
    public interface IInventoryManager
    {
        event Action OnInventoryChanged;
        IReadOnlyList<InventoryItem> GetItems();
        bool AddItem(ItemDefinition definition, int amount = 1);
        bool AddItemDirect(InventoryItem item, int slotIndex);
        void RemoveItemAtSlot(int index);
        void SwapItems(int indexA, int indexB);
        bool TryStackItems(int fromIndex, int toIndex);
        void ClearInventory();
        void NotifyChanged(); 
    }
}
