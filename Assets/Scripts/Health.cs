using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float _health = 0.0F;

    public void ApplyDamage(float damage)
    {
        _health -= damage;
    }
}
