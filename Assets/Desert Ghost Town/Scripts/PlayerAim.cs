using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private Animator _animator;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Aim.performed += OnAimPerformed;
        _inputActions.Player.Aim.canceled += OnAimCanceled;
    }

    private void OnDisable()
    {
        _inputActions.Player.Aim.performed -= OnAimPerformed;
        _inputActions.Player.Aim.canceled -= OnAimCanceled;
        _inputActions.Disable();
    }

    private void OnAimPerformed(InputAction.CallbackContext context)
    {
        _animator.SetBool("isAiming", true);
    }

    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        _animator.SetBool("isAiming", false);
    }
}