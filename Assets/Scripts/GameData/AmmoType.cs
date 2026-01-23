using UnityEngine;

[CreateAssetMenu(fileName = "AmmoType", menuName = "Scriptable Objects/AmmoType")]
public class AmmoType : ScriptableObject
{
    [SerializeField, Tooltip("Maximum ammo that may be held by the player in the inventory")]
    public int maxHeldAmmo = 0;
}
