using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "開ける";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        //音
        //CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_fire_tukeru");
        SceneLoader.LoadScene("EndScene");
    }
}
