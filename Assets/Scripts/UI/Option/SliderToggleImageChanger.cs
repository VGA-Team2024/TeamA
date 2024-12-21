using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
public class SliderToggleImageChanger : MonoBehaviour
{
    [SerializeField] private Sprite _sprite1;
    [SerializeField] private Sprite _sprite2;

    private Image image;
    private ObservableEventTrigger trigger;

    private void Start()
    {
        if (TryGetComponent(out  image))
        {
            if(!TryGetComponent(out trigger)) { trigger = gameObject.AddComponent<ObservableEventTrigger>(); }
            trigger.OnBeginDragAsObservable()
                .Subscribe(_ =>
                {
                    image.sprite = _sprite2;
                }).AddTo(this);
            trigger.OnEndDragAsObservable()
                .Subscribe(_ =>
                {
                    image.sprite = _sprite1;
                }).AddTo(this);
            print("èâä˙âª");
        }
    }


}
