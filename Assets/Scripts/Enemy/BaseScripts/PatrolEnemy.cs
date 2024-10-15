using System;
using UnityEngine;

[Serializable]
/// <summary>���񂷂���</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Transform[] _patrolPositions;

    private Action MoveEnemyPatrol;
}
