using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediator_fuel : MonoBehaviour
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

    private void Fuel_update()
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
        Fuel_update();
    }

    private void Gas_tank_clicked()
    {
        if (glass_on_scale)
            StartCoroutine(Fuel_adding());
    }

    private void Glass_set() // стакан поставили на весы
    {
        glass_on_scale = true;
        Fuel_update();
    }

    private void Glass_unset() // стакан сняли с весов
    {
        glass_on_scale = false;
        Fuel_update();
    }
}
