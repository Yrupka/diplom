using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph_options : MonoBehaviour
{
    [SerializeField]
    private Engine_options engine_options;

    private Engine_options_class options;

    private Graph_script graph;
    private Dropdown dropdown;
    private Transform title;

    private void Awake()
    {
        graph = transform.Find("Graph_window").GetComponent<Graph_script>();
        dropdown = transform.Find("Graph_list").GetComponent<Dropdown>();
        title = transform.Find("Graph_title");
        dropdown.AddOptions(new List<string>() { "Выберите..." , "График мощности", "График момента", "График расхода" });
        dropdown.onValueChanged.AddListener(delegate { Graph_change(dropdown.value); }); 
    }

    public void Value_update()
    {
        options = engine_options.Get_engine_options();
        if (options == null)
            title.gameObject.SetActive(true);
        else
            title.gameObject.SetActive(false);
        
        Graph_change(dropdown.value);
    }

    private void Graph_change(int value)
    {
        switch (value)
        {
            case 1:
                Show_graph_N();
                break;
            case 2:
                Show_graph_m();
                break;
        }
    }

    private void Show_graph_N()
    {
        List<float> list = new List<float>();
        for (int i = 0; i < options.rpms.Count; i++)
            list.Add(options.rpms[i].rpm * options.rpms[i].moment / 9550);

        graph.ShowGraph(list, -1, (int _i) => options.rpms[_i].rpm.ToString(), (float _i) => _i.ToString());
    }

    private void Show_graph_m()
    {
        List<float> list = new List<float>();
        for (int i = 0; i < options.rpms.Count; i++)
            list.Add(options.rpms[i].consumption);

        graph.ShowGraph(list, -1, (int _i) => options.rpms[_i].rpm.ToString(), (float _i) => _i.ToString());
    }

}
