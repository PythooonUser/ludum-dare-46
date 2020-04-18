using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Posture")]
    [SerializeField] float characterHeight = 1.8f;
    [SerializeField] float cameraHeightPercent = 0.9f;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] LayerMask groundCheckLayerMask = -1;
    [SerializeField] float groundCheckDistance = 1f;

    [Header("References")]
    [SerializeField] PlayerInputHandler inputHandler = default;
    [SerializeField] CharacterController characterController = default;
    [SerializeField] new Camera camera = default;

    float m_cameraVerticalAngle = 0f;
    Vector3 m_moveVelocity = Vector3.zero;
    Vector3 m_smoothVelocity = Vector3.zero;

    void Start()
    {
        SetCharacterHeight();
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleVerticalLook();
        HandleHorizontalLook();
    }

    void SetCharacterHeight()
    {
        characterController.height = characterHeight;
        characterController.center = Vector3.up * characterHeight * 0.5f;

        camera.transform.localPosition = Vector3.up * characterHeight * cameraHeightPercent;
    }

    void HandleGroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundCheckLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.distance > characterController.skinWidth)
            {
                characterController.Move(Vector3.down * hit.distance);
            }
        }
    }

    void HandleMovement()
    {
        Vector3 moveInput = inputHandler.GetMoveInput();
        Vector3 moveDirection = transform.TransformDirection(moveInput);

        Vector3 targetVelocity = moveDirection * moveSpeed;
        m_moveVelocity = Vector3.SmoothDamp(m_moveVelocity, targetVelocity, ref m_smoothVelocity, smoothTime);

        Vector3 moveDelta = m_moveVelocity * Time.deltaTime;
        characterController.Move(moveDelta);
    }

    void HandleVerticalLook()
    {
        m_cameraVerticalAngle += inputHandler.GetLookInputVertical();
        m_cameraVerticalAngle = Mathf.Clamp(m_cameraVerticalAngle, -90f, 90f);
        camera.transform.localEulerAngles = new Vector3(m_cameraVerticalAngle, 0f, 0f);
    }

    void HandleHorizontalLook()
    {
        transform.Rotate(new Vector3(0f, inputHandler.GetLookInputHorizontal(), 0f), Space.Self);
    }

    void OnValidate()
    {
        SetCharacterHeight();
    }
}
