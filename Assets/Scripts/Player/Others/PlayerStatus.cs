using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DamageSystem;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class PlayerStatus : MonoBehaviour,DamageSystem.IDamagable
{
    private void Awake()
    {

        Health = new ObservableStatus(3, 3);
        Observable.FromEvent<bool>(
            f => PlayerEventHelper.SetPlayerInvulnerable += f,
            f => PlayerEventHelper.SetPlayerInvulnerable -= f)
            .Subscribe(inv =>
            {
                IsInvulnerable = inv;
            }).AddTo(this.gameObject);


        Health.Where(status => status.GetStatus().value == 0)
            .Subscribe(_ => { PlayerEventHelper.OnPlayerDie?.Invoke(); })
            .AddTo(this);
        #region Test
        //this.UpdateAsObservable()
        //    .Where(_ => Input.GetKeyDown(KeyCode.Return))
        //    .Subscribe(_ =>
        //    {
        //        PlayerManager.Instance.TryGetPlayerRef(out var player);
        //        if (player.TryGetComponent(out DamageSystem.IDamagable damagable))
        //        {
        //            damagable.ApplyDamage(1f);
        //        }
        //    }).AddTo(this);
        //    PlayerEventHelper.OnPlayerDie += () => print("Die");
        //    this.UpdateAsObservable()
        //.Where(_ => Input.GetKeyDown(KeyCode.Space))
        //.Subscribe(_ =>
        //{
        //    PlayerEventHelper.SetPlayerInvulnerable(true);
        //}).AddTo(this);

        //MessageBroker.Default.Receive<CameraSensePram>().Subscribe(param => print($"カメラ感度 {param.value}")).AddTo(this);
        //MessageBroker.Default.Receive<MainVolumePram>().Subscribe(param => print($"主音量 {param.value}")).AddTo(this);
        //MessageBroker.Default.Receive<CvVolumePram>().Subscribe(param => print($"CV音量 {param.value}")).AddTo(this);
        //MessageBroker.Default.Receive<SeVolumePram>().Subscribe(param => print($"SE音量 {param.value}")).AddTo(this);

        //MessageBroker.Default.Receive<OptionExit>().Subscribe(_ => { print("終了"); }).AddTo(this);
        #endregion
    }

    bool IsForcedInvulnerable = false;


    public bool IsInvulnerable { get; set; } = false;

    public bool ApplyDamage(float damage, IDamageArg arg = null)
    {
        if (IsInvulnerable) { return false; }
        if (IsForcedInvulnerable) { return false; }

        Health.Value -= (int)damage;

        UniTask.Create(async () =>
        {
            IsForcedInvulnerable = true;
            await UniTask.Delay(Mathf.RoundToInt(PlayerEventHelper.InvulnerableTime * 1000));
            IsForcedInvulnerable = false;
        }).Forget();

        return true;

    }

    public ObservableStatus Health { get; private set; } = null;

}
