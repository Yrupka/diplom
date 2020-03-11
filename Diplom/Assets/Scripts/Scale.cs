using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
    private Item_button power_button;
    private Item_button weight_zero_button;
    private TextMesh value_text;
    private float weight;
    private float zero_weight;
    private bool power_state;

    private void Awake()
    {
        power_button = transform.Find("Weight_count").GetComponent<Item_button>();
        weight_zero_button = transform.Find("Weight_zero").GetComponent<Item_button>();
        value_text = transform.Find("Value_text").GetComponent<TextMesh>();
        power_button.GetComponent<Button>().onClick.AddListener(delegate { power_state = !power_state; });
        weight_zero_button.GetComponent<Button>().onClick.AddListener(Weight_zero);
        power_state = false;
    }
    private void Update()
    {
        if (power_state)
        {
            if (zero_weight != 0)
                value_text.text = (weight - zero_weight).ToString();
            else
                value_text.text = weight.ToString();
        }
    }

    public void Set_weight(float value)
    {
        weight = value;
    }

    private void Weight_zero()
    {
        zero_weight = weight;
    }
}
