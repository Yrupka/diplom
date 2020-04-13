using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph_options : MonoBehaviour
{
    private Graph_script graph;

    private Dropdown dropdown;
    private Transform title;

    private labels[] graph_data;
    
    private int interpolation;

    private void Awake()
    {
        graph = transform.Find("Graph_window").GetComponent<Graph_script>();
        dropdown = transform.Find("Graph_list").GetComponent<Dropdown>();
        title = transform.Find("Graph_title");
        dropdown.AddOptions(new List<string>() { "Выберите..." , "График момента", "График мощности", "График расхода", "График удельного расхода" });
        dropdown.onValueChanged.AddListener(Graph_change);
        graph_data = new labels[4];
        for (int i = 0; i < 4; i++)
        {
            graph_data[i].label_x = new List<int>();
            graph_data[i].label_y = new List<float>();
        }
    }

    private void Graph_change(int value)
    {
        if (value == 0 || graph_data.Length == 0)
            title.gameObject.SetActive(true);
        else
        {
            value--;
            title.gameObject.SetActive(false);
            graph.Show_graph(graph_data[value].label_y, graph_data[value].label_x, interpolation + 1);
        }
    }

    private void Graph_points_calculation(List<int> x, List<float> y, int graph_num)
    {
        graph_data[graph_num].Clear();
        float dot_place_procent = 1f;
        if (interpolation != 0)
            dot_place_procent = 1f / interpolation;

        for (int j = 0; j < x.Count - 1; j++)
        {
            for (int i = 0; i <= interpolation; i++)
            {
                int calculated_rpm = (int)Mathf.Lerp(x[j], x[j + 1], i * dot_place_procent);
                graph_data[graph_num].label_x.Add(calculated_rpm);
                graph_data[graph_num].label_y.Add(Interpolate(calculated_rpm, x, y));
            }
        }
        graph_data[graph_num].label_x.Add(x[x.Count - 1]);
        graph_data[graph_num].label_y.Add(y[y.Count - 1]);
    }

    // функция вычисляющая интерполирующую функцию графика, label_x,y - значения исходной функции
    private float Interpolate(float x, List<int> label_x, List<float> label_y)
    {
        float answ = 0f;
        for (int j = 0; j < label_x.Count; j++)
        {
            float l_j = 1f;
            for (int i = 0; i < label_x.Count; i++)
            {
                if (i == j)
                    l_j *= 1f;
                else
                    l_j *= (x - label_x[i])/(label_x[j] - label_x[i]);
            }
            answ += l_j * label_y[j];
        }
        return answ;
    }

    private struct labels
    {
        public List<int> label_x;
        public List<float> label_y;

        public void Clear()
        {
            label_x.Clear();
            label_y.Clear();
        }
    }

    public void Calculate_graphs(Engine_options_class options, int graph_num)
    {
        List<int> label_x = options.Get_list_rpm();
        List<float> label_y_moment = options.Get_list_moment();
        List<float> label_y_consumption = options.Get_list_consumption();
        interpolation = options.interpolation;

        switch (graph_num)
        {
            case 0:
                Graph_points_calculation(label_x, label_y_moment, graph_num);
                break;
            case 1:
                List<float> power = label_y_moment; // мощность
                for (int i = 0; i < power.Count; i++)
                    power[i] *= label_x[i] / 9550f;
                Graph_points_calculation(label_x, power, graph_num);
                break;
            case 2:
                Graph_points_calculation(label_x, label_y_consumption, graph_num);
                break;
            case 3:
                Graph_points_calculation(label_x, label_y_moment, 0);
                List<float> power_all = label_y_moment; // мощность
                for (int i = 0; i < power_all.Count; i++)
                    power_all[i] *= label_x[i] / 9550f;
                Graph_points_calculation(label_x, power_all, 1);
                Graph_points_calculation(label_x, label_y_consumption, 2);

                List<float> specific_consumption = label_y_moment; // удельная расход
                for (int i = 0; i < specific_consumption.Count; i++)
                    specific_consumption[i] *= label_x[i] / 9550f / label_y_consumption[i] * 3.6f;
                Graph_points_calculation(label_x, specific_consumption, graph_num);
                break;
        }
        Graph_change(dropdown.value);
    }
}
