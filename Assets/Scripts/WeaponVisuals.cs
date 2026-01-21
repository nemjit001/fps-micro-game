using UnityEngine;

public class WeaponVisuals : MonoBehaviour
{
    [SerializeField]
    Transform _projectileSpawnPoint = null;

    public Transform ProjectileSpawnPoint { get => _projectileSpawnPoint; }
}
