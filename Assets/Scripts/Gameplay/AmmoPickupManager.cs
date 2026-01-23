using UnityEngine;

public class AmmoPickupManager : MonoBehaviour
{
    [SerializeField]
    AmmoPickup ammoPickup = null;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} entered pickup region");
        InventoryManager inventoryManager = other.GetComponent<InventoryManager>();
        if (inventoryManager != null && inventoryManager.ApplyAmmoPickup(ammoPickup))
        {
            // TODO(nemjit001): Should pickups respawn (i.e. inactive -> active based on timer)?
            Destroy(gameObject);
        }
    }
}
