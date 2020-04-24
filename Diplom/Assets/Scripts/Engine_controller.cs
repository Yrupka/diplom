﻿using System.Collections;
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
    private Temperature temperature;
    private Engine_options_class options;
    private AudioSource sound_source;
    public Animator anim;
    public AudioClip[] engine_sounds;

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
        if (options != null)
        {
            engine_state = false;
            rpm = 0;
            rpm_old = 0;
            rpms = options.Get_list_rpm();
            moments = options.Get_list_moment();
            consumptions = options.Get_list_consumption();
            lever_length = options.lever_length;

            sound_source = GetComponent<AudioSource>();
            gauge_rpm = transform.Find("Gauge_rpm").GetComponent<Gauge>();
            gauge_rpm.Set_max_value(7000f);
            gauge_p = transform.Find("Gauge_p").GetComponent<Gauge>();
            gauge_p.Set_max_value(options.max_moment / lever_length);
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
                sound_source.pitch = rpm_old / 2000f;
                sound_source.volume = Mathf.InverseLerp(1000f, 7000f, rpm) + 0.3f;
                rpm = rpm_switch.Get_rpm(); // получение числа оборотов с переключателя
                fuel_weight -= Calculation_formulas.Interpolate(rpm_old, rpms, consumptions) / 3600f * temperature.Penalty();
                action_update(); // обновление количества топлива для весов
                anim.SetFloat("speed", rpm_old / 700); // установка скорости для анимации
            }
        }
        rpm_old = (int)Mathf.Lerp(rpm_old, rpm, Time.deltaTime * 3f);
        gauge_rpm.Value(rpm_old);
        gauge_p.Value(Calculation_formulas.Interpolate(rpm_old, rpms, moments) / lever_length);
    }

    private void Engine_heat_ready() // двигатель нагрет
    {
        info_system.Temp(false);
    }

    private void Engine_prestart() // ключ во 2 положении
    {
        if (!engine_state)
        {
            if (!info_system.Fuel_state())
                info_system.Pre_start();
            sound_source.Stop();
        }
        else
            starter.Block(true);
        StopAllCoroutines();
    }

    private void Engine_start()
    {
        StartCoroutine(Engine_starter_in_work());
    }

    private void Engine_stop()
    {
        info_system.Off();
        temperature.Heat(false);
        starter.Block(false);
        if (engine_state)
            Play_sound(2);
        engine_state = false;
        rpm = 0;
        action_stop();
    }

    private void Play_sound(int index)
    {
        sound_source.clip = engine_sounds[index];
        sound_source.pitch = 1;
        sound_source.volume = 0.3f;
        sound_source.Play();
        if (index == 1)
            sound_source.loop = true;
        else
            sound_source.loop = false;
    }

    private IEnumerator Engine_starter_in_work()
    {
        Play_sound(0);
        yield return new WaitForSeconds(1f);
        action_start(); // получили топливо
        if (fuel_weight > 0)
        {
            info_system.Check(true);
            info_system.Fuel(false);
            info_system.Temp(true);
            temperature.Heat(true);
            engine_state = true;
            Play_sound(1);
        }
        else
            info_system.Fuel(true);
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