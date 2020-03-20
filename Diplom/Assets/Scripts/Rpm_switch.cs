using UnityEngine;

public class Rpm_switch : MonoBehaviour
{
    private float val;

    private void Awake()
    {
        val = -120f;
    }

    private void OnMouseDrag()
    {

        // смещение мыши за кадр
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        // создание луча от камеры до мыши
        Vector2 mouse = Input.mousePosition;
        Ray ray;
        ray = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        // получение объекта над которым находится мышь
        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.point.x < transform.position.x)
                dy *= 50; // если мышь находится слева от объекта
            else
                dy *= -50; // если мышь находится справа от объекта
        }

        dx *= 50; // 50 - значение чувствительности

        val += dx + dy;
        val = Mathf.Clamp(val, -120f, 120f); // ограничение угла вращения от -120 градусов до 120

        transform.localEulerAngles = new Vector3(0f, val, -180f); // поворот на заданный угол
    }

    public int Get_rpm()
    {
        float procent = (val + 120) / 240f;
        return (int)Mathf.Lerp(1000f, 7000f, procent);
    }
}
