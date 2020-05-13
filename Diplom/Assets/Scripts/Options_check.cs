using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options_check : MonoBehaviour
{
    public Stand_controller engine_controller;
    public Item_gas_tank item_gas_tank;
    public Menu_interactive menu;

    private void Awake()
    {
        Transform options_empty = transform.Find("Options_empty");
        options_empty.Find("Button").GetComponent<Button>().onClick.AddListener(Main_menu);
        Engine_options options = File_controller.Load_one_profile();
        if (options == null)
            options_empty.gameObject.SetActive(true);
        else
        {
            if (options.rpms.Count == 0)
                options_empty.gameObject.SetActive(true);
            else
            {
                engine_controller.Load_options(options);
                item_gas_tank.Load_options(options.fuel_amount);
                menu.Load_options(options.hints, options.profile_show, options.car_name, options.engine_name);
            }
        }
    }

    private void Main_menu()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
