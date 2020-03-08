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
        dropdown.AddOptions(new List<string>() { "Выберите..." , "График момента", "График мощности", "График расхода", "График удельного расхода" });
        dropdown.onValueChanged.AddListener(delegate { Graph_change(dropdown.value); }); 
    }

    public void Value_update()
    {
        options = engine_options.Get_engine_options();
        if (options.rpms.Count == 0)
            title.gameObject.SetActive(true);
        else
        {
            title.gameObject.SetActive(false);
            Graph_change(dropdown.value);
        }  
    }

    private void Graph_change(int value)
    {
        List<float> list = new List<float>();
        switch (value)
        {
            case 1:
                for (int i = 0; i < options.rpms.Count; i++)
                    list.Add(options.rpms[i].moment);
                break;
            case 2:
                for (int i = 0; i < options.rpms.Count; i++)
                    list.Add(options.rpms[i].rpm * options.rpms[i].moment / 9550);
                break;
            case 3:
                for (int i = 0; i < options.rpms.Count; i++)
                    list.Add(options.rpms[i].consumption * 3.6f);
                break;
            case 4:
                for (int i = 0; i < options.rpms.Count; i++)
                    list.Add((options.rpms[i].rpm * options.rpms[i].moment / 9550) / (options.rpms[i].consumption * 3.6f));
                break;
        }
        if (list.Count != 0)
            graph.ShowGraph(list, -1, (int _i) => options.rpms[_i].rpm.ToString(), (float _i) => _i.ToString());
    }
}
