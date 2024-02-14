using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
    [SerializeField] private BuildingData _buildingData;
    public BuildingData BuildingData => _buildingData;
    
    private bool _isBuilding;
    private bool _isActivate;
    private float _currentBuildTime;
    protected abstract void OnFixedUpdate();
    
    public void Initialize()
    {
        
    }
    
    private void FixedUpdate()
    {
        
        if (_isActivate)
        {
            OnFixedUpdate();
        }
        else
        {
            //建設する
            _currentBuildTime += Time.fixedDeltaTime;
            if (_currentBuildTime > _buildingData.BuildTime)
                _isActivate = true;
        }
        
    }
    
}
