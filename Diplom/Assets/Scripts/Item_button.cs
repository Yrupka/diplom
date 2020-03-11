using UnityEngine;
using UnityEngine.UI;

public class Item_button : Item_highligh
{

    private void OnMouseDown()
    {
        transform.localPosition -= new Vector3(0, 0, 0.03f);
    }

    private void OnMouseUp()
    {
        transform.localPosition += new Vector3(0, 0, 0.03f);
    }
}
