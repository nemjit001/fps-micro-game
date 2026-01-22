using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField, Tooltip("The player camera transform is used for aiming and spawning weapon visuals")]
    Transform _cameraTransform = null;
    [SerializeField]
    List<Weapon> _weapons = new List<Weapon>(); // FIXME(nemjit001): Player weapon inventory should be runtime data

    int _activeWeaponidx = 0;
    WeaponVisuals _activeWeaponVisuals = null;
    float _weaponCooldown = 0.0F;
    bool _weaponReady = true;

    private Weapon ActiveWeapon { get => _weapons[_activeWeaponidx]; }

    void Start()
    {
        SwitchToWeapon(0);
    }

    void Update()
    {
        TickCooldownTimer();
    }

    public void Shoot()
    {
        // Only shoot if weapon cooldown and ready flag allows it :)
        if (!IsCooldownFinished() || !_weaponReady)
        {
            return;
        }
        _weaponCooldown = ActiveWeapon.WeaponFireCooldown;
        _weaponReady = false;

        // Automatic weapons are always ready to fire again
        if (ActiveWeapon.isAutomatic)
        {
            _weaponReady = true;
        }

        // Spawn projectile for weapon at projectile spawn point
        // TODO(nemjit001): Pool projectile objects to avoid cost of spawning new projectiles
        Projectile projectile = Instantiate(ActiveWeapon.projectile);
        projectile.transform.position = _activeWeaponVisuals.ProjectileSpawnPoint.position;
        projectile.transform.rotation = _activeWeaponVisuals.ProjectileSpawnPoint.rotation;

        // TODO(nemjit001): Play (spawn?) shooting audio clip
        // TODO(nemjit001): Play shooting animation

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

    public void Reload()
    {
        // TODO(nemjit001): Reload weapon with correct ammo from player inventory
        // 1) Check weapon ammo type (scriptable object?)
        // 2) Check if player inventory contains ammo type (how much does it contain? map AmmoType:int)
        // 3) Reduce amount in player inventory and set weapon clip contents (+ play reload anim)
    }

    public void SetWeaponReady()
    {
        _weaponReady = true;
    }

    public void SelectNextWeapon()
    {
        int weaponIdx = (_activeWeaponidx + 1) % _weapons.Count;
        SwitchToWeapon(weaponIdx);
    }

    public void SelectPreviousWeapon()
    {
        int weaponIdx = _activeWeaponidx - 1;
        if (weaponIdx < 0)
        {
            weaponIdx = _weapons.Count - 1;
        }

        SwitchToWeapon(weaponIdx);
    }

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

    private void SpawnWeaponVisuals()
    {
        Destroy(_activeWeaponVisuals);
        if (ActiveWeapon == null)
        {
            return;
        }

        _activeWeaponVisuals = Instantiate(ActiveWeapon.visuals, _cameraTransform);
    }

    private void TickCooldownTimer()
    {
        if (!IsCooldownFinished())
        {
            _weaponCooldown -= Time.deltaTime;
        }
    }

    private bool IsCooldownFinished()
    {
        return _weaponCooldown <= 0.0F;
    }
}
