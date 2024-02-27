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
        _gridSerectManager.ChangeSelectType(GridSerectManager.SelectType.SelectBuildingMode);
    }
    
    public void SetSelectBuildingMode()
    {
        _gridSerectManager.ChangeSelectType(GridSerectManager.SelectType.SelectBuildingMode);
    }
}
