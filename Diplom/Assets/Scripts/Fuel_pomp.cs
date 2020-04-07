using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Fuel_pomp : Item_highligh
{
    private UnityAction click_action;
    private Animator anim;

    private bool work_position;
    private bool interactable;

    private void Start()
    {
        anim = GetComponent<Animator>();
        work_position = false;
        interactable = true;
    }

    private void OnMouseUp()
    {
        if (interactable)
            click_action();
    }

    IEnumerator Animation(string anim_name) // для предотвращения нажатия на объект во время анимации
    {
        interactable = false;
        anim.Play(anim_name);
        yield return new WaitForSeconds(1f);
        interactable = true;
    }

        public void Set_state(bool state)
    {
        work_position = state;
    }

    public bool State_info()
    {
        return work_position;
    }

    public void Play_animation()
    {
        if (work_position)
            StartCoroutine(Animation("Work_unset"));
        else
            StartCoroutine(Animation("Work_set"));

    }

    public void Add_listener(UnityAction call)
    {
        click_action += call;
    }
}
