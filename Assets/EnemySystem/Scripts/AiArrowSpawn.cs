using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Скорость стрелы
    [SerializeField] private GameObject hitEffectPrefab; // Эффект попадания
    private Transform player; // Цель (игрок)
    private Vector2 direction; // Направление полёта

    void Start()
    {
        // Находим игрока по тегу (убедитесь, что у него есть тег "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure it has the 'Player' tag.");
            Destroy(gameObject);
            return;
        }

        // Вычисляем направление к игроку
        direction = (player.position - transform.position).normalized;
        
        // Поворачиваем стрелу в сторону игрока
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hitEffectPrefab != null)
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject); // Уничтожаем стрелу
        }
    }
}