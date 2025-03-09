using UnityEngine;

namespace Assets.Scripts
{
    public class RandomMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f; // Скорость движения
        public float minTime = 1f;   // Минимальное время для смены направления
        public float maxTime = 3f;   // Максимальное время для смены направления

        private Rigidbody2D rb;      // Rigidbody2D для управления физикой
        private float changeDirectionTime; // Таймер для смены направления

        void Start()
        {
            rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
            SetRandomDirection(); // Устанавливаем случайное направление
            changeDirectionTime = Random.Range(minTime, maxTime); // Устанавливаем случайный таймер
        }

        void Update()
        {
            // Снижаем таймер
            changeDirectionTime -= Time.deltaTime;

            // Если таймер закончился, меняем направление
            if (changeDirectionTime <= 0)
            {
                SetRandomDirection(); // Обновляем направление
                changeDirectionTime = Random.Range(minTime, maxTime); // Устанавливаем новый случайный таймер
            }
        }

        // Метод для генерации случайного направления и применения силы
        private void SetRandomDirection()
        {
            float angle = Random.Range(0f, 2f * Mathf.PI); // Случайный угол
            Vector2 newDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // Преобразуем угол в вектор

            // Применяем силу для движения в случайном направлении
            rb.linearVelocity = newDirection * moveSpeed;
        }
    }
}