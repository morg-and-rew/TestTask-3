using InventorySystem.Model;
using InventorySystem.UI;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Core;

namespace InventorySystem.Controller
{
    public class InventoryController : MonoBehaviour
    {
        private IInventoryManager _inventoryManager;
        private IInventoryUI _inventoryUI;

        public void Initialize(IInventoryManager inventoryManager, IInventoryUI inventoryUI)
        {
            _inventoryManager = inventoryManager;
            _inventoryUI = inventoryUI;

            if (_inventoryManager != null && _inventoryUI != null)
                _inventoryManager.OnInventoryChanged += _inventoryUI.RefreshUI;
        }

        private void OnDestroy()
        {
            if (_inventoryManager != null && _inventoryUI != null)
                _inventoryManager.OnInventoryChanged -= _inventoryUI.RefreshUI;
        }

        public void OnUseItem(int slotIndex)
        {
            IReadOnlyList<InventoryItem> items = _inventoryManager.GetItems();

            if (slotIndex < 0 || slotIndex >= items.Count) return;

            InventoryItem item = items[slotIndex];

            if (item == null || item.Definition == null) return;

            item.Definition.Use();

            if (item.Definition.isStackable)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                    _inventoryManager.RemoveItemAtSlot(slotIndex);
                else
                    _inventoryManager.NotifyChanged();
            }
            else
            {
                _inventoryManager.RemoveItemAtSlot(slotIndex);
            }
        }

        public void OnDropItemAtWorld(int slotIndex, Vector3 worldPosition)
        {
            IReadOnlyList<InventoryItem> items = _inventoryManager.GetItems();

            if (slotIndex < 0 || slotIndex >= items.Count) return;

            InventoryItem item = items[slotIndex];

            if (item == null) return;

            _inventoryManager.RemoveItemAtSlot(slotIndex);
        }

        public void OnSwapOrStack(int fromIndex, int toIndex)
        {
            if (!_inventoryManager.TryStackItems(fromIndex, toIndex))
                _inventoryManager.SwapItems(fromIndex, toIndex);
        }

        public void OnDropSingleItem(int slotIndex, Vector3 worldPosition)
        {
            IReadOnlyList<InventoryItem> items = _inventoryManager.GetItems();
            if (slotIndex < 0 || slotIndex >= items.Count)
                return;

            InventoryItem item = items[slotIndex];
            if (item == null || item.Definition == null)
                return;

            item.Quantity--;

            if (item.Quantity <= 0)
                _inventoryManager.RemoveItemAtSlot(slotIndex);
            else
                _inventoryManager.NotifyChanged(); 
        }

    }
}
