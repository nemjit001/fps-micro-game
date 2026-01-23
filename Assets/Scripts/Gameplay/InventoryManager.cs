using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    Dictionary<AmmoType, int> _inventory = new Dictionary<AmmoType, int>(); // Maps ammo type to held number of bullets

    /// <summary>
    /// Apply an ammunition pickup in the inventory.
    /// </summary>
    /// <param name="pickup">Ammunition pickup to apply.</param>
    /// <returns>A boolean indicating if the ammunition pickup could be applied (i.e. the held ammo count is not yet at max value)</returns>
    public bool ApplyAmmoPickup(AmmoPickup pickup)
    {
        if (_inventory.ContainsKey(pickup.ammoType))
        {
            // If ammo inventory is full pickup is not applied
            if (_inventory[pickup.ammoType] >= pickup.ammoType.maxHeldAmmo)
            {
                _inventory[pickup.ammoType] = pickup.ammoType.maxHeldAmmo;
                return false;
            }

            // Apply clamped ammo count
            _inventory[pickup.ammoType] = Mathf.Clamp(_inventory[pickup.ammoType] + pickup.ammoCount, 0, pickup.ammoType.maxHeldAmmo);
        }
        else
        {
            // Just add initial ammo count
            _inventory.Add(pickup.ammoType, pickup.ammoCount);
        }

        return true;
    }

    /// <summary>
    /// Get the held count of ammunition for an ammunition type.
    /// </summary>
    /// <param name="ammoType"></param>
    /// <returns></returns>
    public int GetHeldAmmoCount(AmmoType ammoType)
    {
        return _inventory[ammoType];
    }

    /// <summary>
    /// Reduce the count of held ammunition for an ammunition type by a wanted value.
    /// </summary>
    /// <param name="ammoType">Type of ammunition to reduce.</param>
    /// <param name="wanted">Wanted ammunition count.</param>
    /// <returns>Actual available ammunition count.</returns>
    public int ReduceHeldAmmoCount(AmmoType ammoType, int wanted)
    {
        if (_inventory.ContainsKey(ammoType))
        {
            if (_inventory[ammoType] <= wanted)
            {
                int count = _inventory[ammoType];
                _inventory[ammoType] = 0;
                return count;
            }
            else
            {
                _inventory[ammoType] -= wanted;
                return wanted;
            }
        }

        return 0;
    }
}
