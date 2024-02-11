using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// クッキー(仮)をクリックしたときの処理
/// </summary>
public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnClick();
        }
        else
        {
            Debug.Log("ResourceManagerがありません");
        }
    }
}
