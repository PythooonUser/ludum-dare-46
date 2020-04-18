using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactionRadius = 4f;
    [SerializeField] LayerMask interactionLayerMask = -1;
    [SerializeField] LayerMask groundLayerMask = -1;

    [Header("References")]
    [SerializeField] Text selectionNameText = default;
    [SerializeField] Text selectionActionText = default;
    [SerializeField] new Camera camera = default;
    [SerializeField] Transform handHeldAnchor = default;

    public bool isHoldingResource { get; private set; }

    IInteractable m_selection = default;

    void Start()
    {
        SetSelectionName("");
        SetSelectionAction("");
    }

    void Update()
    {
        HandleSelection();
        HandleInteraction();
    }

    void HandleSelection()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRadius, interactionLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != m_selection)
            {
                m_selection = interactable;

                SetSelectionName(interactable.GetName());

                string action = interactable.GetAction(this);
                SetSelectionAction(action != "" ? "Press 'E' " + action : "");
            }
        }
        else
        {
            m_selection = null;

            SetSelectionName("");
            SetSelectionAction("");
        }
    }

    void SetSelectionName(string name)
    {
        selectionNameText.text = name;
    }

    void SetSelectionAction(string action)
    {
        selectionActionText.text = action;
    }

    void HandleInteraction()
    {
        if (m_selection == null) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_selection.Interact(this);
        }
    }

    public void SetHandHeld(HandHeld handHeld)
    {
        if (handHeldAnchor.childCount > 0)
        {
            GameObject child = handHeldAnchor.GetChild(0).gameObject;
            Resource worldModel = child.GetComponent<HandHeld>().GetWorldModel();
            Destroy(child);

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayerMask))
            {
                Instantiate(worldModel, hit.point, Quaternion.identity);
            }
            else
            {
                Instantiate(worldModel, transform.position, Quaternion.identity);
            }

            isHoldingResource = false;
        }

        if (handHeld != null)
        {
            HandHeld handHeldInstance = Instantiate(handHeld);
            handHeldInstance.transform.SetParent(handHeldAnchor, false);

            isHoldingResource = true;
        }
    }
}
