using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    private float val;
    private bool started;
    private List<float> positions;

    private void Awake()
    {
        positions = new List<float>(3){-120f, -40f, 40f};
        val = -120f;
        started = false;
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
                dy *= 50f; // если мышь находится слева от объекта
            else
                dy *= -50f; // если мышь находится справа от объекта
        }

        dx *= 50f; // 50 - значение чувствительности

        val += dx + dy;
        val = Mathf.Clamp(val, -120f, 120f); // ограничение угла вращения от -120 градусов до 120

        transform.localEulerAngles = new Vector3(-90f, 0, val); // поворот на заданный угол
    }

    private void OnMouseUp()
    {

        int index = positions.BinarySearch(val); // поиск ближайшего положения
        
        if (index == -4)
        {
            started = true; // двигатель завели
            val = positions[2];
        }
        else
        {
            if (index < 0) // если ключ в промежтке между положениями
            {
                index = index * (-1) - 1;
                if (positions[index] - 40 < val) // к какому положению ближе, на то и будет установлен
                    val = positions[index];
                else
                    val = positions[index - 1];
            }
            else
                val = positions[index];

            if (val < 40)
                started = false;
        }

        transform.localEulerAngles = new Vector3(-90f, 0, val); // поворот на заданный угол
    }

    public bool Engine_state()
    {
        return started;
    }
}
