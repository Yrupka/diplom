using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[AddComponentMenu("My components/main")]
public class Engine_speed : MonoBehaviour
{
    public Animator animator;
    
    public Slider slider;

    private void Start()
    {
        animator.Play("main");
    }

    public void onChange()
    {
        animator.SetFloat("New Float", slider.value);
    }

}
