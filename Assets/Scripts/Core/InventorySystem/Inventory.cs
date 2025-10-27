using InventorySystem.Model;
using InventorySystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Core
{
    public class Inventory : MonoBehaviour, IInventoryManager
    {
        [SerializeField] private int _inventorySize = 20;
        private List<InventoryItem> _items;

        public event Action OnInventoryChanged;

        public void InitializeInventory()
        {
            _items = new List<InventoryItem>(_inventorySize);
            for (int i = 0; i < _inventorySize; i++)
                _items.Add(null);
        }

        public IReadOnlyList<InventoryItem> GetItems() => _items;

        public bool AddItem(ItemDefinition definition, int amount = 1)
        {
            if (definition == null || amount <= 0)
                return false;

            if (definition.isStackable)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    InventoryItem currentItem = _items[i];

                    if (currentItem == null || currentItem.Definition != definition)
                        continue;

                    int space = definition.maxStack - currentItem.Quantity;
                    int toAdd = Math.Min(space, amount);

                    currentItem.Quantity += toAdd;
                    amount -= toAdd;

                    if (amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();

                        return true;
                    }
                }
            }

            for (int i = 0; i < _items.Count && amount > 0; i++)
            {
                if (_items[i] != null) continue;

                int toTake = definition.isStackable ? Math.Min(definition.maxStack, amount) : 1;
                _items[i] = new InventoryItem(definition, toTake);
                amount -= toTake;
            }

            OnInventoryChanged?.Invoke();

            return amount <= 0;
        }

        public bool AddItemDirect(InventoryItem item, int slotIndex)
        {
            if (item == null || slotIndex < 0 || slotIndex >= _items.Count)
                return false;

            _items[slotIndex] = new InventoryItem(item.Definition, item.Quantity);
            OnInventoryChanged?.Invoke();

            return true;
        }

        public void RemoveItemAtSlot(int index)
        {
            if (index < 0 || index >= _items.Count)
                return;

            _items[index] = null;
            OnInventoryChanged?.Invoke();
        }

        public void SwapItems(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= _items.Count) return;
            if (indexB < 0 || indexB >= _items.Count) return;

            InventoryItem temp = _items[indexA];
            _items[indexA] = _items[indexB];
            _items[indexB] = temp;

            OnInventoryChanged?.Invoke();
        }

        public bool TryStackItems(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= _items.Count) return false;
            if (toIndex < 0 || toIndex >= _items.Count) return false;

            InventoryItem fromItem = _items[fromIndex];
            InventoryItem toItem = _items[toIndex];

            if (fromItem == null || toItem == null) return false;
            if (!fromItem.Definition.isStackable || fromItem.Definition != toItem.Definition) return false;

            int total = fromItem.Quantity + toItem.Quantity;
            int maxStack = fromItem.Definition.maxStack;

            if (total <= maxStack)
            {
                toItem.Quantity = total;
                _items[fromIndex] = null;
            }
            else
            {
                toItem.Quantity = maxStack;
                fromItem.Quantity = total - maxStack;
            }

            OnInventoryChanged?.Invoke();

            return true;
        }

        public void ClearInventory()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i] = null;

            OnInventoryChanged?.Invoke();
        }

        public void NotifyChanged()
        {
            OnInventoryChanged?.Invoke();
        }
    }
}
