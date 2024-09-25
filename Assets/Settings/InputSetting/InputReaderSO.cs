using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions
{
    public event Action OnInteractEvt;
    public event Action OnOpenInventoryEvt;

    public event Action OnAttackEvt;
    public event Action OnSwapWeaponEvt;
    public event Action OnMainSkillEvt;
    public event Action OnSubSkillEvt;
    public event Action<Vector2> OnMovementEvt;

    private Controls control;

    private void OnEnable()
    {
        if (control == null)
        {
            control = new Controls();
            control.Player.SetCallbacks(this);

            control.Enable();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackEvt?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnInteractEvt?.Invoke();
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnOpenInventoryEvt?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        OnMovementEvt?.Invoke(movement);
    }

    public void OnMainSkill(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnMainSkillEvt?.Invoke();
    }

    public void OnSubSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSubSkillEvt?.Invoke();
    }

    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSwapWeaponEvt?.Invoke();
    }
}
