using UnityEngine;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    private string value_name;

    private const float max_speed_angle = -126; // угол максимального значения скорости
    private const float min_speed_angle = 126; // угол минимального значения скорости
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
        value = transform.Find("Value");
        info.GetComponent<TextMesh>().text = value_name;
        label.gameObject.SetActive(false);
    }

    private void Start()
    {
        Create_labels();
    }

    void Update()
    {
        speed = Mathf.Clamp(speed, 0f, max_speed);
        needle.eulerAngles = new Vector3(90, 0, Get_rotation());
        value.GetComponent<TextMesh>().text = speed.ToString();
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
        float radius = Mathf.Sqrt(Mathf.Pow(label.localPosition.x, 2) + Mathf.Pow(label.localPosition.z, 2));

        for (int i = 0; i <= label_amount; i++) 
        {
            Transform speed_label = Instantiate(label, transform);
            float label_norm = (float)i / label_amount;
            float label_angle = min_speed_angle - label_norm * total_angle;
            speed_label.localPosition = new Vector3(-1 * radius * Mathf.Cos(label_angle * Mathf.Deg2Rad), 0.11f, -1 * radius * Mathf.Sin(label_angle * Mathf.Deg2Rad));
            speed_label.GetComponent<TextMesh>().text = Mathf.RoundToInt(label_norm * max_speed).ToString();
            speed_label.gameObject.SetActive(true);
        }
        needle.SetAsLastSibling(); // стрелка поверх циферблата
    }
}
