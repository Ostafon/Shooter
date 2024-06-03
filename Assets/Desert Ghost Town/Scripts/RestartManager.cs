using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Restart.performed += OnRestartPerformed;
    }

    private void OnDisable()
    {
        _inputActions.Player.Restart.performed -= OnRestartPerformed;
        _inputActions.Disable();
    }

    private void OnRestartPerformed(InputAction.CallbackContext context)
    {
       
        SceneManager.LoadScene("MainScene");
    }
}