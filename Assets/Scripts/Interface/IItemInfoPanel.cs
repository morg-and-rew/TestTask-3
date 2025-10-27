using InventorySystem.ScriptableObjects;
using UnityEngine;

public interface IItemInfoPanel
{
    void Show(ItemDefinition item);
    void Hide();
}
