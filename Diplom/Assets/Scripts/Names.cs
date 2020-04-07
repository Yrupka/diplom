﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Names : MonoBehaviour
{
    [SerializeField]
    private Transform cam_obj;
    [SerializeField]
    private Transform text_prefab;
    [SerializeField]
    private Transform line_prefab;

    private List<Transform> clones_text;
    private List<Transform> clones_line;

    public List<Transform> objects;
    public List<string> names;

    private void Awake()
    {
        clones_text = new List<Transform>();
        clones_line = new List<Transform>();
        GetComponent<Toggle>().onValueChanged.AddListener(Toggle_clicked);
        Names_create();
    }

    private void Update()
    {
        for (int i = 0; i < clones_text.Count; i++)
        {
            clones_text[i].LookAt(cam_obj);
            clones_line[i].LookAt(cam_obj);
            clones_line[i].localEulerAngles = Vector3.Scale(clones_line[i].localEulerAngles, Vector3.up);
        }
    }

    private void Toggle_clicked(bool val)
    {
        if (val)
            Names_create();
        else
            Names_delete();
    }

    private void Names_create()
    {
        int count = 0;
        foreach (Transform obj in objects)
        {
            Transform text = Instantiate(text_prefab);
            TextMesh text_mesh = text.Find("Text").GetComponent<TextMesh>();
            text_mesh.text = names[count];
            text.SetParent(obj);
            text.localPosition = new Vector3(0f, 1.5f, 0f);
            clones_text.Add(text);

            float ch_size = text_mesh.characterSize;
            int ch_count = text_mesh.text.Length;
            float offset = ch_count / 2 * ch_size;

            Transform line = Instantiate(line_prefab);
            LineRenderer line_renderer = line.GetComponent<LineRenderer>();
            line.SetParent(obj);
            line.localPosition = Vector3.zero;
            line_renderer.SetPosition(0, Vector3.zero);
            line_renderer.SetPosition(1, text.localPosition - new Vector3(offset, ch_size, 0f));
            line_renderer.SetPosition(2, text.localPosition + new Vector3(offset, -ch_size, 0f));
            line.localScale = Vector3.one;
            clones_line.Add(line);
            count++;
        }
    }

    private void Names_delete()
    {
        foreach (Transform clone in clones_text)
        {
            Destroy(clone.gameObject);
        }
        foreach (Transform line in clones_line)
        {
            Destroy(line.gameObject);
        }
        clones_text.Clear();
        clones_line.Clear();
    }
}
