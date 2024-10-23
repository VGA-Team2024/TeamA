using Alchemy.Inspector;
using System;
using UnityEngine;

[Serializable]
/// <summary>�ꂩ���������</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3 _guardPosition;
    [SerializeField] private Vector3 _guardFrontAngle;
    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (_guardPosition, _guardFrontAngle);
    }
#if UNITY_EDITOR
    /// <summary>
    /// 
    /// �����̉��z�N���X�ȊO�ŌĂяo���Ȃ�����
    /// </summary>
    [Button]
    public void GenerateGuradPosition() 
    {

    }
#endif
}
