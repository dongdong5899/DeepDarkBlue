using UnityEngine;

public interface IInteractable
{
    public Transform Transform { get; set; }
    public void Interaction();
    public void SetInteractable(bool isInteractable);
}
