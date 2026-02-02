using UnityEngine;

public class WeaponVFX : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _particleSystem = null;

    void Update()
    {
        if (_particleSystem.isStopped)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 1.0F);
        }
    }
}
