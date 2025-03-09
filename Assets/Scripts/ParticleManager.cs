using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using FreeDraw;
using TMPro;
using UnityEngine;

[Serializable]
public class ParticleSystemCustom
{
    public string Name;
    public ParticleSystem Value;
    [HideInInspector] public ParticleCollisionHandler CollisionHandler;
    public Vector2 Offset;
    [HideInInspector] public float Magnitude;
}

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private List<ParticleSystemCustom> _particles;
    [SerializeField] private TextMeshProUGUI _recognitionResult = null!;

    private ParticleSystemCustom _currentSelected;

    private void Start()
    {
        foreach (var particle in _particles)
        {
            particle.CollisionHandler = particle.Value.GetComponent<ParticleCollisionHandler>();
            if(particle.CollisionHandler==null)
                particle.CollisionHandler = particle.Value.GetComponentInChildren<ParticleCollisionHandler>();
        }
    }

    private void Update()
    {
        if (!Drawable.drawable.enabled && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Drawable.drawable.enabled = true;
            _recognitionResult.text += "      Drawing!!!";
        }

        if (!Drawable.drawable.enabled && Input.GetKeyDown(KeyCode.Mouse0) && _currentSelected!=null && !_currentSelected.Value.gameObject.activeSelf)
        {
            SpawnParticle(_currentSelected.Name, _currentSelected.Magnitude);
        }
    }

    public void SpawnParticle(string particleName, float weaknessFactor)
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentSelected = _particles.First(particle => particle.Name == particleName);
        _currentSelected.Value.transform.position = worldMousePosition + _currentSelected.Offset;
        _currentSelected.Magnitude = Mathf.Max(weaknessFactor, 1f);
        _currentSelected.CollisionHandler.WeaknessFactor = _currentSelected.Magnitude;


        //var mainModule = _currentSelected.Value.main;

        //// ��������� �������� � ����������� �� magnitude (��� �������� magnitide - ������ �������)
        //float adjustedMagnitude = Mathf.Max(magnitude, 1f); // ��������, ��� magnitude �� ����� ������ 1
        //mainModule.startSpeed = 1 / adjustedMagnitude;  // ������: ��� ������� magnitude, �������� ����� ������

        // ����������� �������� ������
        float adjustedScale = 1 / _currentSelected.Magnitude; // ������: ����������� ������, ����� magnitude �������
        _currentSelected.Value.transform.localScale = new Vector3(adjustedScale, adjustedScale, 1f);

        //// ����� ����� �������� ��������� ������ ������, ���� ����������
        //mainModule.startSize = Mathf.Max(0.1f, 1 / adjustedMagnitude); // ������: ���� ������� �������, ��� ������
        _currentSelected.Value.gameObject.SetActive(true);
    }
}
