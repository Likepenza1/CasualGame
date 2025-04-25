using UnityEngine;
using System.Collections;

public class AutoDestroyTimer : MonoBehaviour
{
    [Tooltip("Время в секундах до автоматического уничтожения объекта")]
    public float destroyDelay = 1f;

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}