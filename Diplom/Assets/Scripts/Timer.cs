using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private Item_button start;
    private Item_button discard;
    private TextMesh timer_text;
    private bool state;
    private float time;

    private void Awake()
    {
        start = transform.Find("Start").GetComponent<Item_button>();
        start.Add_listener(Timer_start_stop);
        discard = transform.Find("Discard").GetComponent<Item_button>();
        discard.Add_listener(Timer_discard);
        timer_text = transform.Find("Timer_text").GetComponent<TextMesh>();

        Timer_discard();
    }

    private void Update()
    {
        if (state)
        {
            time += Time.deltaTime;
            timer_text.text = time.ToString("0.000");
        }
    }

    private void Timer_start_stop()
    {
        state = !state;
    }

    private void Timer_discard()
    {
        time = 0f;
        timer_text.text = "0";
        state = false;
    }
}
