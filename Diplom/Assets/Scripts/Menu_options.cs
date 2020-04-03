using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Menu_options : MonoBehaviour
{
    private UnityAction action_mouse;
    private Slider mouse_slider;
    private InputField mouse_input;
    private Dropdown resolutions;
    private System.Globalization.CultureInfo culture;

    private Resolution[] resolutions_list;

    private float mouse_sens_value;

    private void Awake()
    {
        Transform menu = transform.Find("Menu");
        // для разшения формата float с точкой вместо запятой
        culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        mouse_slider = menu.Find("Mouse_sens").Find("Slider").GetComponent<Slider>();
        mouse_slider.onValueChanged.AddListener(Mouse_sensitivity_slider);

        mouse_input = menu.Find("Mouse_sens").Find("Number").GetComponent<InputField>();
        mouse_input.onValueChanged.AddListener(Mouse_sensitivity_input);

        resolutions = menu.Find("Resolutions").Find("Dropdown").GetComponent<Dropdown>();
        resolutions.onValueChanged.AddListener(Resolutions);
        Setup_resolutions();

        menu.Find("Fullscreen").GetComponent<Toggle>().
            onValueChanged.AddListener(Fullscreen);

        menu.Find("Back").GetComponent<Button>().onClick.AddListener(Back);
    }

    private void Mouse_sensitivity_slider(float value)
    {
        mouse_sens_value = value;
        mouse_input.text = mouse_sens_value.ToString();
    }

    private void Mouse_sensitivity_input(string text)
    {
        if (string.IsNullOrEmpty(text))
            text = "1";
        mouse_sens_value = float.Parse(text, culture);
        mouse_sens_value = Mathf.Clamp(mouse_sens_value, 1f, 10f);
        mouse_input.text = mouse_sens_value.ToString(culture);
        mouse_slider.value = mouse_sens_value;
        action_mouse();
    }

    private void Setup_resolutions()
    {
        resolutions.ClearOptions();
        resolutions_list = Screen.resolutions;

        List<string> options = new List<string>();
        int curr_res = 0;

        for (int i = 0; i < resolutions_list.Length; i++)
        {
            string option = resolutions_list[i].width + " x " + resolutions_list[i].height;
            options.Add(option);

            if (resolutions_list[i].Equals(Screen.currentResolution))
                curr_res = i;
        }

        resolutions.AddOptions(options);
        resolutions.value = curr_res;
        resolutions.RefreshShownValue();
    }

    private void Resolutions(int index)
    {
        Screen.SetResolution(resolutions_list[index].width,
            resolutions_list[index].width,
            Screen.fullScreen);
    }

    private void Fullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    private void Back()
    {
        transform.gameObject.SetActive(false);
    }

    public void Add_mouse_sens_listener(UnityAction action)
    {
        action_mouse += action;
    }

    public float Get_sens()
    {
        return mouse_sens_value;
    }
}
