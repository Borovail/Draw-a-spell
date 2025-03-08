using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

[Serializable]
public class ParticleSystemCustom
{
    public string Name;
    public ParticleSystem Value;
    public Vector2 Offset;
}

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private List<ParticleSystemCustom> _particles;

    public void SpawnParticle(string particleName,float magnitude)
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var particle = _particles.First(particle => particle.Name == particleName);
        particle.Value.transform.position = worldMousePosition + particle.Offset;

        var mainModule = particle.Value.main;

        // ��������� �������� � ����������� �� magnitude (��� �������� magnitide - ������ �������)
        float adjustedMagnitude = Mathf.Max(magnitude, 1f); // ��������, ��� magnitude �� ����� ������ 1
        mainModule.startSpeed = 1 / adjustedMagnitude;  // ������: ��� ������� magnitude, �������� ����� ������

        // ����������� �������� ������
        float adjustedScale = 1 / adjustedMagnitude; // ������: ����������� ������, ����� magnitude �������
        particle.Value.transform.localScale = new Vector3(adjustedScale, adjustedScale, 1f);

        // ����� ����� �������� ��������� ������ ������, ���� ����������
        mainModule.startSize = Mathf.Max(0.1f, 1 / adjustedMagnitude); // ������: ���� ������� �������, ��� ������
        particle.Value.gameObject.SetActive(true);
    }
}
