using UnityEngine;

namespace Assets.Scripts
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        public GameObject prefab; // Префаб для спавна
        public int spawnCount = 5; // Количество объектов для спавна
        public float spawnInterval = 2f; // Время между спавнами
        public bool spawnInCircle = true; // Спавн внутри круга или квадрата
        public float spawnRadius = 5f; // Радиус круга или половина длины стороны квадрата (если квадрат)

        [Header("Spawn Zone")]
        public bool spawnInSquare = false; // Если true, будет использовать квадрат вместо круга

        private void Start()
        {
            // Запускаем спавн в корутине
            StartCoroutine(SpawnObjects());
        }

        // Корутин для спавна объектов
        private System.Collections.IEnumerator SpawnObjects()
        {
            while (true)
            {
                // Генерация случайной позиции в зависимости от зоны
                Vector3 spawnPosition = GetRandomPosition();
                Instantiate(prefab, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnInterval);
            }

        }

        // Получение случайной позиции в зоне
        private Vector3 GetRandomPosition()
        {
            Vector3 position = Vector3.zero;

            if (spawnInCircle)
            {
                // Генерация позиции в круге
                float angle = Random.Range(0f, 2f * Mathf.PI);
                float radius = Random.Range(0f, spawnRadius);
                position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            }
            else if (spawnInSquare)
            {
                // Генерация позиции внутри квадрата
                position = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0);
            }

            return transform.position + position; // Смещаем позицию относительно родительского объекта
        }

        // Отображение зоны спавна в редакторе
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (spawnInCircle)
            {
                Gizmos.DrawWireSphere(transform.position, spawnRadius); // Круглая зона
            }
            else if (spawnInSquare)
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadius * 2, spawnRadius * 2, 0)); // Квадратная зона
            }
        }
    }
}