using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField]
    private string value_name;

    private const float max_speed_angle = -120; // угол максимального значения скорости
    private const float min_speed_angle = 120; // угол минимального значения скорости
    private const int label_amount = 7; // количество подписей

    private float speed; // значение скорости
    private float max_speed; // максимальное значение скорости

    private Transform needle;
    private Transform label;
    private Transform value;
    private Transform info;

    void Awake()
    {
        needle = transform.Find("Needle");
        label = transform.Find("Label");
        info = transform.Find("Info");
        value = transform.Find("Value_info").Find("Value");
        info.GetComponent<Text>().text = value_name;
        label.gameObject.SetActive(false);
    }

    private void Start()
    {
        Create_labels();
    }

    void Update()
    {
        speed = Mathf.Clamp(speed, 0f, max_speed);
        needle.eulerAngles = new Vector3(0, 0, Get_rotation());
        value.GetComponent<Text>().text = speed.ToString();
    }

    public void Set_speed(float val)
    {
        speed = val;
    }

    public void Set_speed_max(float max_val)
    {
        max_speed = max_val;
    }

    private float Get_rotation()
    {
        float total_angle = min_speed_angle - max_speed_angle;
        float speed_norm = speed / max_speed;
        return min_speed_angle - speed_norm * total_angle;
    }

    private void Create_labels()
    {
        float total_angle = min_speed_angle - max_speed_angle;

        for (int i = 0; i <= label_amount; i++) 
        {
            Transform speed_label = Instantiate(label, transform);
            float label_norm = (float)i / label_amount;
            float label_angle = min_speed_angle - label_norm * total_angle;
            speed_label.eulerAngles = new Vector3(0, 0, label_angle);
            speed_label.Find("Text").GetComponent<Text>().text = Mathf.RoundToInt(label_norm * max_speed).ToString();
            speed_label.Find("Text").eulerAngles = Vector3.zero;
            speed_label.gameObject.SetActive(true);
        }
        needle.SetAsLastSibling(); // стрелка поверх циферблата
    }
}
