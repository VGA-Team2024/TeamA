using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerHealthPresenter : UIGroup
{
    [SerializeField] private List<HelthUIVIew> _helthUIVIews = new();
    
    [SerializeField]PlayerStatus status;

    //–{—ˆ‚ÍPlayerStatus‚ÌHealth‚É‚àSubscribe‚·‚é

    public override void Initialize()
    {
        _helthUIVIews.ForEach(view =>
        {
            view.SetView(WandManager.CaptureAbility.None);
        });

        base.Initialize();
        PlayerEventHelper.OnCaptureAbility
            .Subscribe((ability) =>
            {
                _helthUIVIews.ForEach(view =>
                {
                    view.SetView(ability);
                });
            }).AddTo(this);

        status.Health.Subscribe(status =>
        {
            int helath = Mathf.RoundToInt(status.GetStatus().value);
            for (int i = 0; i < _helthUIVIews.Count; i++)
            {
                if (i >= helath)
                {
                    _helthUIVIews[i].gameObject.SetActive(false);
                }
            }
        }).AddTo(this);
    }

}
