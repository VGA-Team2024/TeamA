using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SURUTOKARI : EnemyBase
{
    [Title("�ŗL�ݒ�")]
    [SerializeField] float _sampleFloat = 1.0f;
    protected sealed override async UniTask OnAttackedActionAsync(CancellationToken cancellationToken)
    {

    }
    protected sealed override async UniTask OnDamagedActionAsync(CancellationToken cancellationToken)
    {

    }
    protected sealed override async UniTask OnDeathActionAsync(CancellationToken cancellationToken)
    {

    }
}