﻿using UnityEngine;
using UnityEngine.Events;

public class Item_button : Item_highligh
{

    public Vector3 direction;
    private UnityAction click_action;

    private void OnMouseDown()
    {
        transform.localPosition -= direction;
    }

    private void OnMouseUp()
    {
        transform.localPosition += direction;
        click_action();
    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }
}
