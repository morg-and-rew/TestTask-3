using InventorySystem.Controller;
using InventorySystem.Model;
using InventorySystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI References")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantityText;
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _dropButton;

        private InventoryItem _currentItem;
        private int _slotIndex;
        private InventoryController _controller;
        private IItemInfoPanel _infoPanel;

        private Canvas _rootCanvas;
        private GameObject _dragIconGO;
        private Image _dragIconImage;

        private void Awake()
        {
            _rootCanvas = GetComponentInParent<Canvas>();

            if (_useButton != null) _useButton.onClick.AddListener(OnUseButtonClicked);

            if (_dropButton != null) _dropButton.onClick.AddListener(OnDropButtonClicked);
        }

        public void BindController(InventoryController ctrl) => _controller = ctrl;

        public void BindInfoPanel(IItemInfoPanel panel) => _infoPanel = panel;

        public void Initialize(int index) => _slotIndex = index;

        public void SetItem(InventoryItem item)
        {
            _currentItem = item;

            _icon.sprite = item?.Definition?.icon;
            _icon.enabled = item != null;

            _quantityText.text = (item != null && item.Definition.isStackable && item.Quantity > 1)
                ? item.Quantity.ToString()
                : "";

            _useButton.interactable = item != null;
            _dropButton.interactable = item != null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_currentItem == null || _rootCanvas == null) return;

            _dragIconGO = new GameObject("DragIcon");
            _dragIconGO.transform.SetParent(_rootCanvas.transform, false);
            _dragIconImage = _dragIconGO.AddComponent<Image>();
            _dragIconImage.sprite = _icon.sprite;
            _dragIconImage.raycastTarget = false;
            _dragIconGO.transform.localScale = Vector3.one;

            _icon.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragIconGO == null || _rootCanvas == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rootCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 pos);

            (_dragIconGO.transform as RectTransform).anchoredPosition = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragIconGO != null) Destroy(_dragIconGO);

            _icon.enabled = true;

            InventorySlot targetSlot = eventData.pointerCurrentRaycast.gameObject?
                .GetComponentInParent<InventorySlot>();

            if (targetSlot != null && targetSlot != this)
            {
                _controller?.OnSwapOrStack(_slotIndex, targetSlot.GetSlotIndex());
            }
            else
            {
                Vector3 worldPos = Vector3.zero;
                if (eventData.pressEventCamera != null)
                {
                    worldPos = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
                    worldPos.z = 0;
                }
                _controller?.OnDropItemAtWorld(_slotIndex, worldPos);
            }
        }

        private void OnUseButtonClicked()
        {
            _controller?.OnUseItem(_slotIndex);
        }

        private void OnDropButtonClicked()
        {
            if (_currentItem == null || _currentItem.Definition == null)
                return;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;

            // Выбрасываем один предмет
            _controller?.OnDropSingleItem(_slotIndex, worldPos);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            InventoryTooltip.Instance?.Show(_currentItem?.Definition, transform as RectTransform);
            _infoPanel?.Show(_currentItem?.Definition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryTooltip.Instance?.Hide();
            _infoPanel?.Hide();
        }

        public int GetSlotIndex() => _slotIndex;
    }
}
