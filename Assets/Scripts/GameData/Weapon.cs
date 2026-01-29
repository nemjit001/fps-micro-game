using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
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
    [SerializeField, Tooltip("Weapon capacity on reload")]
    public int ammoCapacity = 0;
    [SerializeField, Tooltip("Initial weapon ammo count")]
    public int initialAmmoCount = 0;
    [SerializeField, Tooltip("Automatic weapons do not require the player to release the fire button before shooting again")]
    public bool isAutomatic = false;
    [SerializeField, Tooltip("The Ammo Type for a weapon determines which shared ammo inventory it will deplete ammo from")]
    public AmmoType ammoType = null;

    public float WeaponFireCooldown { get => 1.0F / fireRate; }
    public int ammoCount = 0;

    void OnValidate()
    {
        initialAmmoCount = Mathf.Clamp(initialAmmoCount, 0, ammoCapacity);
    }

    /// <summary>
    /// Initialize the weapon state.
    /// </summary>
    public void Initialize()
    {
        ammoCount = initialAmmoCount;
    }
}
