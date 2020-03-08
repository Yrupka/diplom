using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_main : MonoBehaviour
{
    private Transform main;
    private Transform options;
    private Transform password;

    private void Awake()
    {
        main = transform.Find("Main");
        main.Find("Simul").GetComponent<Button>().onClick.AddListener(() => Engine_stand());
        main.Find("Quit").GetComponent<Button>().onClick.AddListener(() => Quit());
        Transform options_obj = main.Find("Options_obj");
        options = options_obj.Find("Options");
        options.GetComponent<Button>().onClick.AddListener(() => Engine_options());
        password = options_obj.Find("Password");
        password.Find("Pass_button").GetComponent<Button>().onClick.AddListener(() => Password_check());
    }

    private void Password_check()
    {
        if (password.Find("Pass_input").GetComponent<InputField>().text == "123")
        {
            options.gameObject.SetActive(true);
            password.gameObject.SetActive(false);
        }
    }

    private void Quit()
    {
        Application.Quit();
    }

   private void Engine_stand()
    {
        SceneManager.LoadScene("Engine_stand");
    }

    private void Engine_options()
    {
        SceneManager.LoadScene("Engine_options");
    }
}
