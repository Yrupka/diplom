using UnityEngine;
using UnityEngine.UI;

public class Scale_obj : MonoBehaviour
{

    private Transform fuel_tank;
    private Animator fuel_tank_animation;
    private Scale scale;

    private void Awake()
    {
        fuel_tank = transform.Find("Fuel_tank");
        fuel_tank.GetComponent<Item_button>().Add_listener(Add_fuel);
        fuel_tank_animation = fuel_tank.GetComponent<Animator>();
        fuel_tank_animation.speed = 0.1f;
        scale = transform.Find("Scale").GetComponent<Scale>();
    }

    private void Add_fuel()
    {
        if (scale.Get_glass_state())
        {
            scale.Add_weight(30f); //!--
            
            fuel_tank_animation.Play("Fuel_tank_act");
        }
    }
}
