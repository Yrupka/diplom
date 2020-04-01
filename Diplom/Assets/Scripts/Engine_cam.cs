using UnityEngine;

public class Engine_cam : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursor_texture;
    private Vector3 transfer;

    private float speed = 5f;
    private bool is_locked; // заблокирован ли курсор (нет - перемещение в 2д)

    void Awake()
    {
        Cursor.SetCursor(cursor_texture, Vector2.zero ,CursorMode.Auto);
        is_locked = true;
        GetComponent<Rigidbody>().freezeRotation = true;
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
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            if (h != 0 || v != 0)
                transform.eulerAngles += new Vector3(-v, h, 0) * speed;

            // перемещение камеры
            transfer = transform.forward * Input.GetAxis("Vertical");
            transfer += transform.right * Input.GetAxis("Horizontal");
            transform.position += transfer * speed * Time.deltaTime;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else // режим выбора в 2д
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}