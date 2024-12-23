using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 継承前提
/// 各ステージの最初にやりたい処理をそれぞれするやつ
/// </summary>
public class BossStageFacade : MainStageFacade
{
    [SerializeField] Animator _animator;
    public void BossDeathState()
    {
        _animator.SetTrigger("Death");
    }
    public void BossDamageState()
    {
        _animator.SetTrigger("Damage");
    }
    public void BossAttackState()
    {
        _animator.SetTrigger("Attack");
    }
}
