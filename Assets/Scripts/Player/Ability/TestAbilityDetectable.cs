using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TestAbilityDetectable : MonoBehaviour,IAbilityDetectable
{
    public bool IsEnableDetect => true;

    private void Start()
    {
        PlayerEventHelper.OnCaptureAbility.Subscribe(Ability => print(Ability)).AddTo(this);
    }

    public void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        switch (ability)
        {
            case WandManager.CaptureAbility.None:
                break;
            case WandManager.CaptureAbility.Candle:
                Debug.Log("Test1 Ability Detected");
                break;
            case WandManager.CaptureAbility.Marshmallow:
                Debug.Log("Test2 Ability Detected");

                break;
            case WandManager.CaptureAbility.Gum:
                Debug.Log("Test3 Ability Detected");

                break;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
