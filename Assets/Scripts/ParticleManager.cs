using System;
using UnityEngine;

[Serializable]
public class ParticleSystemCustom
{
    public ParticleSystem Particle;
    public Vector2 Offset;
}

public class ParticleManager : MonoBehaviour
{
    public ParticleSystemCustom _leftClickParticle;
    public ParticleSystemCustom _rightClickParticle;

    private void Update()
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _leftClickParticle.Particle.transform.position = worldMousePosition + _leftClickParticle.Offset;
            _leftClickParticle.Particle.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _rightClickParticle.Particle.transform.position = worldMousePosition + _rightClickParticle.Offset;
            _rightClickParticle.Particle.gameObject.SetActive(true);
            Debug.Log(worldMousePosition);
        }

    }
}
