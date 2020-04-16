using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Engine_controller : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private Gauge gauge_rpm;
    private Gauge gauge_p;
    private Rpm_switch rpm_switch;
    private Starter starter;
    private Info_system info_system;
    private Temperature temperature;
    private Engine_options_class options;

    private UnityAction action_start;
    private UnityAction action_update;
    private UnityAction action_stop;


    private List<int> rpms;
    private List<float> moments;
    private List<float> consumptions;
    private float lever_length;
    private bool engine_state;
    private int rpm;
    private int rpm_old;
    private float fuel_weight;

    private void Start()
    {
        if (options.rpms.Count != 0)
        {
            engine_state = false;
            rpm = 0;
            rpm_old = 0;
            rpms = options.Get_list_rpm();
            moments = options.Get_list_moment();
            consumptions = options.Get_list_consumption();
            lever_length = options.lever_length;

            gauge_rpm = transform.Find("Gauge_rpm").GetComponent<Gauge>();
            gauge_rpm.Set_max_value(7000f);
            gauge_p = transform.Find("Gauge_p").GetComponent<Gauge>();
            gauge_p.Set_max_value(options.Get_moment_max() / lever_length);
            rpm_switch = transform.Find("Rpm_switch").Find("Head").GetComponent<Rpm_switch>();
            starter = transform.Find("Starter").Find("Head").GetComponent<Starter>();
            starter.Add_listener_prestarted(Engine_prestart);
            starter.Add_listener_started(Engine_start);
            starter.Add_listener_stoped(Engine_stop);
            info_system = transform.Find("Info_system").GetComponent<Info_system>();
            temperature = transform.Find("Temperature").GetComponent<Temperature>();
            temperature.Heat_time_set(options.heat_time);
            temperature.Add_listener_heated(Engine_heat_ready);
        }
        else
            enabled = false; // функция обновления не будет работать
    }

    private void Update()
    {
        if (engine_state)
        {
            if (fuel_weight <= 0)
            {
                Engine_stop();
                info_system.Fuel(true);
            }
            else
            {
                rpm = rpm_switch.Get_rpm(); // получение числа оборотов с переключателя
                fuel_weight -= Interpolate(rpm_old, rpms, consumptions) / 3600f * temperature.Penalty();
                action_update(); // обновление количества топлива для весов
                anim.SetFloat("speed", rpm_old / 700);
            }
        }
        rpm_old = (int)Mathf.Lerp(rpm_old, rpm, Time.deltaTime * 3f);
        gauge_rpm.Value(rpm_old);
        gauge_p.Value(Interpolate(rpm_old, rpms, moments) / lever_length);
    }

    // функция вычисляющая интерполирующую состовляющую графика, label_x,y - значения исходной функции
    private float Interpolate(float x, List<int> label_x, List<float> label_y)
    {
        float answ = 0f;
        for (int j = 0; j < label_x.Count; j++)
        {
            float l_j = 1f;
            for (int i = 0; i < label_x.Count; i++)
            {
                if (i == j)
                    l_j *= 1f;
                else
                    l_j *= (x - label_x[i]) / (label_x[j] - label_x[i]);
            }
            answ += l_j * label_y[j];
        }
        return answ;
    }

    private void Engine_heat_ready() // двигатель нагрет
    {
        info_system.Temp(false);
    }

    private void Engine_prestart() // ключ в положении зажигания
    {
        if (!engine_state)
            info_system.Pre_start();
    }

    private void Engine_start() // ключ в положении зажигания
    {
        action_start(); // получили топливо
        info_system.Check(true);
        info_system.Fuel(false);
        info_system.Temp(true);
        temperature.Heat(true); // начать нагрев двс
        engine_state = true;
        starter.Block(true); // заблокировать стартер на запуск
    }

    private void Engine_stop() // ключ в положении отключен
    {
        info_system.Check(false);
        info_system.Temp(false);
        info_system.Fuel(false);
        temperature.Heat(false);
        starter.Block(false);
        engine_state = false;
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