using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField]
    private Item_grab glass;
    private TextMesh value_text;
    private float weight;
    private float zero_weight;
    private bool power_state;

    private void Awake()
    {
        zero_weight = 0f;
        weight = 0f;
        value_text = transform.Find("Value_text").GetComponent<TextMesh>();
        value_text.text = "1";
        transform.Find("Power_button").GetComponent<Item_button>().Add_listener(Power_state);
        transform.Find("Weight_zero").GetComponent<Item_button>().Add_listener(Weight_zero);
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

    public void Add_weight(float value)
    {
        weight += value;
        Debug.Log("addValue");
        Debug.Log(weight);
    }

    public bool Get_glass_state()
    {
        return glass.On_work_position();
    }

    public void Power_state()
    {
        power_state = !power_state;
        zero_weight = 0f;
        if (!power_state)
            value_text.text = "";
        Debug.Log("power");
        Debug.Log(power_state);
    }

    private void Weight_zero()
    {
        zero_weight = weight;
    }
}
