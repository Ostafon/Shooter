using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Desert_Ghost_Town.Scripts
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform player; 
        [SerializeField] private GameObject bulletPrefab; 
        [SerializeField] private Transform firePoint; 
        [SerializeField] private float bulletSpeed = 50f; 
        [SerializeField] private float shootingInterval = 2f; 
        [SerializeField] private float moveSpeed = 3f; 
        [SerializeField] private float detectionRange = 10f; 
        [SerializeField] private float shootingRange = 5f; 
        [SerializeField] private int maxHealth = 100; 

        [Header("Audio Settings")]
        [SerializeField] private AudioClip shootSound; 
        [SerializeField] private AudioClip hitSound; 
        [SerializeField] private AudioClip deathSound; 

        private AudioSource _audioSource;
        private float _nextShootTime;
        private int _currentHealth;
        private Animator _animator;

        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsAiming = Animator.StringToHash("isAiming");
        private static readonly int IsDead = Animator.StringToHash("isDead");

        void Start()
        {
            _currentHealth = maxHealth;
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                if (distanceToPlayer <= shootingRange)
                {
                    transform.LookAt(player);
                    if (Time.time >= _nextShootTime)
                    {
                        Shoot();
                        _nextShootTime = Time.time + shootingInterval;
                    }
                    SetAnimatorState(IsAiming);
                }
                else
                {
                    transform.LookAt(player);
                    transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
                    SetAnimatorState(IsWalking);
                }
            }
            else
            {
                SetAnimatorState(IsIdle);
            }
        }

        void Shoot()
        {
            if (bulletPrefab != null && firePoint != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if(player != null)
                {
                    Vector3 targetPosition = player.transform.position;

                    Vector3 direction = (targetPosition - firePoint.position);
                    direction.y = 0;

                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                    if (bulletRb != null)
                    {
                        bulletRb.velocity = direction.normalized * bulletSpeed;
                    }

                    RaycastHit hit;
                    if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, shootingRange))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            HP playerHP = hit.transform.GetComponent<HP>();
                            if (playerHP != null)
                            {
                                playerHP.TakeDamage(5);
                            }
                        }
                    }

                    if (_audioSource != null && shootSound != null)
                    {
                        _audioSource.PlayOneShot(shootSound);
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                TakeDamage(100);
                Destroy(other.gameObject);
            }
        }

        void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }

            if (_currentHealth <= 0)
            {
                SetAnimatorState(IsDead);
                if (_audioSource != null && deathSound != null)
                {
                    _audioSource.PlayOneShot(deathSound);
                }
                Destroy(gameObject, 3f);
            }
            else
            {
                if (_audioSource != null && hitSound != null)
                {
                    _audioSource.PlayOneShot(hitSound);
                }
            }
        }

        void SetAnimatorState(int stateHash)
        {
            _animator.SetBool(IsIdle, stateHash == IsIdle);
            _animator.SetBool(IsWalking, stateHash == IsWalking);
            _animator.SetBool(IsAiming, stateHash == IsAiming);
            _animator.SetBool(IsDead, stateHash == IsDead);
        }
    }
}
