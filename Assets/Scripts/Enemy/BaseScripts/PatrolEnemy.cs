using System;
using UnityEngine;

//������

[Serializable]
/// <summary>���񂷂���</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Transform[] _patrolPositions;

    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (Vector3.zero, Vector3.zero);
    }
}
