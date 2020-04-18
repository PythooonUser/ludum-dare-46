using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactionRadius = 4f;
    [SerializeField] LayerMask interactionLayerMask = -1;

    [Header("References")]
    [SerializeField] Text selectionText = default;
    [SerializeField] Text actionText = default;
    [SerializeField] new Camera camera = default;

    Resource m_selection = default;

    void Start()
    {
        SetText("");
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
            Resource resource = hit.collider.GetComponentInParent<Resource>();

            if (resource != m_selection)
            {
                m_selection = resource;
                SetText(resource.GetName());
            }
        }
        else
        {
            m_selection = null;
            SetText("");
        }
    }

    void SetText(string text)
    {
        selectionText.text = text;

        actionText.gameObject.SetActive(text != "");
    }

    void HandleInteraction()
    {
        if (!m_selection) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_selection.PickUp();
        }
    }
}
