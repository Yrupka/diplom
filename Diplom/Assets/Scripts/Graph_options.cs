using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph_options : MonoBehaviour
{
    private Graph_script graph;

    private Dropdown dropdown;
    private Transform title;

    private List<int> interpolated_x;
    private List<float>[] graph_data;
    
    private int interpolation;

    private void Awake()
    {
        graph = transform.Find("Graph_window").GetComponent<Graph_script>();
        dropdown = transform.Find("Graph_list").GetComponent<Dropdown>();
        title = transform.Find("Graph_title");
        dropdown.AddOptions(new List<string>() {
            "Выберите..." , "Крутящий момент",
            "Мощность",  "Часовой расход топлива",
            "Удельный расход топлива" });
        dropdown.onValueChanged.AddListener(Graph_change);
        graph_data = new List<float>[4];
        for (int i = 0; i < 4; i++)
            graph_data[i] = new List<float>();
    }

    private void Graph_change(int value)
    {
        if (value == 0 || graph_data.Length == 0)
            title.gameObject.SetActive(true);
        else
        {
            value--;
            title.gameObject.SetActive(false);
            graph.Show_graph(interpolated_x, graph_data[value], interpolation + 1);
        }
    }

    public void Calculate_graphs(Engine_options_class options, int graph_num)
    {
        List<int> label_x = options.Get_list_rpm();
        interpolation = options.interpolation;
        interpolated_x = Calculation_formulas.Interpolated_x(
            options.Get_list_rpm(), interpolation);

        switch (graph_num)
        {
            case 0:
                graph_data[0] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), options.Get_list_moment(), interpolated_x);
                break;
            case 1:
                List<float> power = options.Get_list_moment(); // мощность
                for (int i = 0; i < power.Count; i++)
                    power[i] *= label_x[i] / 9550f;
                graph_data[1] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), power, interpolated_x);
                break;
            case 2:
                graph_data[2] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), options.Get_list_consumption(), interpolated_x);
                break;
            case 3:
                graph_data[0] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), options.Get_list_moment(), interpolated_x);
                List<float> power_all = options.Get_list_moment(); // мощность
                for (int i = 0; i < power_all.Count; i++)
                    power_all[i] *= label_x[i] / 9550f;
                graph_data[1] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), power_all, interpolated_x);
                graph_data[2] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), options.Get_list_consumption(), interpolated_x);

                List<float> specific_consumption = options.Get_list_moment(); // удельная расход
                List<float> label_y_consumption = options.Get_list_consumption();
                for (int i = 0; i < specific_consumption.Count; i++)
                    specific_consumption[i] *= label_x[i] / 9550f / label_y_consumption[i] * 3.6f;
                graph_data[3] = Calculation_formulas.Interpolated_y(
                    options.Get_list_rpm(), specific_consumption, interpolated_x);
                break;
        }
        Graph_change(dropdown.value);
    }

    public void Clear_graphs()
    {
        dropdown.value = 0;
        for (int i = 0; i < 4; i++)
            graph_data[i].Clear();
    }

    public float Get_max_moment()
    {
        return Mathf.Max(graph_data[0].ToArray());
    }
}
