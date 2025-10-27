using InventorySystem.Model;
using InventorySystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace InventorySystem.Core
{
    [Serializable]
    public class InventorySaveData
    {
        public List<InventorySlotData> slots = new List<InventorySlotData>();
    }

    [Serializable]
    public class InventorySlotData
    {
        public string itemName;
        public int quantity;
    }

    public static class InventoryPreservation
    {
        private static string _SavePath => Path.Combine(Application.persistentDataPath, "inventory.json");

        public static void SaveInventory(IInventoryManager manager)
        {
            if (manager == null)
            {
                return;
            }

            IReadOnlyList<InventoryItem> items = manager.GetItems();
            InventorySaveData data = new InventorySaveData();

            foreach (InventoryItem item in items)
            {
                if (item == null || item.Definition == null) continue;

                data.slots.Add(new InventorySlotData
                {
                    itemName = item.Definition.itemName,
                    quantity = item.Quantity
                });
            }

            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(_SavePath, json);
            }
            catch (Exception e)
            {
                return;
            }
        }

        public static void LoadInventory(IInventoryManager manager, List<ItemDefinition> allDefinitions)
        {
            if (manager == null || allDefinitions == null)
            {
                return;
            }

            if (!File.Exists(_SavePath))
            {
                manager.ClearInventory();

                return;
            }

            try
            {
                string json = File.ReadAllText(_SavePath);
                InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

                if (data == null)
                {
                    manager.ClearInventory();

                    return;
                }

                manager.ClearInventory();

                foreach (InventorySlotData slotData in data.slots)
                {
                    ItemDefinition def = allDefinitions.Find(d => d.itemName == slotData.itemName);
                    if (def != null)
                    {
                        manager.AddItem(def, slotData.quantity);
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
