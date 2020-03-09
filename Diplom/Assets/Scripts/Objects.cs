using UnityEngine;
using UnityEngine.EventSystems;

public class Objects : MonoBehaviour
{
    private void Awake()
    {
        Set_highlite(Color.clear);
    }

    private void OnMouseEnter()
    {
        Set_highlite(Color.blue);
    }
    private void OnMouseExit()
    {
        Set_highlite(Color.clear);
    }

    private void Set_highlite(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }
}
