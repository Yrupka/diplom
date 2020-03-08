using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engine_menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Transform engine_controller;

    private Transform menu_tail;
    private Vector3 menu_pos_old;
    private Vector3 menu_pos_new;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.position = menu_pos_new;
        menu_tail.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.position = menu_pos_old;
        menu_tail.gameObject.SetActive(true);
    }

    private void Awake()
    {
        transform.Find("Controller").GetComponent<Toggle>().onValueChanged.AddListener((value) => engine_controller.gameObject.SetActive(value));
        menu_tail = transform.Find("Menu_tail");
        menu_pos_old = transform.position;
        menu_pos_new = transform.position;
        menu_pos_new.x = 100;
    }
}
