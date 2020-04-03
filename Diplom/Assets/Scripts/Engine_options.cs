using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine_options : MonoBehaviour
{
    [SerializeField]
    private Graph_options graph_options;

    private InputField input_m; // масса добавляемого топлива
    private InputField input_l; // длина рычага тормозящего устройства
    private InputField input_t; // время нагрева двигателя до рабочей температуры
    private Scroll_controller scroll;
    private Engine_options_class options;

    private delegate void MethodContainer();
    private event MethodContainer value_update;

    private void Awake() // нахождение всех полей
    {
        scroll = transform.Find("Scroll").GetComponent<Scroll_controller>();
        input_m = transform.Find("Input1").GetComponent<InputField>();
        input_l = transform.Find("Input2").GetComponent<InputField>();
        input_t = transform.Find("Input3").GetComponent<InputField>();
        transform.Find("Save_button").GetComponent<Button>().onClick.AddListener(() => Confirm_button());
        transform.Find("Load_button").GetComponent<Button>().onClick.AddListener(() => Load());
        value_update += graph_options.Value_update;
    }

    private void Start()
    {
        Load();
    }

    public void Confirm_button()
    {
        if (string.IsNullOrEmpty(input_m.text)) input_m.text = "0";
        if (string.IsNullOrEmpty(input_l.text)) input_l.text = "0";
        if (string.IsNullOrEmpty(input_t.text)) input_t.text = "0";

        // для разшения формата float с точкой вместо запятой
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        options = new Engine_options_class(float.Parse(input_m.text, culture),
            float.Parse(input_l.text, culture),
            int.Parse(input_t.text));

        options.Set_rpms(scroll.GetItems());
        
        value_update();
        Save();
    }

    public void Save()
    {
        Save_controller.Save_engine_options(options);
    }

    public void Load()
    {
        // для разшения формата float с точкой вместо запятой
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        options = Save_controller.Load_engine_options();
        if (options != null)
        {
            input_m.text = options.fuel_amount.ToString(culture);
            input_l.text = options.lever_length.ToString(culture);
            input_t.text = options.heat_time.ToString(culture);
            scroll.AddMany(options.Get_rpms());
        }
        value_update();
    }

    public Engine_options_class Get_engine_options()
    {
        return options;
    }
}

[Serializable]
public class Engine_options_class
{
    [Serializable]
    public struct struct_rpms
    {
        public int rpm;
        public float moment;
        public float consumption;

        public struct_rpms(int rpm, float moment, float consumption)
        {
            this.rpm = rpm;
            this.moment = moment;
            this.consumption = consumption;
        }
    }

    public float fuel_amount;
    public float lever_length;
    public int heat_time;
    public List<struct_rpms> rpms;

    public Engine_options_class(float fuel_amount, float lever_length, int heat_time)
    {
        this.fuel_amount = fuel_amount;
        this.lever_length = lever_length;
        this.heat_time = heat_time;
        rpms = new List<struct_rpms>();
    }

    public void Set_rpms(string [,] rpm_val)
    {
        rpms.Clear();
        for (int i = 0; i < rpm_val.GetLength(0); i++)
        {
            rpms.Add(new struct_rpms(int.Parse(rpm_val[i, 0]),
                float.Parse(rpm_val[i, 1]),
                float.Parse(rpm_val[i, 2])));
        }
    }

    public string[,] Get_rpms()
    {
        string[,] converted_rpms = new string[rpms.Count, 3];
        int i = 0;
        foreach (struct_rpms rpm in rpms)
        {
            converted_rpms[i, 0] = rpm.rpm.ToString();
            converted_rpms[i, 1] = rpm.moment.ToString();
            converted_rpms[i, 2] = rpm.consumption.ToString();
            i++;
        }
        return converted_rpms;
    }

    public float Get_moment_max()
    {
        float[] moments = new float[rpms.Count];
        int i = 0;
        foreach (struct_rpms rpm in rpms)
        {
            moments[i] = rpm.moment;
            i++;
        }
        return Mathf.Max(moments);
    }
}
