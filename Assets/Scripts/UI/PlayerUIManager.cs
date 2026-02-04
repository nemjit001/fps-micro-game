using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    Health _characterHealth = null;
    [SerializeField]
    WeaponManager _weaponManager = null;
    [SerializeField]
    InventoryManager _inventoryManager = null;

    private string WeaponAmmoLabelText { get => $"{_weaponManager.ActiveWeapon.ammoCount}/{_inventoryManager.GetHeldAmmoCount(_weaponManager.ActiveWeapon.ammoType)}"; }

    ProgressBar _playerHealthBar = null;
    Label _weaponAmmoLabel = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _playerHealthBar = document.rootVisualElement.Query<ProgressBar>("player-health-bar");
        _weaponAmmoLabel = document.rootVisualElement.Query<Label>("weapon-ammo-label");
    }

    void OnDisable()
    {
        //
    }

    void Update()
    {
        // TODO(nemjit001): Show percentage based on current and max health
        _playerHealthBar.value = _characterHealth.CurrentHealth;
        _weaponAmmoLabel.text = WeaponAmmoLabelText;
    }
}
