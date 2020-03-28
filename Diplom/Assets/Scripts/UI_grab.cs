﻿using UnityEngine.EventSystems;
using UnityEngine;

public class UI_grab : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

};