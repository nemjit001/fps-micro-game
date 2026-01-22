using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyCharacter : MonoBehaviour
{
    Health _characterHealth = null;

    void Start()
    {
        _characterHealth = GetComponent<Health>();
        _characterHealth.OnHealthDepleted += OnHealthDepleted;
    }

    private void OnHealthDepleted()
    {
        Debug.Log("Owno I died :(");
        gameObject.SetActive(false);
        Destroy(gameObject, 1.0F);
    }
}
