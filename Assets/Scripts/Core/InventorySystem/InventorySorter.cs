using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InventorySystem.Model;
using InventorySystem.Core;

namespace InventorySystem.Utility
{
    [DisallowMultipleComponent]
    public class InventorySorter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _inventoryManagerReference; 
        [SerializeField] private bool _descendingOrder;

        private IInventoryManager _inventoryManager;

        public enum SortMode
        {
            ByName,
            ByType,
            ByTypeThenName
        }

        public SortMode CurrentMode { get; private set; } = SortMode.ByName;
        public event Action OnSorted;

        private void Awake()
        {
            if (_inventoryManagerReference is IInventoryManager manager)
            {
                _inventoryManager = manager;
            }
            else
            {
                _inventoryManager = GetComponent<IInventoryManager>();
            }

            if (_inventoryManager == null)
                return;
        }

        public void Sort(SortMode mode)
        {
            if (_inventoryManager == null)
                return;

            List<InventoryItem> items = new List<InventoryItem>(_inventoryManager.GetItems());
            if (items.Count == 0)
                return;

            IEnumerable<InventoryItem> sorted = mode switch
            {
                SortMode.ByName => items.Where(i => i != null)
                                        .OrderBy(i => i.Definition.itemName, StringComparer.OrdinalIgnoreCase),
                SortMode.ByType => items.Where(i => i != null)
                                        .OrderBy(i => i.Definition.itemType.ToString()),
                SortMode.ByTypeThenName => items.Where(i => i != null)
                                                .OrderBy(i => i.Definition.itemType.ToString())
                                                .ThenBy(i => i.Definition.itemName, StringComparer.OrdinalIgnoreCase),
                _ => items.Where(i => i != null)
            };

            List<InventoryItem> result = _descendingOrder ? sorted.Reverse().ToList() : sorted.ToList();

            while (result.Count < items.Count)
                result.Add(null);

            _inventoryManager.ClearInventory();

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] != null)
                    _inventoryManager.AddItemDirect(result[i], i);
            }

            CurrentMode = mode;
            OnSorted?.Invoke();
        }


        public void ToggleOrder()
        {
            _descendingOrder = !_descendingOrder;
            Sort(CurrentMode);
        }
    }
}
