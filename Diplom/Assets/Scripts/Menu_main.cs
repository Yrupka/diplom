using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_main : MonoBehaviour
{
    private Transform options;
    private Transform password;
    private InputField password_input;
    private Transform options_app;

    private void Awake()
    {
        transform.Find("Main").Find("Quit").GetComponent<Button>().onClick.AddListener(Quit);
        transform.Find("Main").Find("Options").GetComponent<Button>().onClick.AddListener(Options_app);

        Transform student = transform.Find("Main").Find("Student");
        student.Find("Simul").GetComponent<Button>().onClick.AddListener(Engine_stand);

        Transform teacher = transform.Find("Main").Find("Teacher");
        options = teacher.Find("Options");
        options.GetComponent<Button>().onClick.AddListener(Engine_options);

        password = teacher.Find("Password");
        password_input = password.Find("Pass_input").GetComponent<InputField>();
        password.Find("Pass_button").GetComponent<Button>().onClick.AddListener(Password_check);

        options_app = transform.Find("Options");
    }

    private void Password_check()
    {
        if (password_input.text == "123")
        {
            options.gameObject.SetActive(true);
            password.gameObject.SetActive(false);
        }
        else
            password_input.text = "";
    }

   private void Engine_stand()
    {
        SceneManager.LoadScene("Engine_stand");
    }

    private void Engine_options()
    {
        SceneManager.LoadScene("Options");
    }

    private void Options_app()
    {
        options_app.gameObject.SetActive(true);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
