using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float _health = 0.0F;

    public float CurrentHealth { get => _health; }
    public Action OnHealthDepleted = null;

    public void ApplyDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0.0F)
        {
            OnHealthDepleted();
        }
    }
}
