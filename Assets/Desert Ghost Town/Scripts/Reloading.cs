using System.Collections;
using UnityEngine;
using TMPro; 

namespace Desert_Ghost_Town.Scripts
{
    public class Reloading : MonoBehaviour
    {
        [SerializeField] private int maxAmmo = 30; 
        [SerializeField] private int reserveAmmo = 90;
        [SerializeField] private float reloadTime = 2f;
        [SerializeField] private TMP_Text ammoText; 

        private int _currentAmmo;
        private bool _isReloading ;

        void Start()
        {
            _currentAmmo = maxAmmo;
            UpdateAmmoText();
        }

        public void StartReload()
        {
            if (!_isReloading && _currentAmmo < maxAmmo && reserveAmmo > 0)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            _isReloading = true;
            

            int ammoToLoad = maxAmmo - _currentAmmo;
            if (reserveAmmo < ammoToLoad)
            {
                ammoToLoad = reserveAmmo;
            }

            _currentAmmo += ammoToLoad;
            reserveAmmo -= ammoToLoad;

            yield return new WaitForSeconds(reloadTime); 

            _isReloading = false;
            UpdateAmmoText();
        }

        public void UseAmmo()
        {
            if (_currentAmmo > 0)
            {
                _currentAmmo--;
                UpdateAmmoText();
            }
        }

        public bool CanShoot()
        {
            return _currentAmmo > 0 && !_isReloading;
        }

        private void UpdateAmmoText()
        {
            if (ammoText != null)
            {
                ammoText.text = $"{_currentAmmo}/{reserveAmmo}";
            }
        }
    }
}