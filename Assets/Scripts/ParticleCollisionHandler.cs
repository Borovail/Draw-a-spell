using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public float WeaknessFactor = 1;
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
            // Направление силы: противоположное вектору скорости частицы
            if (other.gameObject.TryGetComponent<RandomMover>(out var mover))
                mover.enabled = false;

            Vector3 direction = collisionEvents[i].velocity.normalized;
            Destroy(collisionEvents[i].colliderComponent.gameObject, 2f);

            // Суммируем направления столкновений
            totalForceDirection += (Vector2)direction;
            collisionCount++;

            // Дополнительно учитываем скорость (модуль) частицы
            float particleSpeed = collisionEvents[i].velocity.magnitude;

            // Масштабируем силу в зависимости от скорости частицы
            float forceMagnitude = particleSpeed / WeaknessFactor / 5; // Масштабируем с коэффициентом 10 для заметного эффекта

            // Применяем силу в зависимости от скорости частицы
            rb.AddForce(totalForceDirection.normalized * forceMagnitude, ForceMode2D.Impulse);
        }


        //// Если столкновение было, применяем силу
        //if (collisionCount > 0)
        //{
        //    // Нормализуем направление, чтобы применить силу в одну сторону
        //    totalForceDirection.Normalize();

        //    // Применяем общую силу (можно оставить, если хочешь обобщить)
        //    rb.AddForce(totalForceDirection * 1, ForceMode2D.Impulse);
        //}
    }
}
