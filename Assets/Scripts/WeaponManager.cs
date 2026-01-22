using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    Transform _cameraTransform = null;
    [SerializeField]
    WeaponData _weapon = null;

    WeaponVisuals _activeWeaponVisuals = null;

    void Start()
    {
        SpawnWeaponVisuals();
    }

    public void Shoot(Transform aimTransform)
    {
        // Spawn projectile for weapon at projectile spawn point
        Projectile projectile = Instantiate(_weapon.projectile);
        projectile.transform.position = _activeWeaponVisuals.ProjectileSpawnPoint.position;
        projectile.transform.rotation = _activeWeaponVisuals.ProjectileSpawnPoint.rotation;

        // Do a simple raycast for hit object detection
        if (Physics.Raycast(aimTransform.position, aimTransform.forward, out RaycastHit hit, Mathf.Infinity, ~0))
        {
            Health health = hit.transform.root.GetComponent<Health>(); // We use the root object since colliders may be children of the damagable entity
            if (health != null)
            {
                Debug.Log($"hit something with health! ({hit.transform.root.name})");
                health.ApplyDamage(_weapon.damage);
            }
        }
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
}
