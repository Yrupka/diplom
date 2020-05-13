using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Options_configuration : MonoBehaviour
{
    private Table_rpm_options scroll;
    private Engine_options options;
    private Engine_options options_old;
    private Graph_shower graph;

    private InputField input_m; // масса добавляемого топлива
    private InputField input_l; // длина рычага тормозящего устройства
    private InputField input_t; // время нагрева двигателя до рабочей температуры
    private Slider input_inter; // количество точек интерполяции
    private Dropdown hints_dropdown; // список подсказок
    private Text hints_condition;
    private InputField input_hints; // подсказки
    private InputField input_car; // имя двигателя
    private InputField input_eng; // имя машины
    private Toggle toggle_profile; // показывать название профиля или нет
    // для разшения формата float с точкой вместо запятой
    System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
    private UnityAction action_close;

    private string[] hint_texts;
    private string[] conditions;

    private void Awake() // нахождение всех полей
    {
        scroll = transform.Find("Scroll").GetComponent<Table_rpm_options>();
        scroll.Add_listener_update_first(() => Graph_update(0));
        scroll.Add_listener_update_second(() => Graph_update(1));
        scroll.Add_listener_update_third(() => Graph_update(2));
        input_m = transform.Find("Input_1").GetComponent<InputField>();
        input_l = transform.Find("Input_2").GetComponent<InputField>();
        input_t = transform.Find("Input_3").GetComponent<InputField>();
        input_inter = transform.Find("Input_4").GetComponent<Slider>();
        // изменение значения слайдера приведет к изменению цифры в окошке
        input_inter.onValueChanged.AddListener((value) =>
            transform.Find("Input_text_4").GetComponent<InputField>().text = value.ToString());
        // обновить все графики, если было изменено значение интерполяции
        input_inter.onValueChanged.AddListener((value) => Graph_update(3));
        input_hints = transform.Find("Input_5").GetComponent<InputField>();
        input_hints.onEndEdit.AddListener(Hints_update);
        input_car = transform.Find("Input_6").GetComponent<InputField>();
        input_eng = transform.Find("Input_7").GetComponent<InputField>();
        toggle_profile = transform.Find("Input_8_toggle").GetComponent<Toggle>();
        hints_dropdown = transform.Find("Dropdown_5").GetComponent<Dropdown>();
        hints_dropdown.onValueChanged.AddListener(Dropdown_updated);
        hints_dropdown.AddOptions(new List<string>()
            { "Подсказка 1", "Подсказка 2", "Подсказка 3", "Подсказка 4"});
        hints_condition = transform.Find("Text_condition_5").GetComponent<Text>();
        graph = transform.Find("Graph").GetComponent<Graph_shower>();
        transform.Find("Save_button").GetComponent<Button>().onClick.AddListener(Confirm_button);
        transform.Find("Load_button").GetComponent<Button>().onClick.AddListener(Load);
        transform.Find("Exit_button").GetComponent<Button>().onClick.AddListener(
            () => transform.parent.parent.gameObject.SetActive(false));
        hint_texts = new string[4];
        conditions = new string[4] {"Вход в лабораторию", "Тара на весах",
            "Топливоприемник опущен", "Двигатель работает"};
    }

    private void OnDisable()
    {
        action_close?.Invoke();
    }

    private void Confirm_button() // нажата кнопка сохранить
    {
        if (string.IsNullOrEmpty(input_m.text)) input_m.text = "0";
        if (string.IsNullOrEmpty(input_l.text)) input_l.text = "0";
        if (string.IsNullOrEmpty(input_t.text)) input_t.text = "0";
        if (string.IsNullOrWhiteSpace(input_car.text)) input_car.text = "м1";
        if (string.IsNullOrWhiteSpace(input_eng.text)) input_eng.text = "д1";

        options.car_name = input_car.text;
        options.engine_name = input_eng.text;
        options.profile_show = toggle_profile.isOn;
        float lever = float.Parse(input_l.text, culture);
        if (lever == 0)
            lever = 1;
        options.fuel_amount = int.Parse(input_m.text, culture);
        options.lever_length = lever;
        options.heat_time = int.Parse(input_t.text);
        options.hints = hint_texts;
        options.max_moment = graph.Get_max_moment();

        Save();
    }

    private void Dropdown_updated(int value)
    {
        input_hints.text = hint_texts[value];
        hints_condition.text = conditions[value];
    }

    private void Hints_update(string value)
    {
        hint_texts[hints_dropdown.value] = value;
    }

    private void Save()
    {
        options_old = options;
    }

    private void Load()
    {
        options = options_old;
        graph.Clear_graphs();

        hints_dropdown.value = 0;
        hint_texts = options.hints;
        input_car.text = options.car_name;
        input_eng.text = options.engine_name;
        toggle_profile.isOn = options.profile_show;
        input_m.text = options.fuel_amount.ToString(culture);
        input_l.text = options.lever_length.ToString(culture);
        input_t.text = options.heat_time.ToString(culture);
        scroll.AddMany(options.Get_rpms());
        input_inter.value = options.interpolation;
        input_hints.text = hint_texts[hints_dropdown.value];
        hints_condition.text = conditions[hints_dropdown.value];
        Graph_update(3);
    }

    // обновить график по номеру (0-момента, 1-мощности, 2-расхода, 3 - обновить все)
    private void Graph_update(int graph_num) 
    {
        options.interpolation = (int)input_inter.value;
        options.Set_rpms(scroll.GetItems());
        if (options.rpms.Count != 0)
        {
            options.rpms.Sort((a, b) => a.rpm.CompareTo(b.rpm));
            graph.Calculate_graphs(options, graph_num);
        }
    }

    public void Set_profile(Engine_options profile)
    {
        options_old = profile;
        options = profile;
        Load();
    }

    public Engine_options Get_profile()
    {
        return options_old;
    }

    public void Add_listener_closed(UnityAction action)
    {
        action_close += action;
    }
}