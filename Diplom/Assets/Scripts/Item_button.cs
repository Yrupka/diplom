using UnityEngine;
using UnityEngine.Events;

public class Item_button : Item_highligh
{

    private UnityAction click_action;

    private void OnMouseDown()
    {
        transform.localPosition -= new Vector3(0, 0, 0.03f);
    }

    private void OnMouseUp()
    {
        transform.localPosition += new Vector3(0, 0, 0.03f);
    }

    private void OnMouseUpAsButton()
    {
        click_action();
    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }
}
