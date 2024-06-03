using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Desert_Ghost_Town.Scripts
{
    public class HP : MonoBehaviour
    {
        public int maxHealth = 100;
        private int _currentHealth;

        public Slider healthSlider;
        public TMP_Text healthText;
        public GameOver gameOverManager;

        void Start()
        {
            _currentHealth = maxHealth;

            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = _currentHealth;
            }

            UpdateHealthText();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }

            if (healthSlider != null)
            {
                healthSlider.value = _currentHealth;
            }

            UpdateHealthText();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        void UpdateHealthText()
        {
            if (healthText != null)
            {
                healthText.text = _currentHealth + "/" + maxHealth;
            }
        }

        void Die()
        {
            if (healthText != null)
            {
                healthText.gameObject.SetActive(false);
            }

            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverScene();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                TakeDamage(10);
                Destroy(other.gameObject);
            }
        }
    }
}