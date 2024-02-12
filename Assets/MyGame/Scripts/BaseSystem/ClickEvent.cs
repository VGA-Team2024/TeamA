using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// クッキー(仮)をクリックしたときの処理
/// </summary>
public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
    Vector3 _mousePos;
    Vector3 _worldMousePos;
    float _clickDistance = 0;
    CircleCollider2D _circleColider;
    Canvas _canvas = null;

    void Start()
    {
        _canvas = transform.parent.GetComponent<Canvas>();
        _circleColider = GetComponent<CircleCollider2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _mousePos = eventData.position;
        // キャンバスのz軸に合わせる
        Vector3 screenMousePos = new Vector3(_mousePos.x, _mousePos.y, _canvas.transform.position.z);
        // スクリーン座標からワールド座標に変換
        _worldMousePos = Camera.main.ScreenToWorldPoint(screenMousePos);
        _clickDistance = Vector2.Distance(_circleColider.transform.position, _worldMousePos);
        if (ResourceManager.Instance != null)
        {
            // 左クリックのみ反応
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // 範囲判定
                if (_clickDistance < _circleColider.bounds.extents.x / 1.07)
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
