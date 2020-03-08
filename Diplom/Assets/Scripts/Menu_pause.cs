using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_pause : MonoBehaviour
{
    private bool isPaused = false;
    private Transform pause;

    private void Awake()
    {
        pause = transform.Find("Pause");
        Transform buttons = pause.Find("Buttons");
        buttons.Find("Back").GetComponent<Button>().onClick.AddListener(() => Resume());
        buttons.Find("Main").GetComponent<Button>().onClick.AddListener(() => Main_menu());
        buttons.Find("Quit").GetComponent<Button>().onClick.AddListener(() => Quit());
    }

    private void Start()
    {
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    private void Resume()
    {
        pause.gameObject.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    private void Pause()
    {
        pause.gameObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    private void Main_menu()
    {
        SceneManager.LoadScene("Main_menu");
    }

    private void Quit()
    {
        Application.Quit();
    }
}
