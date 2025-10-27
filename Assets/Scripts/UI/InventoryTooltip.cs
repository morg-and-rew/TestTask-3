using UnityEngine;
using TMPro;
using UnityEngine.UI;
using InventorySystem.ScriptableObjects;

namespace InventorySystem.UI
{
    public class InventoryTooltip : MonoBehaviour
    {
        public static InventoryTooltip Instance;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Tooltip Offset")]
        [SerializeField] private Vector2 _offset = new Vector2(100f, 50f);

        private RectTransform _rectTransform;
        private Canvas _rootCanvas;

        private void Awake()
        {
            Instance = this;
            _rectTransform = GetComponent<RectTransform>();
            _rootCanvas = GetComponentInParent<Canvas>();

            Hide();
        }

        public void Show(ItemDefinition item, RectTransform slotTransform)
        {
            if (item == null) return;

            _titleText.text = item.itemName;
            _descriptionText.text = item.description;

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;

            UpdatePosition(slotTransform);
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void UpdatePosition(RectTransform slotTransform)
        {
            if (_rootCanvas == null || slotTransform == null)
                return;

            Vector3[] slotCorners = new Vector3[4];
            slotTransform.GetWorldCorners(slotCorners);

            Vector3 anchorPos = slotCorners[2] + (Vector3)_offset;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rootCanvas.transform as RectTransform,
                RectTransformUtility.WorldToScreenPoint(null, anchorPos),
                _rootCanvas.worldCamera,
                out Vector2 localPos
            );

            _rectTransform.anchoredPosition = localPos;

            ClampToScreen();
        }

        private void ClampToScreen()
        {
            Vector3[] corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);

            float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
            float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
            float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
            float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);

            Vector3 pos = _rectTransform.position;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            if (maxX > screenSize.x)
                pos.x -= (maxX - screenSize.x);

            if (minX < 0)
                pos.x -= minX;

            if (maxY > screenSize.y)
                pos.y -= (maxY - screenSize.y);

            if (minY < 0)
                pos.y -= minY;

            _rectTransform.position = pos;
        }
    }
}
