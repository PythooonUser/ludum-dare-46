using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] float lookSensitivity = 1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Vector3 GetMoveInput()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        return input;
    }

    public float GetLookInputHorizontal()
    {
        float input = Input.GetAxisRaw("Mouse X");
        input *= lookSensitivity;
        return input;
    }

    public float GetLookInputVertical()
    {
        float input = -Input.GetAxisRaw("Mouse Y");
        input *= lookSensitivity;
        return input;
    }
}
