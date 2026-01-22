using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField, Tooltip("The player camera transform is used for aiming and spawning weapon visuals")]
    Transform _cameraTransform = null;
    [SerializeField]
    WeaponData _weapon = null;

    WeaponVisuals _activeWeaponVisuals = null;
    float _weaponCooldown = 0.0F;
    bool _weaponReady = true;

    void Start()
    {
        SpawnWeaponVisuals();
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
        _weaponCooldown = _weapon.WeaponFireCooldown;
        _weaponReady = false;

        // Automatic weapons are always ready to fire again
        if (_weapon.isAutomatic)
        {
            _weaponReady = true;
        }

        // Spawn projectile for weapon at projectile spawn point
        // TODO(nemjit001): Pool projectile objects to avoid cost of spawning new projectiles
        Projectile projectile = Instantiate(_weapon.projectile);
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
                health.ApplyDamage(_weapon.damage);
            }
        }
    }

    public void SetWeaponReady()
    {
        _weaponReady = true;
    }

    private void SpawnWeaponVisuals()
    {
        if (_weapon == null)
        {
            return;
        }

        Destroy(_activeWeaponVisuals);
        _activeWeaponVisuals = Instantiate(_weapon.visuals, _cameraTransform);
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
