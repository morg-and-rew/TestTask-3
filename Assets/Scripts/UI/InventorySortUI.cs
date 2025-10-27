using InventorySystem.Core;
using InventorySystem.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventorySortUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventorySorter _sorter;
        [SerializeField] private Button _sortByNameButton;
        [SerializeField] private Button _sortByTypeButton;
        [SerializeField] private Button _toggleOrderButton;

        private void Start()
        {
            if (_sortByNameButton != null)
                _sortByNameButton.onClick.AddListener(() => _sorter.Sort(InventorySorter.SortMode.ByName));

            if (_sortByTypeButton != null)
                _sortByTypeButton.onClick.AddListener(() => _sorter.Sort(InventorySorter.SortMode.ByType));

            if (_toggleOrderButton != null)
                _toggleOrderButton.onClick.AddListener(() => _sorter.ToggleOrder());
        }
    }
}