using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] float lookSensitivity = 1f;

    bool m_canInteract = true;

    void Start()
    {
        SetMenuInteractivity(false);
    }

    public Vector3 GetMoveInput()
    {
        if (!m_canInteract) { return Vector3.zero; }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        return input;
    }

    public float GetLookInputHorizontal()
    {
        if (!m_canInteract) { return 0f; }

        float input = Input.GetAxisRaw("Mouse X");
        input *= lookSensitivity;
        return input;
    }

    public float GetLookInputVertical()
    {
        if (!m_canInteract) { return 0f; }

        float input = -Input.GetAxisRaw("Mouse Y");
        input *= lookSensitivity;
        return input;
    }

    public void SetMenuInteractivity(bool activate)
    {
        if (activate)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        m_canInteract = !activate;
    }
}
