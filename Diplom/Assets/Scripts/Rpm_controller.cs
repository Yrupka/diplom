using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpm_controller : MonoBehaviour
{
    private Transform ddd;
    private void Awake()
    {
        ddd = transform.Find("Cd");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ddd.localPosition += new Vector3(0.01f, 0f, 0f);
        }
        ddd.localPosition += new Vector3(-0.001f, 0f, 0f);
    }
}
