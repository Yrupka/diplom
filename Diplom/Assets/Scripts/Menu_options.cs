using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class Menu_options : MonoBehaviour
{
    private Slider mouse_slider;
    private InputField mouse_input;
    private Dropdown resolutions;
    private Resolution[] resolutions_list;
    private Slider sound_slider;
    private InputField sound_input;
    private System.Globalization.CultureInfo culture;
    public AudioMixerGroup audio_mixer;

    private UnityAction action_mouse;

    private float mouse_sens_value;
    private float sound_level;
    public bool in_game;

    private void Awake()
    {
        Transform menu = transform.Find("Menu");
        // для разшения формата float с точкой вместо запятой
        culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        mouse_slider = menu.Find("Mouse_sens").Find("Slider").GetComponent<Slider>();
        mouse_slider.onValueChanged.AddListener(
            (value) => mouse_input.text = value.ToString(culture));

        mouse_input = menu.Find("Mouse_sens").Find("Number").GetComponent<InputField>();
        mouse_input.onValueChanged.AddListener(Mouse_sensitivity_input);

        if (!in_game)
            menu.Find("Mouse_sens").gameObject.SetActive(false);

        if (!in_game)
            menu.Find("Sound").gameObject.SetActive(false);

        resolutions = menu.Find("Resolutions").Find("Dropdown").GetComponent<Dropdown>();
        resolutions.onValueChanged.AddListener(Resolutions);
        Setup_resolutions();

        menu.Find("Fullscreen").GetComponent<Toggle>().
            onValueChanged.AddListener((value) => Screen.fullScreen = value);

        sound_slider = menu.Find("Sound").Find("Slider").GetComponent<Slider>();
        sound_slider.onValueChanged.AddListener(
            (value) => sound_input.text = value.ToString(culture));

        sound_input = menu.Find("Sound").Find("Number").GetComponent<InputField>();
        sound_input.onValueChanged.AddListener(Sound_input);

        menu.Find("Sound").Find("Toggle").GetComponent<Toggle>()
            .onValueChanged.AddListener(Sound_toggle);

        menu.Find("Back").GetComponent<Button>().onClick.AddListener(Back);
    }

    private void Sound_input(string text)
    {
        if (string.IsNullOrEmpty(text))
            text = "1";
        sound_level = float.Parse(text, culture);
        sound_level = Mathf.Clamp(sound_level, 1f, 10f);
        sound_slider.value = sound_level;
        audio_mixer.audioMixer.SetFloat("Master_volume", Mathf.Lerp(-80, 20, sound_level / 10));
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

    private void Sound_toggle(bool value)
    {
        if (value)
            audio_mixer.audioMixer.SetFloat("Master_volume", sound_level);
        else
            audio_mixer.audioMixer.SetFloat("Master_volume", -80f);
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
