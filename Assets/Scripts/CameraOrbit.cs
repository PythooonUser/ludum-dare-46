using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] float speed = 4f;

    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        transform.LookAt(Vector3.zero);
    }
}
