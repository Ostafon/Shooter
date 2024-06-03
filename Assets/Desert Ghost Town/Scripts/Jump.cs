using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private Rigidbody _rigidbody;
    public float jumpForce = 5.0f;
    public float fallMultiplier = 2.5f; // Управление скоростью падения
    private bool _isGrounded = true; // Переменная для отслеживания нахождения персонажа на земле

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Jump.performed += ctx => Jump();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Jump()
    {
        
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false; 
        }
    }

    private void Update()
    {
        
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}