using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryManager))]
public class WeaponManager : MonoBehaviour
{
    [SerializeField, Tooltip("The player camera transform is used for aiming and spawning weapon visuals")]
    Transform _cameraTransform = null;
    [SerializeField]
    List<Weapon> _weapons = new List<Weapon>(); // FIXME(nemjit001): Player weapon inventory should be runtime data

    InventoryManager _inventoryManager = null;
    int _activeWeaponidx = 0;
    WeaponVisuals _activeWeaponVisuals = null;
    float _weaponCooldown = 0.0F;
    bool _weaponReady = true;

    public Weapon ActiveWeapon { get => _weapons[_activeWeaponidx]; }

    void Start()
    {
        // Set initial state for each weapon
        foreach (var weapon in _weapons)
        {
            weapon.Initialize();
        }

        _inventoryManager = GetComponent<InventoryManager>();
        SwitchToWeapon(0);
    }

    void Update()
    {
        TickCooldownTimer();
    }

    /// <summary>
    /// Shoot the currently active weapon.
    /// </summary>
    public void Shoot()
    {
        // Only shoot if weapon cooldown and ready flag allows it :)
        if (!IsCooldownFinished() || !_weaponReady)
        {
            return;
        }
        _weaponCooldown = ActiveWeapon.WeaponFireCooldown;
        _weaponReady = false;

        // Check if weapon is empty and reduce ammo count
        if (ActiveWeapon.ammoCount == 0)
        {
            // TODO(nemjit001): Play empty weapon animation and audio clip
            return;
        }
        ActiveWeapon.ammoCount -= 1;

        // Automatic weapons are always ready to fire again
        if (ActiveWeapon.isAutomatic)
        {
            _weaponReady = true;
        }

        // TODO(nemjit001): Play (spawn?) shooting audio clip
        // TODO(nemjit001): Play shooting animation

        // NOTE(nemjit001): Weapon hit detection & projectile spawning can be different for weapons (e.g. rifles, shotguns, projectile based weapons)
        // Spawn projectile for weapon at projectile spawn point
        // TODO(nemjit001): Pool projectile objects to avoid cost of spawning new projectiles
        Projectile projectile = Instantiate(ActiveWeapon.projectile);
        projectile.transform.position = _activeWeaponVisuals.ProjectileSpawnPoint.position;
        projectile.transform.rotation = _activeWeaponVisuals.ProjectileSpawnPoint.rotation;

        // Do a simple raycast for hit object detection
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, Mathf.Infinity, ~0))
        {
            // Apply damage from weapon if the hit object has a health component
            // NOTE(nemjit001): We use the root object since colliders may be children of the damagable entity
            Health health = hit.transform.root.GetComponent<Health>();
            if (health != null)
            {
                health.ApplyDamage(ActiveWeapon.damage);
            }
        }
    }

    /// <summary>
    /// Reload the currently active weapon.
    /// </summary>
    public void Reload()
    {
        AmmoType ammoType = ActiveWeapon.ammoType;
        if (_inventoryManager.GetHeldAmmoCount(ammoType) == 0) // Empty ammo inventory :(
        {
            // TODO(nemjit001): Play no ammo animation
            return;
        }

        // TODO(nemjit001): Play reload animation
        int wanted = ActiveWeapon.ammoCapacity - ActiveWeapon.ammoCount;
        ActiveWeapon.ammoCount += _inventoryManager.ReduceHeldAmmoCount(ammoType, wanted);
        Debug.Assert(ActiveWeapon.ammoCount <= ActiveWeapon.ammoCapacity, "Ammo count for weapon is larger than capacity!");
    }

    /// <summary>
    /// Set the currently active weapon ready state.
    /// </summary>
    public void SetWeaponReady()
    {
        _weaponReady = true;
    }

    /// <summary>
    /// Select the next weapon in the weapon inventory.
    /// </summary>
    public void SelectNextWeapon()
    {
        int weaponIdx = (_activeWeaponidx + 1) % _weapons.Count;
        SwitchToWeapon(weaponIdx);
    }

    /// <summary>
    /// Select the previous weapon in the weapon inventory.
    /// </summary>
    public void SelectPreviousWeapon()
    {
        int weaponIdx = _activeWeaponidx - 1;
        if (weaponIdx < 0)
        {
            weaponIdx = _weapons.Count - 1;
        }

        SwitchToWeapon(weaponIdx);
    }

    /// <summary>
    /// Switch to a given weapon index in the inventory.
    /// </summary>
    /// <param name="weaponIdx"></param>
    private void SwitchToWeapon(int weaponIdx)
    {
        _weaponCooldown = 0.0F;
        _weaponReady = true;
        if (weaponIdx >= 0 && weaponIdx < _weapons.Count)
        {
            _activeWeaponidx = weaponIdx;
        }

        SpawnWeaponVisuals();
        Debug.Log($"Selected weapon {ActiveWeapon.name}");
    }

    /// <summary>
    /// Spawn the active weapon visuals.
    /// </summary>
    private void SpawnWeaponVisuals()
    {
        Destroy(_activeWeaponVisuals);
        if (ActiveWeapon == null)
        {
            return;
        }

        _activeWeaponVisuals = Instantiate(ActiveWeapon.visuals, _cameraTransform);
    }

    /// <summary>
    /// Tick the weapon cooldown timer.
    /// </summary>
    private void TickCooldownTimer()
    {
        if (!IsCooldownFinished())
        {
            _weaponCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Check if the weapon cooldown timer is finished.
    /// </summary>
    /// <returns></returns>
    private bool IsCooldownFinished()
    {
        return _weaponCooldown <= 0.0F;
    }
}
