using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item_gas_tank : Item_highligh
{
    private UnityAction click_action;
    private Animator animation;
    private Engine_options_class options;
    private float fuel_add_amount;

    private void Start()
    {
        animation = GetComponent<Animator>();
        if (options != null)
            fuel_add_amount = options.fuel_amount;
    }

    private void OnMouseUpAsButton()
    {
        click_action();
    }

    public void Load_options(Engine_options_class loaded_options) // получить загруженные данные
    {
        options = loaded_options;
    }

    public void Play_animation()
    {
        animation.Play("Fuel_tank_act");
    }

    public float Get_fuel()
    {
        return fuel_add_amount;
    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }
}
