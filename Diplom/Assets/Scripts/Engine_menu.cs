using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engine_menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Transform timer;
    [SerializeField]
    private Transform scale;
    [SerializeField]
    private Transform hints;

    private Transform menu_tail;

    private void Awake()
    {
        transform.Find("Scale").GetComponent<Toggle>().onValueChanged.AddListener((value) => scale.gameObject.SetActive(value));
        transform.Find("Timer").GetComponent<Toggle>().onValueChanged.AddListener((value) => timer.gameObject.SetActive(value));
        transform.Find("Hints").GetComponent<Toggle>().onValueChanged.AddListener((value) => hints.gameObject.SetActive(value));
        menu_tail = transform.Find("Tail");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.position += new Vector3(130,0f);
        menu_tail.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        transform.position += new Vector3(-130, 0f);
        menu_tail.gameObject.SetActive(true);
    }
}
