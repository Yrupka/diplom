using UnityEngine;

public class Item_glass : Item_highligh
{
    private Transform base_parent;

    private float weight_glass;
    private float weight_fuel;
    private float volume; // если будет несколько видов топлива !! разная плотность = разный объем
    private float volume_max;

    private void Awake()
    {
        base_parent = transform.parent;
        GetComponent<Rigidbody>().freezeRotation = true;

        weight_glass = 250f;
        weight_fuel = 0f;
        volume = 0f;
        volume_max = 1000f;
    }

    private void OnMouseDown()
    {
        transform.SetParent(Camera.main.transform);
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnMouseDrag()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void OnMouseUp()
    {
        transform.SetParent(base_parent);
        GetComponent<Rigidbody>().useGravity = true;
    }

    public void Fuel_update(float amount) // добавить топлива или убрать топливо
    {
        weight_fuel += amount;
        volume += amount * 1f; // плотность

        // изменить количество внутри визуально
    }
    
    public float Get_weight() // вернуть общую массу
    {
        return weight_glass + weight_fuel;
    }

    public float Get_fuel_weight()
    {
        return weight_fuel;
    }
}
