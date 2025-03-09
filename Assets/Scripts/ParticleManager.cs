using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using FreeDraw;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ParticleSystemCustom
{
    public string Name;
    public ParticleSystem Value;
    [HideInInspector] public ParticleCollisionHandler CollisionHandler;
    public Vector2 Offset;
    [HideInInspector] public float Scale;
    [HideInInspector] public float Magnitude;
}

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private List<ParticleSystemCustom> _particles;

    private ParticleSystemCustom _currentSelected;

    private void Start()
    {
        foreach (var particle in _particles)
        {
            particle.Scale = particle.Value.transform.localScale.x;
            particle.CollisionHandler = particle.Value.GetComponent<ParticleCollisionHandler>();
            if (particle.CollisionHandler == null)
                particle.CollisionHandler = particle.Value.GetComponentInChildren<ParticleCollisionHandler>();
        }
    }

    private void Update()
    {

        if (!Drawable.drawable.enabled && Input.GetKeyDown(KeyCode.Mouse0) && _currentSelected != null && !_currentSelected.Value.gameObject.activeSelf)
        {
            SpawnParticle();
            _currentSelected = null;
            Invoke(nameof(EnableDrawing), 0.2f);
        }

    }

    public void EnableDrawing()
    {
        Drawable.drawable.enabled = true;
    }

    public void SetParticle(string particleName, float weaknessFactor)
    {
        _currentSelected = _particles.FirstOrDefault(particle => particle.Name == particleName);
        _currentSelected.Magnitude = Mathf.Max(weaknessFactor, 1f);
    }

    private void SpawnParticle()
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentSelected.Value.transform.position = worldMousePosition + _currentSelected.Offset;
        _currentSelected.CollisionHandler.WeaknessFactor = _currentSelected.Magnitude;

        //var mainModule = _currentSelected.Value.main;

        //// ��������� �������� � ����������� �� magnitude (��� �������� magnitide - ������ �������)
        //float adjustedMagnitude = Mathf.Max(magnitude, 1f); // ��������, ��� magnitude �� ����� ������ 1
        //mainModule.startSpeed = 1 / adjustedMagnitude;  // ������: ��� ������� magnitude, �������� ����� ������

        _currentSelected.Value.transform.localScale = new Vector3(_currentSelected.Scale / _currentSelected.Magnitude, _currentSelected.Scale / _currentSelected.Magnitude, 1f);

        //// ����� ����� �������� ��������� ������ ������, ���� ����������
        //mainModule.startSize = Mathf.Max(0.1f, 1 / adjustedMagnitude); // ������: ���� ������� �������, ��� ������
        _currentSelected.Value.gameObject.SetActive(true);
    }
}
