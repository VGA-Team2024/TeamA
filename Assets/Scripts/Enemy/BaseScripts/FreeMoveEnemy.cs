using System;
using UnityEngine;

//������

[Serializable]
/// <summary>���R�ɓ��������</summary>
public sealed class FreeMoveEnemy : IMovePatternEnemy
{

    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (Vector3.zero, Vector3.zero);
    }
}