using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マシュマロブロック
/// </summary>
public class BlockTest : MonoBehaviour, IAbilityDetectable
{
    [SerializeField, LabelText("崩れるプロックのプレハブ")] private Transform _breakBlockPrefab;
    [SerializeField, LabelText("崩壊したパーツを消す秒数")] private float _collapseDestorySecond = 5f;

    public bool IsEnableDetect => true;

    /// <summary>
    /// 崩壊処理
    /// </summary>
    private void Collapse()
    {
        CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_clash_block");
        //分割されたブロックのPrefabを生成する
        Transform breakBlockTransform = Instantiate(_breakBlockPrefab, transform.position, Quaternion.identity);

        //崩壊したパーツを消す処理
        Destroy(breakBlockTransform.gameObject, _collapseDestorySecond);
        //元のブロックを削除
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ボタンテスト用
    /// </summary>
    public void OnButton()
    {
        Collapse();
    }

    public void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        if(ability == WandManager.CaptureAbility.Marshmallow)
        {
            Collapse();
        }
        else
        {
            Debug.Log("破壊不可");
        }
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
