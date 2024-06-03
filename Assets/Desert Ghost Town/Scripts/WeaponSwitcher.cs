using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;
    private PlayerInputActions playerInputActions;
    private int currentWeaponIndex = 0; 

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.SelectWeapon1.performed += ctx => SelectWeapon(0);
        playerInputActions.Player.SelectWeapon2.performed += ctx => SelectWeapon(1);
        playerInputActions.Player.SelectWeapon3.performed += ctx => SelectWeapon(2);
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    void Start()
    {
        
        SelectWeapon(currentWeaponIndex);
    }

    void SelectWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Length)
        {
            return;
        }

       
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        
        weapons[weaponIndex].SetActive(true);

       
        currentWeaponIndex = weaponIndex;
    }
}