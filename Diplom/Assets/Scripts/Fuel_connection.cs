using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel_connection : MonoBehaviour
{
    [SerializeField]
    private Engine_controller engine_controller;
    [SerializeField]
    private Fuel_controller fuel_controller;
    private float fuel_taken;

    private void Awake()
    {
        engine_controller.Add_listener_start(Fuel_to_engine);
        engine_controller.Add_listener_update(Fuel_to_scale);
        fuel_taken = 0f;
    }

    private void Fuel_to_engine()
    {
        fuel_taken = fuel_controller.Fuel_get();
        engine_controller.Set_fuel(fuel_taken);
    }

    private void Fuel_to_scale()
    {
        float fuel_remaining = engine_controller.Get_fuel();
        fuel_controller.Fuel_spent(fuel_taken - fuel_remaining);
        fuel_taken = fuel_remaining;
    }
}
