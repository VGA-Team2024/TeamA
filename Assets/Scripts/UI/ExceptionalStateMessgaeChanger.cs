using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ExceptionalStateMessgaeChanger : UIGroup
{
    [SerializeField] private UIGroup _targetUIGroup;

    public override void Initialize()
    {
        base.Initialize();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (PlayerEventHelper.IsExceptionalState())
                {
                    _targetUIGroup.GetCanvasGroup().alpha = 0;
                    gameObject.SetActive(true);
                }
                else
                {
                    _targetUIGroup.GetCanvasGroup().alpha = 1;
                    gameObject.SetActive(false);
                }
            }).AddTo(gameObject);
    }

    

}
