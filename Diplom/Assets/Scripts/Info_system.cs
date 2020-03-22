using UnityEngine;

public class Info_system : MonoBehaviour
{
    private Renderer temp;
    private Renderer fuel;

    private void Awake()
    {
        temp = transform.Find("Temp").GetComponent<Renderer>();
        fuel = transform.Find("Fuel").GetComponent<Renderer>();
        temp.material.color = Color.clear;
        fuel.material.color = Color.clear;
    }

    public void Fuel_on()
    {
        fuel.material.color = Color.white;
    }

    public void Fuel_off()
    {
        fuel.material.color = Color.clear;
    }

    public void Temp_on()
    {
        temp.material.color = Color.white;
    }

    public void Temp_off()
    {
        temp.material.color = Color.clear;
    }
}
