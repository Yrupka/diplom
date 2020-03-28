using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engine_menu : MonoBehaviour
{

    [SerializeField]
    private Transform timer;
    [SerializeField]
    private Transform scale;

    private Transform menu_tail;
    private Vector3 menu_pos_old;
    private Vector3 menu_pos_new;

    private void Awake()
    {
        transform.Find("Scale").GetComponent<Toggle>().onValueChanged.AddListener((value) => scale.gameObject.SetActive(value));
        transform.Find("Timer").GetComponent<Toggle>().onValueChanged.AddListener((value) => timer.gameObject.SetActive(value));
        menu_tail = transform.Find("Tail");
        menu_pos_old = transform.position;
        menu_pos_new = transform.position;
        menu_pos_new.x = 100;
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    transform.position = menu_pos_new;
    //    menu_tail.gameObject.SetActive(false);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    transform.position = menu_pos_old;
    //    menu_tail.gameObject.SetActive(true);
    //}
}
