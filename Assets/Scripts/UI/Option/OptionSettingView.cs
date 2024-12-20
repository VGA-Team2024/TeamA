using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettingView : OptionViewBase
{
    [SerializeField] private Slider _cameraSenseSlider;
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _cvVolumeSlider;
    [SerializeField] private Slider _seVolumeSlider;
    [SerializeField] private Button _cancelButton;

    public IObservable<Unit> OnCancelButtonPressed => _cancelButton.OnClickAsObservable();
    public override void Entry()
    {
        _cameraSenseSlider.onValueChanged.AsObservable()
            .Subscribe(value => { MessageBroker.Default.Publish(new CameraSensePram { value = value }); })
            .AddTo(this);
        _mainVolumeSlider.onValueChanged.AsObservable()
            .Subscribe(value => { MessageBroker.Default.Publish(new MainVolumePram { value = value }); })
            .AddTo(this); ;
        _cvVolumeSlider.onValueChanged.AsObservable()
            .Subscribe(value => { MessageBroker.Default.Publish(new CvVolumePram { value = value }); })
            .AddTo(this); ;
        _seVolumeSlider.onValueChanged.AsObservable()
            .Subscribe(value => { MessageBroker.Default.Publish(new SeVolumePram { value = value }); })
            .AddTo(this); ;
    }




}
public class CameraSensePram { public float value = 0; }
public class MainVolumePram { public float value = 0; }
public class CvVolumePram { public float value = 0; }
public class SeVolumePram { public float value = 0; }

