using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Temperature : MonoBehaviour
{
    private TextMesh info;
    private int temp_curr;
    private int temp_max;
    private int step;
    private bool state;

    private UnityAction action_heated;

    private void Awake()
    {
        info = transform.Find("Info").GetComponent<TextMesh>();
        temp_curr = 25;
        temp_max = 70;
        info.text = temp_curr.ToString();
    }

    IEnumerator Heating()
    {
        // задержка нужна для того, чтобы двигатель успел проверить наличие топлива, а не сразу начал греться
        yield return new WaitForSeconds(1f); 
        while (state && temp_curr != 70)
        {
            temp_curr += step;
            temp_curr = Mathf.Clamp(temp_curr, 25, temp_max);
            info.text = temp_curr.ToString();
            yield return new WaitForSeconds(1f);
        }
        if (temp_curr == 70)
            action_heated();
    }

    IEnumerator Cooling()
    {
        while (!state && temp_curr != 25)
        {
            yield return new WaitForSeconds(2f);
            temp_curr -= step;
            temp_curr = Mathf.Clamp(temp_curr, 25, temp_max);
            info.text = temp_curr.ToString();
        }
    }

    public void Heat_time_set(int val)
    {
        step = temp_max / val;
    }

    public void Heat(bool value)
    {
        state = value;
        if (state)
            StartCoroutine(Heating());
        else
            StartCoroutine(Cooling());
    }

    public float Penalty()
    {
        return 2 - Mathf.InverseLerp(25, 70, temp_curr); ;
    }

    public void Add_listener_heated(UnityAction action)
    {
        action_heated += action;
    }
}
