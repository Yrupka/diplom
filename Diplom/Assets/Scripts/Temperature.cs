using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Temperature : MonoBehaviour
{
    private TextMesh info;
    private int temp_curr;
    private int temp_max;
    private int step;
    private bool heated;
    private bool state;

    private UnityAction action_heated;

    private void Awake()
    {
        info = transform.Find("Info").GetComponent<TextMesh>();
        temp_curr = 25;
        temp_max = 70;
        heated = false; // двс нагрет до рабочей температуры
    }

    private void Update()
    {
        info.text = temp_curr.ToString();
        if (!heated && temp_curr == 70)
        {
            heated = true;
            action_heated();
        }
    }

    IEnumerator Heating()
    {
        while (state)
        {
            temp_curr += step;
            temp_curr = Mathf.Clamp(temp_curr, 25, temp_max);
            yield return new WaitForSeconds(1f); 
        }
    }

    IEnumerator Cooling()
    {
        while (!state)
        {
            yield return new WaitForSeconds(2f);
            if (heated)
                heated = false;
            temp_curr -= step;
            temp_curr = Mathf.Clamp(temp_curr, 25, temp_max);  
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
