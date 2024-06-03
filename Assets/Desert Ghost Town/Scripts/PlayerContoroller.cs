using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Desert_Ghost_Town.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _sprintInput;
        private bool _fireInput;
        private bool _reloadInput;
        public AudioClip[] footstepSounds;
        public AudioClip reloadSound;
        private AudioSource audioSource;

        [Header("Movement Settings")]
        [SerializeField] private float mainSpeed = 3.0f;
        [SerializeField] private float shiftAdd = 2.0f;
        [SerializeField] private float maxShift = 5.0f;

        [Header("Mouse Control")]
        [SerializeField] private float sensitivity = 1f;
        [SerializeField] private float minVerticalAngle = -45f;
        [SerializeField] private float maxVerticalAngle = 45f;
        [SerializeField] private float smoothTime = 0.1f;

        [Header("Shooting Settings")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Image crosshair;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private AudioClip shootSound;

        private Animator _animator;
        private Rigidbody _rigidbody;
        private bool _isJumping;
        private bool _isGrounded;
        private float _cameraVerticalAngle;
        private float _currentVerticalVelocity;
        private float _currentHorizontalVelocity;

        private Reloading _reloading;
        private float _nextFireTime;
        private bool _isReloading;

        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        private static readonly int IsWalkBack = Animator.StringToHash("isWalkBack");
        private static readonly int IsSprinting = Animator.StringToHash("isSprinting");

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            _rigidbody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();

            _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
            _inputActions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Look.canceled += ctx => _lookInput = Vector2.zero;
            _inputActions.Player.Sprint.performed += ctx => _sprintInput = ctx.ReadValueAsButton();
            _inputActions.Player.Shoot.performed += ctx => _fireInput = ctx.ReadValueAsButton();
            _inputActions.Player.Shoot.canceled += ctx => _fireInput = false;
            _inputActions.Player.Reload.performed += ctx => _reloadInput = ctx.ReadValueAsButton();
            _inputActions.Player.Reload.canceled += ctx => _reloadInput = false;
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
            _animator = GetComponent<Animator>();
            _reloading = GetComponent<Reloading>();
            Cursor.lockState = CursorLockMode.Locked;
            _nextFireTime = Time.time;
        }

        private void Update()
        {
            HandleLook();
            if (_fireInput)
            {
                HandleShooting();
            }

            if (_reloadInput && !_isReloading)
            {
                HandleReloading();
            }
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleLook()
        {
            if (_lookInput == Vector2.zero)
                return;

            float targetHorizontalAngle = transform.eulerAngles.y + _lookInput.x * sensitivity;
            float targetVerticalAngle = _cameraVerticalAngle - _lookInput.y * sensitivity;
            targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);

            float smoothHorizontalAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetHorizontalAngle, ref _currentHorizontalVelocity, smoothTime);
            float smoothVerticalAngle = Mathf.SmoothDampAngle(_cameraVerticalAngle, targetVerticalAngle, ref _currentVerticalVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(0f, smoothHorizontalAngle, 0f);
            _cameraVerticalAngle = smoothVerticalAngle;
            Camera.main.transform.localRotation = Quaternion.Euler(_cameraVerticalAngle, 0f, 0f);
        }

        private void HandleMovement()
        {
            Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

            if (_sprintInput)
            {
                direction *= shiftAdd;
                direction.x = Mathf.Clamp(direction.x, -maxShift, maxShift);
                direction.z = Mathf.Clamp(direction.z, -maxShift, maxShift);
            }
            else
            {
                direction *= mainSpeed;
            }

            Vector3 movement = transform.TransformDirection(direction) * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + movement);

            UpdateAnimation(direction);

            
            if (audioSource != null && footstepSounds.Length > 0 && movement.magnitude > 0)
            {
                PlayFootstepSound();
            }
        }

        private void PlayFootstepSound()
        {
            if (audioSource.isPlaying) return;
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[index]);
        }

        private void UpdateAnimation(Vector3 direction)
        {
            bool isWalking = direction.z > 0;
            bool isWalkingBack = direction.z < 0;

            if (_animator.HasParameterOfType(IsWalk, AnimatorControllerParameterType.Bool))
                _animator.SetBool(IsWalk, isWalking);
            if (_animator.HasParameterOfType(IsWalkBack, AnimatorControllerParameterType.Bool))
                _animator.SetBool(IsWalkBack, isWalkingBack);
            if (_animator.HasParameterOfType(IsSprinting, AnimatorControllerParameterType.Bool))
                _animator.SetBool(IsSprinting, _sprintInput);
        }

        private void HandleShooting()
        {
            if (bulletPrefab != null && firePoint != null && crosshair != null && Camera.main != null)
            {
                if (_reloading.CanShoot() && Time.time >= _nextFireTime)
                {
                    
                    if (audioSource != null && shootSound != null)
                    {
                        audioSource.PlayOneShot(shootSound);
                    }

                    Vector3 screenPoint = crosshair.transform.position;
                    Ray ray = Camera.main.ScreenPointToRay(screenPoint);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Vector3 targetPoint = hit.point;
                        Vector3 direction = (targetPoint - firePoint.position).normalized;
                        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                        if (bulletRb != null)
                        {
                            bulletRb.velocity = direction * bulletSpeed;
                        }
                    }
                    else
                    {
                        Vector3 direction = ray.direction;
                        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                        if (bulletRb != null)
                        {
                            bulletRb.velocity = direction * bulletSpeed;
                        }
                    }

                    _reloading.UseAmmo();
                    _nextFireTime = Time.time + fireRate;
                }
            }
        }

        private void HandleReloading()
        {
            _isReloading = true;
            if (audioSource != null && reloadSound != null)
            {
                audioSource.PlayOneShot(reloadSound);
            }
            _reloading.StartReload();
            Invoke(nameof(ResetReloading), reloadSound.length);
        }

        private void ResetReloading()
        {
            _isReloading = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                HP playerHp = GetComponent<HP>();
                if (playerHp != null)
                {
                    playerHp.TakeDamage(10);
                }
                Destroy(other.gameObject);
            }
        }
    }

    public static class AnimatorExtensions
    {
        public static bool HasParameterOfType(this Animator animator, int hash, AnimatorControllerParameterType type)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.nameHash == hash && param.type == type) return true;
            }
            return false;
        }
    }
}
