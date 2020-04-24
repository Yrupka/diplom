using UnityEngine;

public class Fuel_connection : MonoBehaviour
{
    public Engine_controller engine_controller;
    public Fuel_controller fuel_controller;
    private float fuel_taken;

    private void Awake()
    {
        engine_controller.Add_listener_start(Engine_start);
        engine_controller.Add_listener_update(Fuel_update);
        engine_controller.Add_listener_stop(Engine_stop);
        fuel_taken = 0f;
    }

    private void Engine_start()
    {
        fuel_taken = fuel_controller.Fuel_get();
        fuel_controller.Set_engine_state(true);
        engine_controller.Set_fuel(fuel_taken);
    }

    private void Fuel_update()
    {
        float fuel_remaining = engine_controller.Get_fuel();
        fuel_controller.Fuel_spent(fuel_taken - fuel_remaining);
        fuel_taken = fuel_remaining;
    }

    private void Engine_stop()
    {
        fuel_controller.Set_engine_state(false);
    }
}
