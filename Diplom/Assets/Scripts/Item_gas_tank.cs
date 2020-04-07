using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Item_gas_tank : Item_highligh
{
    private UnityAction click_action;
    private Animator anim;
    private Engine_options_class options;

    private float fuel_add_amount;
    private bool interactable;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0.3f);
        if (options != null)
            fuel_add_amount = options.fuel_amount;
        interactable = true;
    }

    private void OnMouseUp()
    {
        if (interactable)
            click_action();
    }

    IEnumerator Animation() // для предотвращения нажатия на объект во время анимации
    {
        interactable = false;
        anim.Play("Fuel_add");
        yield return new WaitForSeconds(3f);
        interactable = true;
    }

    public void Load_options(Engine_options_class loaded_options) // получить загруженные данные
    {
        options = loaded_options;
    }

    public void Play_animation()
    {
        StartCoroutine(Animation());
    }

    public float Get_fuel()
    {
        return fuel_add_amount;
    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }
}
