using UnityEngine;

public class SpawnPointVisuals : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5F);
    }
}
