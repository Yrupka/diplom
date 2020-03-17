using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item_gas_tank : Item_highligh
{
    private UnityAction click_action;
    private Animator animation;

    private void Awake()
    {
        animation = GetComponent<Animator>();
        animation.speed = 0.1f;
    }

    private void OnMouseUpAsButton()
    {
        click_action();
    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }

    private void Add_fuel()
    {
            //scale.Add_weight(30f); //!--
        animation.Play("Fuel_tank_act");
    }
}
