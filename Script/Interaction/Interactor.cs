using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public bool CanInteract { get; private set; }
    public IInteractable Interactable { get; private set; }

    [SerializeField] private InputReaderSO _input;

    private List<IInteractable> _interactableList = new List<IInteractable>();

    private void Awake()
    {
        _input.OnInteractionEvent += HandleInteractionEvent;
    }

    private void OnDestroy()
    {
        _input.OnInteractionEvent -= HandleInteractionEvent;
    }

    private void HandleInteractionEvent()
    {
        Interactable?.Interaction();
    }

    private void TargetInteractableUpdate()
    {
        float distance = 10000;
        IInteractable minDisIInteractable = null;
        _interactableList.ForEach(interactable =>
        {
            float newDistence = (interactable.Transform.position - transform.position).magnitude;
            if (distance > newDistence)
            {
                distance = newDistence;
                minDisIInteractable = interactable;
            }
        });
        Interactable?.SetInteractable(false);
        Interactable = minDisIInteractable;
        Interactable?.SetInteractable(true);
    }

    private void Update()
    {
        TargetInteractableUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            _interactableList.Add(collision.GetComponent<IInteractable>());
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            _interactableList.Remove(collision.GetComponent<IInteractable>());
            CanInteract = false;
        }
    }
}
