using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float _health = 0.0F;

    public float CurrentHealth { get => _health; }
    public Action OnHealthDepleted = null;

    /// <summary>
    /// Apply damage to the stored health value. When the health is depleted the `OnHealthDepleted` callback is invoked.
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0.0F)
        {
            OnHealthDepleted();
        }
    }
}
