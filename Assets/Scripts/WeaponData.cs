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
}
