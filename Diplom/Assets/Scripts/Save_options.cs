using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_options : MonoBehaviour
{
    private List<Engine_options_class> options;
    public Engine_options engine_options;

    private Transform profiles_list;
    private InputField profile_name;
    private Text profile_current;
    private Button btn_delete;
    private Button btn_change;
    private List<Toggle> items_list;
    public GameObject profile;

    private int current_profile_index;

    private void Awake()
    {
        profiles_list = transform.Find("Scroll").Find("Scroll_window").Find("Content");
        profile_name = transform.Find("Input_name").GetComponent<InputField>();
        profile_current = transform.Find("Text_current").GetComponent<Text>();
        transform.Find("Add").GetComponent<Button>().onClick.AddListener(Profile_add);
        btn_delete = transform.Find("Delete").GetComponent<Button>();
        btn_delete.onClick.AddListener(Profile_delete);
        btn_change = transform.Find("Change").GetComponent<Button>();
        btn_change.onClick.AddListener(Profile_choose);
        items_list = new List<Toggle>();
        current_profile_index = -1;

        Profiles_load();
    }

    private void Start() // необходимо для инициализации окна с настройками параметров
    {
        engine_options.transform.parent.parent.gameObject.SetActive(false);
        engine_options.Add_listener_closed(Profile_update);
    }

    private void OnDestroy()
    {
        if (current_profile_index == -1)
            current_profile_index = 0;
        options.Insert(0, options[current_profile_index]);
        options.RemoveAt(current_profile_index + 1);

        Save_controller.Save_engine_options(options);
    }

    private void Profiles_load()
    {
        options = Save_controller.Load_engine_options();
        if (options != null)
        {
            foreach (Engine_options_class item in options)
                Item_add(item.car_name + "-" + item.engine_name);
            Profile_clicked(true, 0); // выбрать первый профиль как основной
            items_list[0].isOn = true;
        }
        else
            options = new List<Engine_options_class>();
    }

    private void Profile_add()
    {
        string created_name = profile_name.text;
        if (!string.IsNullOrWhiteSpace(created_name))
        {
            Item_add("-" + created_name);
            options.Add(new Engine_options_class(created_name, ""));
            Data_update(current_profile_index);
        }
    }

    private void Item_add(string name)
    {
        GameObject instanse = Instantiate(profile);
        instanse.transform.SetParent(profiles_list, false);
        instanse.transform.Find("Label").GetComponent<Text>().text = name;
        int index = items_list.Count;
        instanse.GetComponent<Toggle>().onValueChanged.AddListener((val) => Profile_clicked(val, index));
        items_list.Add(instanse.GetComponent<Toggle>());
    }

    private void Profile_delete()
    {
        Destroy(items_list[current_profile_index].gameObject);
        items_list.RemoveAt(current_profile_index);
        options.RemoveAt(current_profile_index);

        for (int i = current_profile_index; i < items_list.Count; i++)
        {
            items_list[i].onValueChanged.RemoveAllListeners();
            int index = i;
            items_list[i].onValueChanged.AddListener((val) => Profile_clicked(val, index));
        }
        current_profile_index = -1;
        Data_update(current_profile_index);
    }

    private void Profile_clicked(bool state, int index)
    {
        if (state)
        {
            for (int i = 0; i < items_list.Count; i++)
                if (i != index)
                    items_list[i].isOn = false;
            current_profile_index = index;
            Data_update(current_profile_index);
        }

        // нельзя отменить выбор выбранного профиля
        int active_index = items_list.FindIndex(x => x.isOn == true);
        if (active_index == -1)
            items_list[index].isOn = true;
    }

    private void Profile_choose()
    {
        engine_options.Set_profile(options[current_profile_index]);
        engine_options.transform.parent.parent.gameObject.SetActive(true);
        transform.parent.parent.gameObject.SetActive(false);
    }

    private void Profile_update()
    {
        transform.parent.parent.gameObject.SetActive(true);
        options[current_profile_index] = engine_options.Get_profile();
        items_list[current_profile_index].transform.Find("Label").GetComponent<Text>().text =
            options[current_profile_index].car_name + "-" + options[current_profile_index].engine_name;
    }

    private void Data_update(int index)
    {
        if (index == -1)
        {
            profile_current.text = "Выберите профиль";
            btn_delete.interactable = false;
            btn_change.interactable = false;
        }
        else
        {
            profile_current.text = options[index].car_name + "-" + options[index].engine_name;
            btn_change.interactable = true;
            if (options.Count != 1) // один профиль должен быть
                btn_delete.interactable = true;
            else
                btn_delete.interactable = false;
        }
    }
}
