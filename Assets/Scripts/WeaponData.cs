using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    public WeaponVisuals visuals = null;
    [SerializeField]
    public Projectile projectile = null;
}
