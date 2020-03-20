using System.Collections;
using UnityEngine;

public class Fuel_controller : MonoBehaviour
{
    private Scale scale;
    private Glass_detection detector;
    private Item_gas_tank gas_tank;
    private Item_glass glass;

    private bool glass_on_scale;

    private void Awake()
    {
        scale = transform.Find("Scale").GetComponent<Scale>();

        detector = scale.transform.Find("Glass_detection").GetComponent<Glass_detection>();
        detector.Add_listener_enter(Glass_set);
        detector.Add_listener_exit(Glass_unset);

        gas_tank = transform.Find("Gas_tank").GetComponent<Item_gas_tank>();
        gas_tank.Add_listener(Gas_tank_clicked);

        glass = transform.Find("Glass").GetComponent<Item_glass>();
    }

    private void Update()
    {
        if (glass_on_scale)
            scale.Weight_set(glass.Get_weight());
        else
            scale.Weight_set(0f);
    }

    IEnumerator Fuel_adding()
    {
        gas_tank.Play_animation();
        yield return new WaitForSeconds(1);
        glass.Fuel_update(gas_tank.Get_fuel());
    }

    private void Gas_tank_clicked()
    {
        if (glass_on_scale)
            StartCoroutine(Fuel_adding());
    }

    private void Glass_set() // стакан поставили на весы
    {
        glass_on_scale = true;
    }

    private void Glass_unset() // стакан сняли с весов
    {
        glass_on_scale = false;
    }

    public float Fuel_get()
    {
        if (glass_on_scale)
            return glass.Get_fuel_weight();
        else
            return 0f;
    }

    public void Fuel_spent(float value)
    {
        glass.Fuel_update(-value);
    }
}
