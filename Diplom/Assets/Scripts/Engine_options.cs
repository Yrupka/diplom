using UnityEngine;
using UnityEngine.UI;

public class Engine_options : MonoBehaviour
{
    private InputField input_m; // масса добавляемого топлива
    private InputField input_l; // длина рычага тормозящего устройства
    private InputField input_t; // время нагрева двигателя до рабочей температуры
    private Slider input_inter; // количество точек интерполяции
    private Scroll_controller scroll;
    private Engine_options_class options;
    private Graph_options graph;
    // для разшения формата float с точкой вместо запятой
    System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

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
        graph = transform.Find("Graph").GetComponent<Graph_options>();
        transform.Find("Save_button").GetComponent<Button>().onClick.AddListener(Confirm_button);
        transform.Find("Load_button").GetComponent<Button>().onClick.AddListener(Load);
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

        Engine_options_class options_update = new Engine_options_class(float.Parse(input_m.text, culture),
            float.Parse(input_l.text, culture),
            int.Parse(input_t.text),
            (int)input_inter.value);
        options_update.Set_rpms(scroll.GetItems());

        if (options != options_update)
        {
            options = options_update;
            Save();
        }
    }

    private void Save()
    {
        Save_controller.Save_engine_options(options);
    }

    private void Load()
    {
        options = Save_controller.Load_engine_options();
        input_m.text = options.fuel_amount.ToString(culture);
        input_l.text = options.lever_length.ToString(culture);
        input_t.text = options.heat_time.ToString(culture);
        scroll.AddMany(options.Get_rpms());
        input_inter.value = options.interpolation;
    }

    // обновить график по номеру (0-момента, 1-мощности, 2-расхода, 3 - обновить все)
    private void Graph_update(int graph_num) 
    {
        options = new Engine_options_class(float.Parse(input_m.text, culture),
            float.Parse(input_l.text, culture),
            int.Parse(input_t.text),
            (int)input_inter.value);
        options.Set_rpms(scroll.GetItems());
        graph.Calculate_graphs(options, graph_num);
    }
}