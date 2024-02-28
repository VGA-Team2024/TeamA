using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オーディオを管理するクラス
/// </summary>
public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;
    [SerializeField] private AudioClip _clickSE;
    [SerializeField] private AudioClip _battleSE;
    [SerializeField] private AudioClip _buildingSE;

    private void Start()
    {
        _bgmSource.Play();
    }
    
    public enum  SEName
    {
        Click,
        Battle,
        Building,
    }
    
    public void PlaySE(AudioClip clip, SEName seName)
    {
        switch (seName)
        {
            case SEName.Click:
                _seSource.PlayOneShot(_clickSE);
                break;
            case SEName.Battle:
                _seSource.PlayOneShot(_battleSE);
                break;
            case SEName.Building:
                _seSource.PlayOneShot(_buildingSE);
                break;
        }
        _seSource.PlayOneShot(clip);
    }
}
