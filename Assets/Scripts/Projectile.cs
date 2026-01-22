using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float _speed = 1.0F;
    [SerializeField]
    float _timeToLive = 1.0F;

    void Update()
    {
        _timeToLive -= Time.deltaTime;
        transform.position += transform.forward * _speed * Time.deltaTime;

        if (_timeToLive <= 0.0F)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 1.0F);
        }
    }
}
