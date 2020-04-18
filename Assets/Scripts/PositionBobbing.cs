using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBobbing : MonoBehaviour
{
    [SerializeField] float verticalBobFrequency = 1f;
    [SerializeField] float bobbingAmount = 0.5f;

    [Header("References")]
    [SerializeField] PlayerCharacterController characterController = default;

    Vector3 m_startPosition;

    void Start()
    {
        m_startPosition = transform.localPosition;
    }

    void Update()
    {
        float magnitude = characterController.GetMoveVelocityMagnitude();

        if (magnitude == 0f) { return; }

        float delta = ((Mathf.Cos(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount * Mathf.Clamp(magnitude, 0f, 1f);
        transform.localPosition = m_startPosition + transform.up * delta;
    }
}
