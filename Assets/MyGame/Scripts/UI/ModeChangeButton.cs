using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GridSerectManagerのモードを変更するボタン
/// </summary>
public class ModeChangeButton : MonoBehaviour
{
    [SerializeField] private GridSerectManager _gridSerectManager;
    public void SetBuildMode()
    {
        Debug.Log("SetBuildMode");
        _gridSerectManager.ChangeSelectType(GridSerectManager.SelectType.SetBuildingMode);
    }
    
    public void SetSelectBuildingMode()
    {
        Debug.Log("SetSelectBuildingMode");
        _gridSerectManager.ChangeSelectType(GridSerectManager.SelectType.SelectBuildingMode);
    }
}
