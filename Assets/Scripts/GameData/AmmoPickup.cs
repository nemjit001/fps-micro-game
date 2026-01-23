using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPickup", menuName = "Scriptable Objects/AmmoPickup")]
public class AmmoPickup : ScriptableObject
{
    [SerializeField]
    public AmmoType ammoType = null;
    [SerializeField]
    public int ammoCount = 0;
}
