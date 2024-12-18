using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionIntroductionView : OptionViewBase
{
    [SerializeField] private Button _cancelButton;

    public IObservable<Unit> OnCancelButtonPressed => _cancelButton.onClick.AsObservable();
    public override void Entry()
    {
        
    }


}
