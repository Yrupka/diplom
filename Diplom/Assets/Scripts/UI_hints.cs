using UnityEngine;
using UnityEngine.UI;

public class UI_hints : MonoBehaviour
{
    [SerializeField]
    private Fuel_controller fuel_controller;

    private Text text_info;
    private RectTransform background;

    private string[] texts;

    private void Awake()
    {
        text_info = transform.Find("Text").GetComponent<Text>();
        background = transform.Find("Background").GetComponent<RectTransform>();
        fuel_controller.Add_listener_state(Text_update);
    }

    private void Text_update()
    {
        text_info.text = texts[fuel_controller.State()];
        background.sizeDelta = new Vector2(200, text_info.preferredHeight + 5);
    }

    public void Load_options(string[] loaded_options) // получить загруженные данные
    {
        texts = loaded_options;
        Text_update();
    }
}
