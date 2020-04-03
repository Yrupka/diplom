using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options_empty : MonoBehaviour
{
    [SerializeField]
    private Engine_controller engine_controller;
    [SerializeField]
    private Item_gas_tank item_gas_tank;

    private void Awake()
    {
        Transform options_empty = transform.Find("Options_empty");
        options_empty.Find("Button").GetComponent<Button>().onClick.AddListener(Main_menu);
        Engine_options_class options = Save_controller.Load_engine_options();
        if (options == null)
            options_empty.gameObject.SetActive(true);
        engine_controller.Load_options(options);
        item_gas_tank.Load_options(options);
    }

    private void Main_menu()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
