using System;
using UnityEngine;

/// <summary>
/// <br>複数のギミックをリセットするための実装</br>
/// <br>今後改善する予定</br>
/// </summary>
public class GimmickResetManager : MonoBehaviour, IInteractable
{
    public Action _resetAction;

    public bool IsEnableDetect => true;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "F:リセット";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        _resetAction?.Invoke();
        Debug.Log("Interact Reset");
    }
}
