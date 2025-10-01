using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PixelRivals.Inputs
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform ring;
        public RectTransform knob;
        public float maxRadius = 80f;

        public Vector2 Direction { get; private set; }

        private Vector2 _startPos;
        private bool _active;

        private void Awake()
        {
            if (ring == null) ring = GetComponent<RectTransform>();
            if (knob == null && transform.childCount > 0) knob = transform.GetChild(0) as RectTransform;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _active = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ring, eventData.position, eventData.pressEventCamera, out _startPos);
            UpdateKnob(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_active) return;
            UpdateKnob(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _active = false;
            Direction = Vector2.zero;
            if (knob != null) knob.anchoredPosition = Vector2.zero;
        }

        private void UpdateKnob(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(ring, eventData.position, eventData.pressEventCamera, out localPoint))
                return;

            var delta = localPoint - Vector2.zero;
            var clamped = Vector2.ClampMagnitude(delta, maxRadius);
            if (knob != null) knob.anchoredPosition = clamped;
            Direction = clamped / maxRadius;
        }
    }
}