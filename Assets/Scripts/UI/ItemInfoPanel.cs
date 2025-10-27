using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InventorySystem.ScriptableObjects;

namespace InventorySystem.UI
{
    public class ItemInfoPanel : MonoBehaviour, IItemInfoPanel
    {
        [Header("UI References")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake()
        {
            Hide();
        }

        public void Show(ItemDefinition item)
        {
            if (item == null) return;

            _iconImage.sprite = item.icon;
            _titleText.text = item.itemName;
            _typeText.text = item.itemType.ToString();
            _descriptionText.text = item.description;

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
