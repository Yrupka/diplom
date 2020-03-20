using UnityEngine;
using UnityEngine.Events;

public class Item_button : Item_highligh
{

    private UnityAction click_action;

    private void OnMouseDown()
    {
        transform.localPosition -= new Vector3(0f, 0.03f, 0f);
    }

    private void OnMouseUp()
    {
        transform.localPosition += new Vector3(0f, 0.03f, 0f);
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
