using UnityEngine;

public class Item_grab : Item_highligh
{
    
    [SerializeField]
    private Transform pos_work;
    private Transform pos_base;
    private bool on_work_pos; // находится ли объект в рабочей позиции

    private void Awake()
    {
        on_work_pos = false;
        pos_base = transform.parent;
    }

    private void OnMouseDown()
    {
        if (on_work_pos) // если стоит в рабочей зоне
        {
            transform.SetParent(pos_base);
            transform.localPosition = new Vector3(0, 0, 0);
            on_work_pos = false;
        }
        else
        {
            transform.SetParent(pos_work);
            transform.localPosition = new Vector3(0, 0, 0);
            on_work_pos = true;
        }
    }

    public bool On_work_position()
    {
        return on_work_pos;
    }
}
