using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PendantController : StageGimmickBase, IInteractable
{
    public UnityEvent OnActivated;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "ドア開けます";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        Debug.Log("インタラクト確認");
        OnActivated?.Invoke();
        ClearActive(true);
    }
    protected override void ClearActive(bool changeIsClear)
    {
        isClear = changeIsClear;
    }
}
