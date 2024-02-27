using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オーディオを管理するクラス
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

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
                break;
            case SEName.Battle:
                break;
            case SEName.Building:
                break;
        }
        _seSource.PlayOneShot(clip);
    }
}
