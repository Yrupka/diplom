using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine_options : MonoBehaviour
{
    private InputField input_m; // масса добавляемого топлива
    private InputField input_l; // длина рычага тормозящего устройства
    private InputField input_t; // время нагрева двигателя до рабочей температуры
    private Slider input_inter; // количество точек интерполяции
    private Dropdown hints_dropdown; // список подсказок
    private InputField input_hints; // подсказки
    private Scroll_controller scroll;
    private Engine_options_class options;
    private Graph_options graph;
    // для разшения формата float с точкой вместо запятой
    System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

    private string[] hint_texts;

    private void Awake() // нахождение всех полей
    {
        scroll = transform.Find("Scroll").GetComponent<Scroll_controller>();
        scroll.Add_listener_update_first(() => Graph_update(0));
        scroll.Add_listener_update_second(() => Graph_update(1));
        scroll.Add_listener_update_third(() => Graph_update(2));
        input_m = transform.Find("Input1").GetComponent<InputField>();
        input_l = transform.Find("Input2").GetComponent<InputField>();
        input_t = transform.Find("Input3").GetComponent<InputField>();
        input_inter = transform.Find("Input4").GetComponent<Slider>();
        // изменение значения слайдера приведет к изменению цифры в окошке
        input_inter.onValueChanged.AddListener((value) =>
            transform.Find("Input4_text").GetComponent<InputField>().text = value.ToString());
        // обновить все графики, если было изменено значение интерполяции
        input_inter.onValueChanged.AddListener((value) => Graph_update(3));
        input_hints = transform.Find("Input5").GetComponent<InputField>();
        input_hints.onEndEdit.AddListener(Hints_update);
        hints_dropdown = transform.Find("Dropdown5").GetComponent<Dropdown>();
        hints_dropdown.onValueChanged.AddListener((value) => 
            input_hints.text = hint_texts[value]);
        hints_dropdown.AddOptions(new List<string>()
            { "Подсказка 1", "Подсказка 2", "Подсказка 3", "Подсказка 4"});
        graph = transform.Find("Graph").GetComponent<Graph_options>();
        transform.Find("Save_button").GetComponent<Button>().onClick.AddListener(Confirm_button);
        transform.Find("Load_button").GetComponent<Button>().onClick.AddListener(Load);
        hint_texts = new string[4];
    }

    private void Start()
    {
        Load();

        Graph_update(3);
    }

    private void Confirm_button() // нажата кнопка сохранить
    {
        if (string.IsNullOrEmpty(input_m.text)) input_m.text = "1";
        if (string.IsNullOrEmpty(input_l.text)) input_l.text = "1";
        if (string.IsNullOrEmpty(input_t.text)) input_t.text = "0";

        float lever = float.Parse(input_l.text);
        if (lever == 0)
            lever = 1;
        Engine_options_class options_update = new Engine_options_class(int.Parse(input_m.text, culture),
            lever,
            int.Parse(input_t.text),
            (int)input_inter.value);
        options_update.hints = hint_texts;
        options_update.Set_rpms(scroll.GetItems());

        if (options != options_update)
        {
            options = options_update;
            Save();
        }
    }

    private void Hints_update(string value)
    {
        hint_texts[hints_dropdown.value] = value;
    }

    private void Save()
    {
        Save_controller.Save_engine_options(options);
    }

    private void Load()
    {
        options = Save_controller.Load_engine_options();
        hint_texts = options.hints;
        input_m.text = options.fuel_amount.ToString(culture);
        input_l.text = options.lever_length.ToString(culture);
        input_t.text = options.heat_time.ToString(culture);
        scroll.AddMany(options.Get_rpms());
        input_inter.value = options.interpolation;
        input_hints.text = hint_texts[hints_dropdown.value];
    }

    // обновить график по номеру (0-момента, 1-мощности, 2-расхода, 3 - обновить все)
    private void Graph_update(int graph_num) 
    {
        float lever = float.Parse(input_l.text);
        if (lever == 0)
            lever = 1;
        options = new Engine_options_class(int.Parse(input_m.text, culture),
            lever,
            int.Parse(input_t.text),
            (int)input_inter.value);
        options.Set_rpms(scroll.GetItems());
        options.hints = hint_texts;
        graph.Calculate_graphs(options, graph_num);
    }
}