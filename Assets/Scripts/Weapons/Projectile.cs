using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float _timeToLive = 1.0F;

    void Update()
    {
        _timeToLive -= Time.deltaTime;
        if (_timeToLive <= 0.0F)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 1.0F);
        }
    }
}
