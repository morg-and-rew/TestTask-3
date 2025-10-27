using InventorySystem.Controller;
using InventorySystem.Core;
using InventorySystem.ScriptableObjects;
using InventorySystem.UI;
using System.Collections.Generic;
using UnityEngine;

public class Boostrap : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private Inventory _inventoryManager;

    [Header("Controller")]
    [SerializeField] private InventoryController _inventoryController;

    [Header("View")]
    [SerializeField] private InventoryUI _inventoryUI;

    [Header("Optional Info Panel")]
    [SerializeField] private MonoBehaviour _infoPanel; 
    [SerializeField] private List<ItemDefinition> allDefinitions;
    private IItemInfoPanel infoPanelInterface;

    private void Awake()
    {
        _inventoryManager.OnInventoryChanged += () =>
        {
            InventoryPreservation.SaveInventory(_inventoryManager);
        };

        if (_infoPanel != null)
            infoPanelInterface = _infoPanel as IItemInfoPanel;

        _inventoryManager.InitializeInventory();
        InventoryPreservation.LoadInventory(_inventoryManager, allDefinitions);

        _inventoryController.Initialize(_inventoryManager, _inventoryUI);

        _inventoryUI.Initialize(_inventoryManager, _inventoryController, infoPanelInterface);
    }
}
