using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item_gas_tank : Item_highligh
{
    private UnityAction click_action;
    private Animator animation;
    private float fuel_add_amount;

    private void Awake()
    {
        animation = GetComponent<Animator>();
        animation.speed = 0.1f;
        fuel_add_amount = 10f; //!! load
    }

    private void OnMouseUpAsButton()
    {
        click_action();
    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }

    public void Play_animation()
    {
        animation.Play("Fuel_tank_act");
    }

    public float Get_fuel()
    {
        return fuel_add_amount;
    }
}
