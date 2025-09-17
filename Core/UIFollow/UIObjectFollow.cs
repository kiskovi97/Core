using UnityEngine;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    public class FollowTargetData : IData
    {
        public Transform goal;
        public Sprite icon;
        public bool canSeenAllTheTime;
    }

    [RequireComponent(typeof(RectTransform))]
    internal class UIObjectFollow : DataHolder<FollowTargetData>
    {
        public Image iconImage;
        public GameObject baseObj;

        private RectTransform CanvasRect;
        private RectTransform rectTransform;

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);
            if (Data == null) return;
            iconImage.sprite = Data.icon;
            baseObj.SetActive(false);
        }

        private void Start()
        {
            CanvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Data == null) return;
            if (Data.goal == null || Data.goal.gameObject == null || !Data.goal.gameObject.activeInHierarchy)
            {
                baseObj.SetActive(false);
                return;
            }
            var camera = Camera.main;
            if (camera != null)
            {
                baseObj.SetActive(true);
                var ViewportPosition = camera.WorldToViewportPoint(Data.goal.position);
                var tY = rectTransform.sizeDelta.y / CanvasRect.sizeDelta.y / 2f;
                var tX = rectTransform.sizeDelta.x / CanvasRect.sizeDelta.x / 2f;
                var color = iconImage.color;
                color.a = ViewportPosition.y < 0f || ViewportPosition.y > 1f ||
                    ViewportPosition.x < 0f || ViewportPosition.x > 1f
                    ? 1f
                    : 0.5f;
                if (!Data.canSeenAllTheTime)
                    if (!(ViewportPosition.y < 0f || ViewportPosition.y > 1f ||
                    ViewportPosition.x < 0f || ViewportPosition.x > 1f))
                        baseObj.SetActive(false);
                iconImage.color = color;
                ViewportPosition.y = Mathf.Clamp(ViewportPosition.y, 0f + tY, 1f - tY);
                ViewportPosition.x = Mathf.Clamp(ViewportPosition.x, 0f + tX, 1f - tX);

                var WorldObject_ScreenPosition = new Vector2(
                    ViewportPosition.x * CanvasRect.sizeDelta.x - CanvasRect.sizeDelta.x * 0.5f,
                    ViewportPosition.y * CanvasRect.sizeDelta.y - CanvasRect.sizeDelta.y * 0.5f);

                rectTransform.anchoredPosition = WorldObject_ScreenPosition;
            }
        }
    }
}
