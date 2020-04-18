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
    [SerializeField] Transform handheldAnchor = default;

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
            Vector3 selectionPosition = m_selection.transform.position;
            Quaternion selectionRotation = m_selection.transform.rotation;

            GameObject handHeldPrefab = m_selection.PickUp();
            SetHandHeld(handHeldPrefab, selectionPosition, selectionRotation);
        }
    }

    void SetHandHeld(GameObject handHeld, Vector3 position, Quaternion rotation)
    {
        if (handheldAnchor.childCount > 0)
        {
            GameObject child = handheldAnchor.GetChild(0).gameObject;

            Resource worldModel = child.GetComponent<HandHeld>().GetWorldModel();
            Instantiate(worldModel, position, rotation);

            Destroy(child);
        }

        if (handHeld != null)
        {
            GameObject handHeldInstance = Instantiate(handHeld);
            handHeldInstance.transform.SetParent(handheldAnchor, false);
        }
    }
}
