using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "Scriptable Objects/PlayerInputSettings")]
public class PlayerInputSettings : ScriptableObject
{
    [SerializeField]
    public float lookSensitivity = 0.5F;
    [SerializeField]
    public bool invertYLook = false;
    [SerializeField]
    public bool toggleSprint = false;
}
