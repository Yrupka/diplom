using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    public Texture2D cursor_texture;
    public Menu_options options;
    private Vector3 transfer;

    private float speed;
    private bool is_locked; // заблокирован ли курсор (нет - перемещение в 2д)

    private void Awake()
    {
        speed = 5f;
        is_locked = false;
        options.Add_mouse_sens_listener(Set_mouse_speed);
        GetComponent<Rigidbody>().freezeRotation = true;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            is_locked = false;

        if (Input.GetMouseButtonDown(1)) // нажата правая кнопка мыши
            is_locked = !is_locked;

        if (is_locked) // режим полета в 3д
        {
            // повороты мыши
            float mouse_x = Input.GetAxis("Mouse X");
            float mouse_y = Input.GetAxis("Mouse Y");
            transform.eulerAngles += new Vector3(-mouse_y, mouse_x, 0) * speed;

            // перемещение камеры
            transfer = transform.forward * Input.GetAxis("Vertical");
            transfer += transform.right * Input.GetAxis("Horizontal");
            transform.position += transfer * 3f * Time.deltaTime;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.SetCursor(cursor_texture, new Vector2(15f, 15f), CursorMode.Auto);
        }
        else // режим выбора в 2д
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void Set_mouse_speed()
    {
        speed = options.Get_sens();
    }
}