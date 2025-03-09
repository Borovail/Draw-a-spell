using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Color = UnityEngine.Color;

public class ParticleCollisionHandler : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    [HideInInspector] public float WeaknessFactor = 1;
    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {

        // �������� ������� ������������
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        // �������� Rigidbody ������� ��� ���������� ����
        var rb = other.GetComponent<Rigidbody2D>();

        // ���������� ��� ���������� ��������� ����
        Vector2 totalForceDirection = Vector2.zero;
        int collisionCount = 0;

        // �������� �� ���� �������� ������������
        for (int i = 0; i < numCollisionEvents; i++)
        {
            // ����������� ����: ��������������� ������� �������� �������
            if (collisionEvents[i].colliderComponent.gameObject.TryGetComponent<RandomMover>(out var mover))
                mover.enabled = false;
            collisionEvents[i].colliderComponent.gameObject.GetComponent<Collider2D>().enabled = false;

            Vector3 direction = collisionEvents[i].velocity.normalized;
            var renderer = collisionEvents[i].colliderComponent.gameObject.GetComponent<SpriteRenderer>();
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 100f / 255f);
            // ��������� ����������� ������������
            totalForceDirection += (Vector2)direction;
            collisionCount++;

            // ������������� ��������� �������� (������) �������
            float particleSpeed = collisionEvents[i].velocity.magnitude;

            // ������������ ���� � ����������� �� �������� �������
            float forceMagnitude = particleSpeed / WeaknessFactor; // ������������ � ������������� 10 ��� ��������� �������

            // ��������� ���� � ����������� �� �������� �������
            rb.AddForce(totalForceDirection.normalized * forceMagnitude, ForceMode2D.Impulse);
            Destroy(collisionEvents[i].colliderComponent.gameObject, 2f);
        }
        SFXManager.Instance.PlaySfx(SFXManager.Instance.MonsterDeath, 0.1f);

        //// ���� ������������ ����, ��������� ����
        //if (collisionCount > 0)
        //{
        //    // ����������� �����������, ����� ��������� ���� � ���� �������
        //    totalForceDirection.Normalize();

        //    // ��������� ����� ���� (����� ��������, ���� ������ ��������)
        //    rb.AddForce(totalForceDirection * 1, ForceMode2D.Impulse);
        //}
    }
}
