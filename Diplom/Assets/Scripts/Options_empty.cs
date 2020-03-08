using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options_empty : MonoBehaviour
{
    private Transform options_empty;

    private void Awake()
    {
        options_empty = transform.Find("Options_empty");
        options_empty.Find("Button").GetComponent<Button>().onClick.AddListener(() => Main_menu());
        if (Save_controller.Load_engine_options() == null)
            options_empty.gameObject.SetActive(true);
    }

    private void Main_menu()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
