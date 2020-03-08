using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine_options : MonoBehaviour
{
    [SerializeField]
    private Graph_options graph_options;

    private InputField input_m;
    private Scroll_controller scroll;
    private Engine_options_class options;

    private delegate void MethodContainer();
    private event MethodContainer value_update;

    private void Awake() // нахождение всех полей
    {
        scroll = transform.Find("Scroll").GetComponent<Scroll_controller>();
        input_m = transform.Find("Input1").GetComponent<InputField>();
        transform.Find("Save_button").GetComponent<Button>().onClick.AddListener(() => Confirm_button());
        transform.Find("Load_button").GetComponent<Button>().onClick.AddListener(() => Load());
        value_update += graph_options.Value_update;
    }

    private void Start()
    {
        Load();
    }

    public void Confirm_button() // заполенение нулями пустых полей
    {
        if (string.IsNullOrEmpty(input_m.text))
            input_m.text = "0";

        options = new Engine_options_class();
        options.fuel_amount = int.Parse(input_m.text);
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
        options = Save_controller.Load_engine_options();
        if (options != null)
        {
            input_m.text = options.fuel_amount.ToString();
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

        public struct_rpms(int rpm_val, float moment_val, float consumption_val)
        {
            rpm = rpm_val;
            moment = moment_val;
            consumption = consumption_val;
        }
    }

    public float fuel_amount;
    public List<struct_rpms> rpms;

    public Engine_options_class()
    {
        fuel_amount = 0f;
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
