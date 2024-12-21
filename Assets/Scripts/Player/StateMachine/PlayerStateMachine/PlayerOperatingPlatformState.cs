using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PlayerOperatingPlatformState : PlayerBaseState
{
    private readonly int WalkStateHash = Animator.StringToHash("Walk");
    private readonly int SpeedPramHash = Animator.StringToHash("Speed");
    private const float TransitionDuration = 0.1f;
    private CompositeDisposable disposables = new();
    private CancellationTokenSource cts = new();
    public PlayerOperatingPlatformState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        UniTask.Create(async () =>
        {
            float timeCount = 0;
            while (true)
            {
                if (cts.IsCancellationRequested) { break; }
                if (timeCount >= 1f || GetAnimationNormalizedTime(_stateMachine.Animator) >= 1f) { break; }
                await UniTask.NextFrame(cts.Token);
                timeCount += Time.deltaTime;
            }

            if (cts.IsCancellationRequested) { return; }

            _stateMachine.Animator.CrossFadeInFixedTime(WalkStateHash, TransitionDuration);
            _stateMachine.Animator.SetFloat(SpeedPramHash, 0);
        }).Forget();


        InputReader.Instance.OnSkillAsObservable()
             .Where(c => c.performed)
             .Subscribe(_ =>
             {
                 PlayerEventHelper.SetPlayerStateAsOperatingPlatform(false);
             }).AddTo(disposables);


    }

    public override void Tick(float deltaTime)
    {
    }
    public override void Exit()
    {
        cts.Cancel();
        disposables.Dispose();
    }

  
}
