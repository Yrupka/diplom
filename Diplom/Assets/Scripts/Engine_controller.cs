using System.Collections.Generic;
using UnityEngine;

public class Engine_controller : MonoBehaviour
{
    private Gauge gauge_rpm;
    private Gauge gauge_p;
    private Rpm_switch rpm_switch;
    private Starter starter;
    Engine_options_class options;

    private int rpm;
    private int rpm_old;

    private void Awake()
    {
        options = Save_controller.Load_engine_options();
        rpm = 0;

        gauge_rpm = transform.Find("Gauge_rpm").GetComponent<Gauge>();
        gauge_rpm.Set_max_value(7000f);
        gauge_p = transform.Find("Gauge_p").GetComponent<Gauge>();
        if (options.lever_length != 0)
            gauge_p.Set_max_value(options.Get_moment_max() / options.lever_length);
        rpm_switch = transform.Find("Rpm_switch").GetComponent<Rpm_switch>();
        starter = transform.Find("Starter").Find("Head").GetComponent<Starter>();
    }

    private void Update()
    {
        if (starter.Engine_state())
        {
            rpm = rpm_switch.Get_rpm();
            
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
}