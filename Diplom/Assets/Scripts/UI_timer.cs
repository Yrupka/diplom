﻿using UnityEngine;
using UnityEngine.UI;

public class UI_timer : MonoBehaviour
{
    private Text text_value;
    private bool state;
    private float time;

    private void Awake()
    {
        text_value = transform.Find("Value").GetComponent<Text>();
        transform.Find("Start_stop").GetComponent<Button>().onClick.AddListener(Timer_start_stop);
        transform.Find("Discard").GetComponent<Button>().onClick.AddListener(Timer_discard);

        Timer_discard();
    }

    private void Update()
    {
        if (state)
        {
            time += Time.deltaTime;
            text_value.text = "Значение: " + time.ToString("0.000");
        }
    }

    private void Timer_start_stop()
    {
        state = !state;
    }

    private void Timer_discard()
    {
        time = 0f;
        text_value.text = "0";
        state = false;
    }
}
