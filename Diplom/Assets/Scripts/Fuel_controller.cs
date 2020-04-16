using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Fuel_controller : MonoBehaviour
{
    private Scale scale;
    private Glass_detection detector;
    private Item_gas_tank fuel_tank;
    private Item_glass glass;
    private Fuel_pomp fuel_pomp;
    private UnityAction action_hints;

    private bool glass_on_scale;
    private bool engine_in_work;

    private void Awake()
    {
        fuel_pomp = transform.Find("Fuel_pomp").GetComponent<Fuel_pomp>();
        fuel_pomp.Add_listener(Fuel_pomp_clicked);

        scale = transform.Find("Scale").GetComponent<Scale>();

        detector = scale.transform.Find("Glass_detection").GetComponent<Glass_detection>();
        detector.Add_listener_enter(Glass_set);
        detector.Add_listener_exit(Glass_unset);

        fuel_tank = transform.Find("Fuel_tank").GetComponent<Item_gas_tank>();
        fuel_tank.Add_listener(Gas_tank_clicked);

        glass = transform.Find("Glass").GetComponent<Item_glass>();

        engine_in_work = false;
    }

    private void Update()
    {
        if (glass_on_scale)
            scale.Weight_set(glass.Get_weight());
        else
            scale.Weight_set(0f);
    }

    private void Fuel_pomp_clicked()
    {
        if (glass_on_scale) // если стакан на весах
            if (fuel_pomp.State_info()) // если насос уже в рабочем положении
            {
                if (!engine_in_work)
                {
                    fuel_pomp.Play_animation();
                    fuel_pomp.Set_state(false);
                    glass.Set_interactable(true);
                }
            }
            else // если насос в нерабочем положении
            {
                fuel_pomp.Play_animation();
                fuel_pomp.Set_state(true); // установить рабочее положение для помпы
                glass.Set_interactable(false); // со стаканом нельзя взаимодействовать
            }
        action_hints();
    }

    private void Gas_tank_clicked()
    {
        if (glass_on_scale && !fuel_pomp.State_info())
            StartCoroutine(Fuel_adding());
    }

    private void Glass_set() // стакан поставили на весы
    {
        glass_on_scale = true;
        action_hints();
    }

    private void Glass_unset() // стакан сняли с весов
    {
        glass_on_scale = false;
        action_hints();
    }

    IEnumerator Fuel_adding() // добавление топлива в стакан
    {
        fuel_tank.Play_animation();
        yield return new WaitForSeconds(1f);
        int fuel_to_add = fuel_tank.Get_fuel();
        for (int i = 0; i < fuel_to_add; i++)
        {
            Debug.Log(i);
            glass.Fuel_update(1);
            //yield return new WaitForSeconds(1f / fuel_to_add);
        }
    }

    public float Fuel_get() // получение количества топлива для стенда двигателя
    {
        if (fuel_pomp.State_info())
            return glass.Get_fuel_weight();
        else
            return 0f;
    }

    public void Fuel_spent(float value) // стенд двигателя изменяет значение топлива по мере работы
    {
        glass.Fuel_update(-value);
    }

    public void Set_engine_state(bool state)
    {
        engine_in_work = state;
        action_hints();
    }

    public int State()
    {
        int state_count = 0;
        if (glass_on_scale)
            state_count++;
        if (fuel_pomp.State_info())
            state_count++;
        if (engine_in_work)
            state_count++;
        return state_count;
    }

    public void Add_listener_state(UnityAction action)
    {
        action_hints += action;
    }
}
