using System;
using System.Collections;
using Assets.Scripts;
using EZCameraShake;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    [Header("Audio Source")]
    // [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _castingSpellSource;
    [SerializeField] private AudioSource _sfxSource;
    
    [Header("Audio Clips")]
    public AudioClip casting_spell;
    public AudioClip take_damage;
    public AudioClip deal_damage;
    public AudioClip explosion;
    public AudioClip fire_spell;
    public AudioClip wind_spell;
    public AudioClip water_spell;

    private void Start()
    {
        _castingSpellSource.clip = casting_spell;
    }

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    public void DealDamageEffect()
    {
        _sfxSource.PlayOneShot(take_damage);
        CameraShaker.Instance.ShakeOnce(2,2,0,1);
    }

    public void StartCastingSpellEffect()
    {
        _castingSpellSource.Play();
    }
    
    public void StopCastingSpellEffect()
    {
        _castingSpellSource.Stop();
    }
    
    public void TakeDamageEffect()
    {
        _sfxSource.PlayOneShot(deal_damage);
        CameraShaker.Instance.ShakeOnce(3,3,0,1);
    }
    
    public void FireSpellEffect()
    {
        _sfxSource.PlayOneShot(fire_spell);
    }
    
    public void WindSpellEffect()
    {
        _sfxSource.PlayOneShot(wind_spell);
    }
    
    public void WaterSpellEffect()
    {
        _sfxSource.PlayOneShot(water_spell);
    }

    public void ExplosionEffect()
    {
        _sfxSource.PlayOneShot(explosion);
        CameraShaker.Instance.ShakeOnce(3,6,0,2);
    }
}