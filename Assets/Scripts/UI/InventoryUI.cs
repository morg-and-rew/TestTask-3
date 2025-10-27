using InventorySystem.Model;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Controller;
using InventorySystem.Core;

namespace InventorySystem.UI
{
    public class InventoryUI : MonoBehaviour, IInventoryUI
    {
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _slotsParent;

        private IInventoryManager _inventoryManager;
        private InventoryController _controller;
        private readonly List<InventorySlot> _slots = new List<InventorySlot>();

        public void Initialize(IInventoryManager inventoryManager, InventoryController controller, IItemInfoPanel infoPanel)
        {
            _inventoryManager = inventoryManager;
            _controller = controller;

            _inventoryManager.OnInventoryChanged += RefreshUI;

            GenerateSlots(infoPanel);
            RefreshUI();
        }

        private void GenerateSlots(IItemInfoPanel infoPanel)
        {
            foreach (Transform child in _slotsParent)
                Destroy(child.gameObject);

            _slots.Clear();
            List<InventoryItem> items = (List<InventoryItem>)_inventoryManager.GetItems();

            for (int i = 0; i < items.Count; i++)
            {
                GameObject slotGO = Instantiate(_slotPrefab, _slotsParent);
                InventorySlot slot = slotGO.GetComponent<InventorySlot>();
                slot.BindController(_controller);
                slot.BindInfoPanel(infoPanel);
                slot.Initialize(i);
                _slots.Add(slot);
            }
        }


        public void RefreshUI()
        {
            if (_inventoryManager == null) return;

            List<InventoryItem> items = new List<InventoryItem>(_inventoryManager.GetItems());

            for (int i = 0; i < _slots.Count; i++)
            {
                InventoryItem currentItem = i < items.Count ? items[i] : null;
                _slots[i].SetItem(currentItem);
            }
        }
    }
}
