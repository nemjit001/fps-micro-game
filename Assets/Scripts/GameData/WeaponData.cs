using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Visuals")]
    [SerializeField]
    public WeaponVisuals visuals = null;
    [SerializeField]
    public Projectile projectile = null;
    [Header("Weapon Stats")]
    [SerializeField]
    public float damage = 1.0F;
    [SerializeField, Tooltip("Weapon fire rate in projectiles per second")]
    public float fireRate = 1.0F;
    [SerializeField, Tooltip("Automatic weapons do not require the player to release the fire button before shooting again")]
    public bool isAutomatic = false;

    public float WeaponFireCooldown { get => 1.0F / fireRate; }
}
