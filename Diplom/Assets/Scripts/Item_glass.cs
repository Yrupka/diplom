﻿using System.Collections;
using UnityEngine;

public class Item_glass : Item_highligh
{
    private Transform base_parent;
    private Transform fuel;
    private Rigidbody body;

    private float weight_glass;
    private float weight_fuel;
    private float fuel_zero_pos; // начальная позиция объекта "топливо" в стакане
    private float volume; // если будет несколько видов топлива !! разная плотность = разный объем
    private float volume_max;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        base_parent = transform.parent;
        body.freezeRotation = true;
        fuel = transform.Find("Fuel");
        fuel.gameObject.SetActive(false);
        fuel_zero_pos = Mathf.Abs(fuel.localPosition.y);

        weight_glass = 250f;
        weight_fuel = 0f;
        volume = 0f;
        volume_max = 100f;
    }

    private void OnMouseDown()
    {
        transform.SetParent(Camera.main.transform);
        body.useGravity = false;
        body.drag = 1000;
    }

    private void OnMouseDrag()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void OnMouseUp()
    {
        transform.SetParent(base_parent);
        body.useGravity = true;
        body.drag = 0;
    }

    public void Fuel_update(float amount) // добавить топлива или убрать топливо
    {
        weight_fuel += amount;
        volume += amount * 1f; // плотность

        weight_fuel = Mathf.Clamp(weight_fuel, 0f, 100f);

        if (weight_fuel == 0)
            fuel.gameObject.SetActive(false);
        else
            fuel.gameObject.SetActive(true);

        // изменить количество внутри визуально
        fuel.localScale = new Vector3(fuel.localScale.x, weight_fuel * fuel_zero_pos / 100, fuel.localScale.z);
        fuel.localPosition = new Vector3(0f, fuel.localScale.y - fuel_zero_pos, 0f); 
    }
    
    public float Get_weight() // вернуть общую массу
    {
        return weight_glass + weight_fuel;
    }

    public float Get_fuel_weight()
    {
        return weight_fuel;
    }

    public void Set_interactable(bool state) // можно ли взаимодействовать со стаканом
    {
        body.useGravity = state;
        transform.GetComponent<BoxCollider>().enabled = state;
    }
}
