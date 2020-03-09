using UnityEngine;
using System.Collections;

public class Engine_cam : MonoBehaviour
{

    // Присваиваем переменные
    public float mouseSensitivity = 10f;
    public float speed = 5f;
    private Vector3 transfer;
    public float minimumX = -360f;
    public float maximumX = 360f;
    public float minimumY = -60f;
    public float maximumY = 60f;
    float rotationX = 0f;
    float rotationY = 0f;
    Quaternion originalRotation;


    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Движения мыши -> Вращение камеры
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.rotation = originalRotation * xQuaternion * yQuaternion;

        //// Ускорение при нажатии клавиши Shift
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //    speed *= 10;
        //else if (Input.GetKeyUp(KeyCode.LeftShift))
        //    speed /= 10;

        //// Поднятие и опускание камеры
        //Vector3 newPos = new Vector3(0, 1, 0);
        //if (Input.GetKey(KeyCode.E))
        //    transform.position += newPos * speed * Time.deltaTime;
        //else if (Input.GetKey(KeyCode.Q))
        //    transform.position -= newPos * speed * Time.deltaTime;

        // перемещение камеры
        transfer = transform.forward * Input.GetAxis("Vertical");
        transfer += transform.right * Input.GetAxis("Horizontal");
        transform.position += transfer * speed * Time.deltaTime;
    }
}