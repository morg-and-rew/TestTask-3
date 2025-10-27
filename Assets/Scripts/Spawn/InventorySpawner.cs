using InventorySystem.Controller;
using InventorySystem.Core;
using InventorySystem.Model;
using InventorySystem.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventorySpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Inventory _manager;
        [SerializeField] private InventoryController _controller;
        [SerializeField] private Button _addRandomButton;
        [SerializeField] private List<ItemDefinition> _possibleItems;

        [Header("Settings")]
        [SerializeField] private int _minAmount = 1;
        [SerializeField] private int _maxAmount = 5;

        private void Awake()
        {
            if (_manager == null)
                return;

            if (_addRandomButton != null)
                _addRandomButton.onClick.AddListener(AddRandomItem);
        }

        private void Start()
        {
            if (_possibleItems != null && _possibleItems.Count >= 2)
            {
                _manager.AddItemDirect(new InventoryItem(_possibleItems[0], 2), 0);
                _manager.AddItemDirect(new InventoryItem(_possibleItems[1], 1), 1);
                _manager.AddItemDirect(new InventoryItem(_possibleItems[0], 3), 2);
            }
        }

        private void AddRandomItem()
        {
            if (_possibleItems == null || _possibleItems.Count == 0)
            {
                return;
            }

            int randomIndex = Random.Range(0, _possibleItems.Count);
            ItemDefinition randomDef = _possibleItems[randomIndex];
            int amount = randomDef.isStackable ? Random.Range(_minAmount, _maxAmount + 1) : 1;

            List<InventoryItem> items = (List<InventoryItem>)_manager.GetItems();
            int emptySlotIndex = items.FindIndex(i => i == null);

            if (emptySlotIndex >= 0)
            {
                _manager.AddItemDirect(new InventoryItem(randomDef, amount), emptySlotIndex);
            }
            else
            {
                return;
            }
        }
    }
}
