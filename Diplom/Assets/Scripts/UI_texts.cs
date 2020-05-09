﻿using UnityEngine;
using UnityEngine.UI;

public class UI_texts : MonoBehaviour
{
    public Fuel_controller fuel_controller;

    private Text hint_info;
    private Text profile_info;
    private RectTransform background;

    private string[] texts;

    private void Awake()
    {
        hint_info = transform.Find("Hint").GetComponent<Text>();
        profile_info = transform.Find("Profile").GetComponent<Text>();
        fuel_controller.Add_listener_state(Text_update);
    }

    private void Text_update()
    {
        string text = texts[fuel_controller.State()];
        if (string.IsNullOrEmpty(text))
            hint_info.text = "Нет подсказки";
        else
            hint_info.text = text;
    }

    public void Clicked(bool state)
    {
        profile_info.gameObject.SetActive(state);
    }

    public void Set_texts(string[] loaded_options, bool profile_state, string car_name, string engine_name) // получить загруженные данные
    {
        texts = loaded_options;
        if (profile_state)
            profile_info.text = "Профиль:\nАвтомобиль: " + car_name + "\nМодель двигателя: " + engine_name;
        else
            Destroy(profile_info.gameObject);
        Text_update();
    }
}
