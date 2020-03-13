using UnityEngine;

public class Item_grab : Item_highligh
{
    
    [SerializeField]
    private Transform pos_work; // рабочая позиция объекта
    private Transform pos_base; // не рабочая позиция объекта
    private bool item_in_work; // находится ли объект в процессе вычислений
    private bool on_work_pos; // находится ли объект в рабочей позиции

    private void Awake()
    {
        on_work_pos = false;
        pos_base = transform.parent;
    }

    private void OnMouseDown()
    {
        if (!item_in_work)
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
    }

    public bool On_work_position()
    {
        return on_work_pos;
    }

    public void Set_state(bool state) // если значение true, значит нельзя перемещать
    {
        item_in_work = state;
    }
}
