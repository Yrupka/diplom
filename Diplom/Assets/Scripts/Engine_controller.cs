using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Engine_controller : MonoBehaviour
{
    private Gauge gauge_rpm;
    private Gauge gauge_p;
    private Rpm_switch rpm_switch;
    private Starter starter;
    private Info_system info_system;
    private Engine_options_class options;

    private UnityAction action_start;
    private UnityAction action_update;
    private UnityAction action_stop;

    private bool engine_state;
    private int rpm;
    private int rpm_old;
    private float fuel_weight;

    private void Start()
    {
        if (options != null)
        {
            engine_state = false;
            rpm = 0;
            rpm_old = 0;

            gauge_rpm = transform.Find("Gauge_rpm").GetComponent<Gauge>();
            gauge_rpm.Set_max_value(7000f);
            gauge_p = transform.Find("Gauge_p").GetComponent<Gauge>();
            if (options.lever_length != 0)
                gauge_p.Set_max_value(options.Get_moment_max() / options.lever_length);
            rpm_switch = transform.Find("Rpm_switch").Find("Head").GetComponent<Rpm_switch>();
            starter = transform.Find("Starter").Find("Head").GetComponent<Starter>();
            starter.Add_listener_started(Engine_start);
            starter.Add_listener_stoped(Engine_stop);
            info_system = transform.Find("Info_system").GetComponent<Info_system>();
        }
        else
            enabled = false; // функция обновления не будет работать
    }

    private void Update()
    {
        if (engine_state)
        {
            rpm = rpm_switch.Get_rpm(); // получение числа оборотов с переключателя
            fuel_weight -= Rpm_fuel_count(rpm_old); // расчет потраченного топлива
            if (fuel_weight <= 0)
            {
                Engine_stop();
                info_system.Fuel_on();
            }
            action_update(); // обновление количества топлива для весов
        }
        rpm_old = (int)Mathf.Lerp(rpm_old, rpm, Time.deltaTime * 3f);
        gauge_rpm.Value(rpm_old);
        gauge_p.Value(Rpm_p_count(rpm_old));
    }

    private float Rpm_fuel_count(int rpm_num) // расчет потраченного топлива на заданных оборотах в минуту
    {
        int i = Get_index(rpm_num);
        if (i >= 0) // если число оборотов совпадет с известным значением расхода, то вернуть это значение
            return options.rpms[i].consumption / 3600f;
        else // иначе нужно вычислить расход основываясь на известных данных
        {
            i = ~i;
            if (options.rpms.Count <= i) // если нет показаний для данных оборотов, то расход нулевой
                return 0f;
            else // если показания расхода есть, то нужно высчитать расход по заданных оборотам
            {
                float procents = rpm_num % 1000 / 1000f;
                float value;
                if (i != 0)
                    value = Mathf.Lerp(options.rpms[i - 1].consumption, options.rpms[i].consumption, procents) / 3600f;
                else
                    value = Mathf.Lerp(0f, options.rpms[0].consumption, procents) / 3600f;
                return value;
            }
        }
    }

    private float Rpm_p_count(int rpm_num) // расчет потраченного топлива на заданных оборотах в минуту
    {
        int i = Get_index(rpm_num);
        if (i >= 0)
            return options.rpms[i].moment / options.lever_length;
        else
        {
            i = ~i;
            if (options.rpms.Count <= i)
                return 0f;
            else
            {
                float procents = rpm_num % 1000 / 1000f;
                float value;
                if (i != 0)
                    value = Mathf.Lerp(options.rpms[i - 1].moment, options.rpms[i].moment, procents) / options.lever_length;
                else
                    value = Mathf.Lerp(0f, options.rpms[0].moment, procents) / options.lever_length;
                return value;
            }
        }
    }

    private int Get_index(int rpm_num)
    {
        options.rpms.Sort((a, b) => a.rpm.CompareTo(b.rpm));

        // создаем список с значениями оборотов для поиска значения большего чем заданные обороты
        List<int> only_rpm = new List<int>();
        for (int i = 0; i < options.rpms.Count; i++)
            only_rpm.Add(options.rpms[i].rpm);

        int index = only_rpm.BinarySearch(rpm_num);
        return index;
    }

    private void Engine_start() // ключ в положении зажигания
    {
        info_system.Fuel_off();
        engine_state = true;
        action_start();
    }

    private void Engine_stop() // ключ в выключенном положении
    {
        info_system.Fuel_off();
        engine_state = false;
        fuel_weight = 0;
        rpm = 0;
        action_stop();
    }

    public void Load_options(Engine_options_class loaded_options) // получить загруженные данные
    {
        options = loaded_options;
    }

    public void Set_fuel(float value) // при включении двигателя, топливо взять из стакана
    {
        fuel_weight = value;
    }

    public float Get_fuel() // получить оставшиеся после работы топливо
    {
        return fuel_weight;
    }

    public void Add_listener_start(UnityAction action)
    {
        action_start += action;
    }

    public void Add_listener_update(UnityAction action)
    {
        action_update += action;
    }

    public void Add_listener_stop(UnityAction action)
    {
        action_stop += action;
    }

}