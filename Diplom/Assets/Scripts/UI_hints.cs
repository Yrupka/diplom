using UnityEngine;
using UnityEngine.UI;

public class UI_hints : MonoBehaviour
{
    [SerializeField]
    private Fuel_controller fuel_controller;

    private Text text;

    private string[] texts;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();

        texts = new string[4];
        texts[0] = "Для начала работы, переместите мерный стакан со стола на весы";
        texts[1] = "Необходимо налить топливо в стакан, для этого нужно нажать на канистру, стоящую на столе." +
            "После наполнения стакана, необходимо нажать на устройство подачи топлива.";
        texts[2] = "Можно заводить двигатель, необходимо повернуть левый переключатель на положение '3' и отпустить.";
        texts[3] = "Можно приступать к работе, для удобства, в меню присутствуют секундомер и окно отображающее показания весов.";
    }

    private void Update()
    {
        text.text = texts[fuel_controller.State()];
    }
}
