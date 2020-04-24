using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engine_menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Transform menu_tail;

    public Transform timer;
    public Transform scale;
    public Transform hints;

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
