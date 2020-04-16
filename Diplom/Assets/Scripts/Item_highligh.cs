using UnityEngine;

public class Item_highligh : MonoBehaviour
{
    private Renderer rend;

    private void Awake()
    {
        rend = transform.GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        rend.material.color += Color.white;
    }
    private void OnMouseExit()
    {
        rend.material.color -= Color.white;
    }
}
