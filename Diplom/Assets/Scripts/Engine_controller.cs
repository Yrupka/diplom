using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine_controller : MonoBehaviour
{ 
    private Text fuel_text; // вывод на экран топлива
    private Toggle toggle_engine; // кнопка зажигания
    private Slider gas_value;
    private Speedometer speedometer;
    private Speedometer p_meter;

    Engine_options_class options;
    private int rpm_value_new;
    private int rpm_value_old;

    private bool engine_start; // состояние двигателя

    private void Awake()
    {
        fuel_text = transform.Find("Fuel_amount").GetComponent<Text>();
        toggle_engine = transform.Find("Engine_start").GetComponent<Toggle>();
        gas_value = transform.Find("Gas").GetComponent<Slider>();
        toggle_engine.onValueChanged.AddListener((value) => { Engine_control(value); });
        
        options = Save_controller.Load_engine_options();

        speedometer = transform.Find("Speedometer").GetComponent<Speedometer>();
        speedometer.Set_speed_max(7000f);
        p_meter = transform.Find("P_meter").GetComponent<Speedometer>();
        p_meter.Set_speed_max(options.Get_moment_max() / options.lever_length);
        engine_start = false;
    }

    private void Update()
    {
        if (engine_start)
        {
            rpm_value_new = (int)gas_value.value;
            options.fuel_amount -= Rpm_fuel_count(rpm_value_new);
            fuel_text.text = options.fuel_amount.ToString();
        }
        rpm_value_old = (int)Mathf.Lerp(rpm_value_old, rpm_value_new, Time.deltaTime * 3f);
        speedometer.Set_speed(rpm_value_old);
        p_meter.Set_speed(Rpm_p_count(rpm_value_old));
    }

    public void Engine_control(bool value)
    {
        engine_start = value;
        if (!engine_start)
            rpm_value_new = 0;
    }

    private float Rpm_fuel_count(int rpm_num) // расчет потраченного топлива на заданных оборотах в минуту
    {
        int i = Get_index(rpm_num);
        if (i >= 0) // если число оборотов совпадет с известным значением расхода, то вернуть это значение
            return options.rpms[i].consumption / 3600f;
        else // иначе нужно вычислить расход основываясь на известных данных
        {
            i = i * (-1) - 1;
            if (options.rpms.Count <= i) // если нет показаний для данных оборотов, то расход нулевой
                return 0f;
            else // если показания расхода есть, то нужно высчитать расход по заданных оборотам
            {
                float procents = rpm_num % 1000 / 1000f; // процент отклонения заданной величины от элемента[i - 1]
                return Mathf.Lerp(options.rpms[i - 1].consumption, options.rpms[i].consumption, procents) / 3600f;
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
            i = i * (-1) - 1;
            if (options.rpms.Count <= i || i == 0)
                return 0f;
            else
            {
                float procents = rpm_num % 1000 / 1000f;
                return Mathf.Lerp(options.rpms[i - 1].moment, options.rpms[i].moment, procents) / options.lever_length;
            }
        }
    }

    private int Get_index(int rpm_num)
    {
        options.rpms.Sort((a,b) => a.rpm.CompareTo(b.rpm));

        List<int> only_rpm = new List<int>(); // создаем список с значениями оборотов для поиска значения большего чем заданные обороты
        for (int i = 0; i < options.rpms.Count; i++)
            only_rpm.Add(options.rpms[i].rpm);

        int index = only_rpm.BinarySearch(rpm_num); // приведение индекса найденного значения к нормальному виду
        return index;
    }
}