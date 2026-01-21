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
