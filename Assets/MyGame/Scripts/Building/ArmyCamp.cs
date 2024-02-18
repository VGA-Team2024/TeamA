using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyCamp : BuildingBase
{
    [SerializeField , Header("1つ当たりの最大保持ユニット数")] private int _maxUnit;
    
    private int _currentUnit;
    /// <summary>
    /// Unitの最大保持数
    /// </summary>
    public int MaxUnit => _maxUnit;
    /// <summary>
    /// Unitの数
    /// </summary>
    public int CurrentUnit => _currentUnit;

    public bool IsUnitCreatable() => _currentUnit < _maxUnit;
    public Transform AddUnit()
    {
        _currentUnit ++;
        //navMeshの目的地用
        return this.transform;
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
