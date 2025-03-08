using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        // Получаем события столкновений
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        // Получаем Rigidbody объекта для применения силы
        var rb = other.GetComponent<Rigidbody2D>();

        // Переменная для накопления суммарной силы
        Vector2 totalForceDirection = Vector2.zero;
        int collisionCount = 0;

        // Проходим по всем событиям столкновений
        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Точка пересечения
            Vector3 pos = collisionEvents[i].intersection;

            // Направление силы: противоположное вектору скорости частицы
            Vector3 direction = -collisionEvents[i].velocity.normalized;

            // Суммируем направления столкновений
            totalForceDirection += (Vector2)direction;
            collisionCount++;
        }

        // Если столкновение было, применяем силу
        if (collisionCount > 0)
        {
            // Нормализуем направление, чтобы применить силу в одну сторону
            totalForceDirection.Normalize();

            // Сила пропорциональна количеству столкновений

            // Применяем силу в суммарном направлении
            rb.AddForce(-totalForceDirection * 1, ForceMode2D.Impulse);
            Debug.Log($"Applying force in direction: {totalForceDirection}, Force magnitude: {1}");
        }
    }
}