using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// クッキー(仮)をクリックしたときの処理
/// </summary>
public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Canvas _canvas;
    [SerializeField] RectTransform _rectTransform;
    float _clickDistance = 0;
    float _radius = 0;

    void Start()
    {
        _radius = _rectTransform.sizeDelta.x / 2 * _canvas.scaleFactor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clickDistance = Vector2.Distance(_rectTransform.transform.position, eventData.position);
        _radius = _rectTransform.sizeDelta.x / 2 * _canvas.scaleFactor;
        if (ResourceManager.Instance != null)
        {
            // 左クリックのみ反応
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // 範囲判定
                if (_clickDistance < _radius)
                {
                    ResourceManager.Instance.OnClick();
                }
            }
        }
        else
        {
            Debug.Log("ResourceManagerがありません");
        }
    }
}
