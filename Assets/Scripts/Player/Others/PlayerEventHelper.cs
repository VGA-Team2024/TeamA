using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public static class PlayerEventHelper
{
    public static Subject<WandManager.CaptureAbility> OnCaptureAbility = new();
    public static Action<bool> SetPlayerInvulnerable;
    public static Action OnPlayerDie;
    public static Action<bool> SetPlayerStateAsOperatingPlatform;
    public static float InvulnerableTime => 2.0f;
    public static bool IsExceptionalState()
    {
        if (PlayerManager.Instance.TryGetPlayerRef(out var player))
        {
            return player.GetComponent<PlayerStateMachine>().IsPlayerExceptionalState();
        }
        else
        {
            return false;
        }
    }
} 

