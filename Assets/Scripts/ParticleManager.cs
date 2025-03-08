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

        // Уменьшаем скорость в зависимости от magnitude (для большего magnitide - слабее партикл)
        float adjustedMagnitude = Mathf.Max(magnitude, 1f); // Убедимся, что magnitude не будет меньше 1
        mainModule.startSpeed = 1 / adjustedMagnitude;  // Пример: при большем magnitude, скорость будет меньше

        // Регулировка масштаба частиц
        float adjustedScale = 1 / adjustedMagnitude; // Пример: увеличиваем размер, когда magnitude большое
        particle.Value.transform.localScale = new Vector3(adjustedScale, adjustedScale, 1f);

        // Также можно изменить стартовый размер частиц, если необходимо
        mainModule.startSize = Mathf.Max(0.1f, 1 / adjustedMagnitude); // Пример: если частица сильнее, она больше
        particle.Value.gameObject.SetActive(true);
    }
}
