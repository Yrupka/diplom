using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Scroll_controller : MonoBehaviour
{
    public GameObject prefab;
    public string info1;
    public string info2;
    public string info3;
    public string info4;

    private RectTransform content;
    private List<GameObject> items_list; // хранение ячеек на сцене
    private Button add_button;
    private Button del_button;
    private int size = 0;

    private void Awake()
    {
        Transform window = transform.Find("Scroll_window");
        content = window.Find("Content").GetComponent<RectTransform>();
        items_list = new List<GameObject>();
        size = 0;
        add_button = transform.Find("Add_btn").GetComponent<Button>();
        del_button = transform.Find("Del_btn").GetComponent<Button>();
        transform.Find("Info1").GetComponent<Text>().text = info1;
        transform.Find("Info2").GetComponent<Text>().text = info2;
        transform.Find("Info3").GetComponent<Text>().text = info3;
        transform.Find("Info4").GetComponent<Text>().text = info4;
        add_button.onClick.AddListener(() => AddItem());
        del_button.onClick.AddListener(() => DeleteItem());
    }

    private void DeleteAll() // удаление всех объектов списка
    {
        foreach (GameObject go in items_list)
            Destroy(go);
        items_list = new List<GameObject>();
        size = 0;
    }

    private void AddItem()
    {
        Item item = new Item((size + 1).ToString());
        CreateItem(item);
    }

    private void DeleteItem()
    {
        if (size != 0)
        {
            Destroy(items_list[size - 1]);
            items_list.RemoveAt(size - 1);
            size--;
        }
    }

    public void AddMany(string[,] items)
    {
        DeleteAll();
        for (int i = 0; i < items.GetLength(0); i++)
        {
            Item item = new Item((i + 1).ToString(), items[i, 0], items[i, 1], items[i, 2]);
            CreateItem(item);
        }  
    }

    public string[,] GetItems()
    {
        string[,] items = new string[items_list.Count, 3];
        int i = 0;
        foreach (GameObject obj in items_list)
        {
            ItemModel item = new ItemModel(obj.transform);
            items[i, 0] = item.input1.text;
            items[i, 1] = item.input2.text;
            items[i, 2] = item.input3.text;
            i++;
        }
        return items;
    }

    private void CreateItem(Item item)
    {
        GameObject instanse = Instantiate(prefab); // клонируем префаб
        instanse.transform.SetParent(content, false); // устанавливаем клону родителя
        ItemModel model = new ItemModel(instanse.transform); // создаем по клону объект
        model.num.text = item.number;
        model.input1.text = item.first;
        model.input2.text = item.second;
        model.input3.text = item.third;
        items_list.Add(instanse);
        size++;
    }

    public class ItemModel
    {
        public Text num;
        public InputField input1;
        public InputField input2;
        public InputField input3;

        public ItemModel(Transform transform)
        {
            num = transform.Find("Text").GetComponent<Text>();
            input1 = transform.Find("Field_1").GetComponent<InputField>();
            input2 = transform.Find("Field_2").GetComponent<InputField>();
            input3 = transform.Find("Field_3").GetComponent<InputField>();
        }
    }

    public class Item
    {
        public string number;
        public string first;
        public string second;
        public string third;

        public Item(string par1, string par2, string par3, string par4)
        {
            number = par1;
            first = par2;
            second = par3;
            third = par4;
        }
        public Item(string num)
        {
            number = num;
            first = "0";
            second = "0";
            third = "0";
        }
    }
}

