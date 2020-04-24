using UnityEngine;
using UnityEngine.UI;

public class UI_scale : MonoBehaviour
{
    public Scale scale;

    private Text weight_value;
   
    private void Awake()
    {
        weight_value = transform.Find("Value").GetComponent<Text>();
    }

    private void Update()
    {
        weight_value.text = scale.Get_weight();
    }
}
