using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, PlayerInput.INewactionmapActions
{
    private PlayerInput _playerInput;


    #region Actions

    public event Action OnInteractionEvent;
    public event Action OnInventoryEvent;
    public event Action OnFireEvent;
    public event Action OnESCEvent;
    public event Action<bool> OnFlashEvent;
    public event Action<bool> OnSprintEvent;
    public event Action<int> OnQuickSlotEvent;

    #endregion

    #region Values

    public Vector2 Movement {  get; private set; }

    #endregion


    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.Newactionmap.Enable();
        }

        _playerInput.Newactionmap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        _playerInput.Newactionmap.Disable();
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    public void OnInteraction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInteractionEvent?.Invoke();
    }

    public void OnInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInventoryEvent?.Invoke();
    }

    public void OnQuickSlot1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnQuickSlotEvent?.Invoke(0);
    }

    public void OnQuickSlot2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnQuickSlotEvent?.Invoke(1);
    }

    public void OnQuickSlot3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnQuickSlotEvent?.Invoke(2);
    }

    public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnFireEvent?.Invoke();
    }

    public void OnESC(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnESCEvent?.Invoke();
    }

    public void OnFlash(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnFlashEvent?.Invoke(true);
        else if (context.canceled)
            OnFlashEvent?.Invoke(false);
    }

    public void OnSprint(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSprintEvent?.Invoke(true);
        else if (context.canceled)
            OnSprintEvent?.Invoke(false);
    }
}
