using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips")]
    public AudioClip SpellDrawn;
    public AudioClip SpellCasted;
    public AudioClip MonsterDeath;
    public AudioClip SpellDrawnIncorrectly;

    private void Start()
    {
        _musicSource.Play();
    }

    public void PlaySfx(AudioClip clip, float volume=0)
    {
        if (volume != 0)
            _sfxSource.volume = volume;
        _sfxSource.PlayOneShot(clip);
    }

    private Vector3 originalCameraPos;
    private float shakeDuration = 0.2f;
    private float shakeMagnitude = 0.1f;

    public void SpellDrawnIncorrectlyEffect(float volume =0f)
    {
        PlaySfx(SpellDrawnIncorrectly, volume);
        StartCoroutine(CameraShake());
    }

    private IEnumerator CameraShake()
    {
        originalCameraPos = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            Camera.main.transform.position = originalCameraPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originalCameraPos;
    }

}