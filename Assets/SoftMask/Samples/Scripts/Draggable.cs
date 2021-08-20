﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMask.Samples.Scripts {
    [RequireComponent(typeof(RectTransform))]
    public class Draggable : UIBehaviour, IDragHandler {
        RectTransform _rectTransform;

        protected override void Awake() {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void OnDrag(PointerEventData eventData) {
            _rectTransform.anchoredPosition += eventData.delta;
        }
    }
}
