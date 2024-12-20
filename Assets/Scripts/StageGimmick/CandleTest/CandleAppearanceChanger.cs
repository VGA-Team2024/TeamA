using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ability;
using System;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;

public class CandleAppearanceChanger : MonoBehaviour, IResetable, IAbilityDetectable
{
    private bool _processed = true;
    private Renderer _candleRenderer;
    [SerializeField] private GameObject _candleObject = null;
    [LabelText("火がついている状態が正しい")]
    [SerializeField] private bool _isFiredCorrect = true;
    public bool IsFiredCorrect => _isFiredCorrect;
    [LabelText("火がついたときのみUnityEventを起動するかどうか")]
    [SerializeField] private bool _isInvokeIfOnlyFired = false;

    /// <summary>
    /// 現在、火がついているかを管理するブール
    /// </summary>
    public bool _isFire { get; private set; } = false;

    public bool IsEnableDetect => true;

    public UnityEvent OnStateChanged;

    private void Start()
    {
        _candleRenderer = GetComponent<Renderer>();
        if (_candleObject)
        {
            _candleObject.SetActive(false);
        }
        try
        {
            GimmickResetManager[] objects = FindObjectsByType<GimmickResetManager>(FindObjectsSortMode.None);
            foreach (var resetManager in objects)
            {
                resetManager._resetAction += ResetGimmick;
            }
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }

    /// <summary>
    /// ロウソクの火が変更された時に呼ぶ関数
    /// </summary>
    /// <param name="newState">火のオンオフ</param>
    public void SetState()
    {
        if(_isInvokeIfOnlyFired && !_isFire)
        {
            return;
        }
        OnStateChanged?.Invoke(); // イベントを発火
    }
    /// <summary>
     /// リセットアクションの追加
     /// </summary>
    public void RegisterReset()
    {
        try
        {
            GimmickResetManager[] objects = FindObjectsByType<GimmickResetManager>(FindObjectsSortMode.None);
            foreach (var resetManager in objects)
            {
                resetManager._resetAction += ResetGimmick;
            }
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }
    /// <summary>
    /// ギミックの状態をリセットする
    /// </summary>
    public void ResetGimmick()
    {
        //memo ここは複数回変更ができるかどうかで変更が入るかもしれない
        _processed = true;

        _isFire = false;
        _candleObject.SetActive(false);
        Debug.Log($"{this.gameObject.name} reset gimmick");
    }

    /// <summary>
    /// 登録したリセットアクションの解除
    /// </summary>
    public void CancelletionReset()
    {
        try
        {
            GimmickResetManager[] objects = FindObjectsByType<GimmickResetManager>(FindObjectsSortMode.None);
            foreach (var resetManager in objects)
            {
                resetManager._resetAction -= ResetGimmick;
            }
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't remove ResetGimmick ");
        }
    }
    private void OnDisable()
    {
        CancelletionReset();
    }

    public async void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        if (WandManager.CaptureAbility.Candle != ability) { return; }
        if (_processed)
        {
            _processed = false;
            _isFire = !_isFire;
            Debug.Log($"isFire:{_isFire}");
            SetState();
            if (_candleObject)
            {
                _candleObject.SetActive(_isFire);
                var _candleGimmick = this.GetComponent<TestCandleGimmick>();
                _candleGimmick.OnFire();
                if(_isFire)
                {
                    CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_fire_tukeru");
                }
                else
                {
                    CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_fire_kesu");
                }
            }
            await UniTask.WaitForSeconds(1);
            _processed = true;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
