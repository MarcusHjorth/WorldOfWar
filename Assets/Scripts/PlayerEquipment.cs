using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment instance;
    public float attackDamage = 10f;
    public float reach = 4f;

    public Transform weaponHolder; // Assign in the Inspector: Empty GameObject as a holder
    private GameObject currentWeapon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipWeapon(InventoryItemData weaponData)
    {
        if (weaponData == null)
        {
            Debug.LogError("Weapon data is null! Cannot equip weapon.");
            return;
        }

        if (weaponData.prefab == null)
        {
            Debug.LogError("Weapon prefab is null! Cannot equip weapon.");
            return;
        }

        // Destroy previous weapon if equipped
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instantiate new weapon
        currentWeapon = Instantiate(weaponData.prefab, weaponHolder);

        // Ensure it has a Weapon component
        Weapon weaponComponent = currentWeapon.GetComponent<Weapon>();
        if (weaponComponent == null)
        {
            Debug.LogError("Weapon component missing from prefab! Cannot equip weapon.");
            return;
        }

        weaponComponent.weaponData = weaponData;

        // Reset weapon position
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }



    public void AttackWithEquippedWeapon()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, reach))
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.Damage(attackDamage);
            }
        }
    }
}