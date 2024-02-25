using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ArmyCamp : BuildingBase
{
    [SerializeField , Header("1つ当たりの最大保持ユニット数")] private int _maxUnit;
    private readonly Stack<GameObject> _unitList = new Stack<GameObject>(50);
    
    private int _currentUnit;
    /// <summary>
    /// Unitの最大保持数
    /// </summary>
    public int MaxUnit => _maxUnit;
    /// <summary>
    /// Unitの数
    /// </summary>
    public int CurrentUnit => _unitList.Count;

    public bool IsUnitCreatable() => _unitList.Count < _maxUnit;
    public bool IsUnitRemovable() => _unitList.Count > 0;
    
    
    public Transform AddUnit(GameObject soldier)
    {
        _unitList.Push(soldier);
        //navMeshの目的地用
        //TODOここでsoldierをSetDestinationしてもいい
        return transform;
    }

    public void RemoveUnit()
    {
        var soldier = _unitList.Pop();
        Destroy(soldier);
    }
    
    // Start is called before the first frame update
    public override void OnClick()
    {
        
    }

    protected override void OnStart()
    {

    }

    protected override void OnFixedUpdate()
    {

    }
    
}
