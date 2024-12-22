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
        CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_clash_pendant");
    }
    protected override void ClearActive(bool changeIsClear)
    {
        isClear = changeIsClear;
    }
}
