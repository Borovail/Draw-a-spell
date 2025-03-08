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
            // ����� �����������
            Vector3 pos = collisionEvents[i].intersection;

            // ����������� ����: ��������������� ������� �������� �������
            Vector3 direction = -collisionEvents[i].velocity.normalized;

            // ��������� ����������� ������������
            totalForceDirection += (Vector2)direction;
            collisionCount++;
        }

        // ���� ������������ ����, ��������� ����
        if (collisionCount > 0)
        {
            // ����������� �����������, ����� ��������� ���� � ���� �������
            totalForceDirection.Normalize();

            // ���� ��������������� ���������� ������������

            // ��������� ���� � ��������� �����������
            rb.AddForce(-totalForceDirection * 1, ForceMode2D.Impulse);
            Debug.Log($"Applying force in direction: {totalForceDirection}, Force magnitude: {1}");
        }
    }
}