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
        // Do a simple raycast for hit object detection
        if (Physics.Raycast(aimTransform.position, aimTransform.forward, out RaycastHit hit, Mathf.Infinity, ~0))
        {
            Health health = hit.transform.root.GetComponent<Health>(); // We use the root object since colliders may be children of the damagable entity
            if (health != null)
            {
                Debug.Log($"hit something with health! ({hit.transform.root.name})");
                // TODO(nemjit001): Apply weapon damage to hit health component
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
