using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_main : MonoBehaviour
{
    private Transform options;
    private Transform password;

    private void Awake()
    {
        transform.Find("Main").Find("Quit").GetComponent<Button>().onClick.AddListener(() => Quit());

        Transform student = transform.Find("Main").Find("Student");
        student.Find("Simul").GetComponent<Button>().onClick.AddListener(() => Engine_stand());

        Transform teacher = transform.Find("Main").Find("Teacher");
        options = teacher.Find("Options");
        options.GetComponent<Button>().onClick.AddListener(() => Engine_options());

        password = teacher.Find("Password");
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
