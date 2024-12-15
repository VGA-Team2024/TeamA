using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BallonController : MonoBehaviour, IAbilityDetectable, IResetable
{
    [LabelText("移動距離")]
    [SerializeField] private float _moveDistance = 3.0f;
    [LabelText("移動時間")]
    [SerializeField] private float _moveDuration = 3.0f;
    [LabelText("停止時間")]
    [SerializeField] private float _pauseDuration = 2.0f;
    [LabelText("入力受け付けない時間")]
    [SerializeField] private float _disableDetectDuration = 1.0f;

    [Header("当たり判定")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _size;

    private Vector3 _startPos;
    private Vector3 _endPos;
    // private MotionBuilder<Vector3, NoOptions, LitMotion.Adapters.Vector3MotionAdapter> _upMoveBuilder;
    // private MotionBuilder<Vector3, NoOptions, LitMotion.Adapters.Vector3MotionAdapter> _downMoveBuilder;
    private MotionHandle _upMoveMotion;
    private MotionHandle _downMoveMotion;
    private bool _isPause = true;
    private bool _isCountingDown;
    private bool _isUp = true;

    private CharacterMovement _cache = null;

    private float _timer = 0;
    private bool _isEnableDetect = true;
    public bool IsEnableDetect => _isEnableDetect;


    // Start is called before the first frame update
    private void Start()
    {
        _startPos = this.transform.position;
        _endPos = new Vector3(_startPos.x, _startPos.y + _moveDistance, _startPos.z);

        // // 上昇と下降をまとめる
        // _downMoveBuilder = LMotion
        //    .Create(_endPos, _startPos, _moveDuration)
        //    .WithEase(Ease.InOutCubic)
        //    .WithOnComplete(async () =>
        //    {
        //        CancellationTokenSource token = new CancellationTokenSource();
        //        await StartCountdown(token.Token);
        //    })
        //    .Preserve();
        // _downMoveMotion = _downMoveBuilder.BindToPosition(transform);
        //
        // _upMoveBuilder = LMotion
        //     .Create(_startPos, _endPos, _moveDuration)
        //     .WithEase(Ease.InOutCubic)
        //     .WithOnComplete(async () =>
        //     {
        //         CancellationTokenSource token = new CancellationTokenSource();
        //         await StartCountdown(token.Token);
        //     })
        //     .Preserve();
        // _upMoveMotion = _upMoveBuilder.BindToPosition(transform);
    }

    private void Update()
    {
        if(!_isEnableDetect)
        {
            _timer += Time.deltaTime;
            if(_timer >= _disableDetectDuration)
            {
                _isEnableDetect = true;
                _timer = 0f;
            }
        }
        else
        {
            CatchPlayer();
        }
    }

    //Motionを再生するメソッド
    private void StartMotion()
    {
        _isPause = false;
        if (!_isCountingDown)
        {
            if (_isUp)
            {
                if (!_upMoveMotion.IsActive())
                {
                    PlayCurrentMotion();
                }
                _upMoveMotion.PlaybackSpeed = 1f;
            }
            else
            {
                if (!_downMoveMotion.IsActive())
                    PlayCurrentMotion();
                _downMoveMotion.PlaybackSpeed = 1f;
            }
        }
    }

    //Motionを止めるメソッド
    private void StopMotion()
    {
        _isPause = true;
        if (!_isCountingDown)
        {
            if (_isUp)
            {
                _upMoveMotion.PlaybackSpeed = 0f;
            }
            else
            {
                _downMoveMotion.PlaybackSpeed = 0f;
            }
        }
    }

    //LMotionの呼出しを行う
    private void PlayCurrentMotion()
    {
        if(_isUp)
        {
            _upMoveMotion.ToDisposable().Dispose();
            _upMoveMotion = LMotion
                .Create(_startPos, _endPos, _moveDuration)
                .WithEase(Ease.InOutCubic)
                .WithOnComplete(async () =>
                {
                    CancellationTokenSource token = new CancellationTokenSource();
                    await StartCountdown(token.Token);
                })
                .BindToPosition(transform)
                .AddTo(gameObject);
            _upMoveMotion.PlaybackSpeed = 0f;
        }
        else
        {
            _downMoveMotion.ToDisposable().Dispose();
            _downMoveMotion = LMotion
                .Create(_endPos, _startPos, _moveDuration)
                .WithEase(Ease.InOutCubic)
                .WithOnComplete(async () =>
                {
                    CancellationTokenSource token = new CancellationTokenSource();
                    await StartCountdown(token.Token);
                })
                .BindToPosition(transform)
                .AddTo(gameObject);
            _downMoveMotion.PlaybackSpeed = 0f;
        }
    }

    //カウントダウンメソッド
    private async UniTask StartCountdown(CancellationToken cancellationToken)
    {
        if (_isCountingDown) return;

        _isCountingDown = true;
        float time = 0;
        //停止時間
        while(_pauseDuration >= time)
        {
            if(!_isPause)
            {
                time += Time.deltaTime;
            }
            await UniTask.NextFrame(cancellationToken);
        }
        _isCountingDown = false;

        await UniTask.WaitUntil(() => !_isPause);

        SwitchDirection();
        PlayCurrentMotion();
        StartMotion();
    }

    private void SwitchDirection()
    {
        _isUp = !_isUp;
    }

    [Button]
    public void TestButton()
    {
        if (!_isPause)
            StopMotion();
        else
            StartMotion();
    }

    public void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        if(ability != WandManager.CaptureAbility.Test3 || !_isEnableDetect)
            return;
        _isEnableDetect = false;
        if (!_isPause)
            StopMotion();
        else
            StartMotion();
    }

    private void CatchPlayer()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.TransformVector(_offset), _size / 2f);
        bool found = false;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out CharacterMovement characterMovement))
            {
                found = true;
                if (characterMovement == _cache) { return; }
                characterMovement.SetFollowTarget(transform);
                _cache = characterMovement;
            }
        }
        if (!found && _cache != null)
        {
            _cache.SetFollowTarget(null);
            _cache = null;
        }
    }

    private void OnDestroy()
    {
        // _upMoveBuilder.Dispose();
        // _downMoveBuilder.Dispose();
        CancelletionReset();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        // ギズモで移動範囲を表示
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + _moveDistance, startPosition.z);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(endPosition, this.transform.localScale);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, endPosition);

        //当たり判定
        Gizmos.DrawCube(transform.position + transform.TransformVector(_offset), _size);
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
    /// <summary>
    /// リセットアクションの追加
    /// </summary>
    public void RegisterReset()
    {
        try
        {
            FindAnyObjectByType<GimmickResetManager>().GetComponent<GimmickResetManager>()._resetAction += ResetGimmick;
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }
    public void ResetGimmick()
    {
        _isPause = true;
        _isUp = true;

        // 初期状態で停止
        _upMoveMotion.PlaybackSpeed = 0f;
        _downMoveMotion.PlaybackSpeed = 0f;

        //位置リセット
        this.transform.position = _startPos;
    }

    public void CancelletionReset()
    {
        try
        {
            FindAnyObjectByType<GimmickResetManager>().GetComponent<GimmickResetManager>()._resetAction -= ResetGimmick;
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }
}
