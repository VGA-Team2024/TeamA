using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 鉱山
/// </summary>
public class Mine : BuildingBase
{
    [SerializeField, Header("TestUI / Gold表示")] private Text _goldText;
    [SerializeField, Header("gold生産量/毎秒")] private float _generateGold;
    [SerializeField, Header("保持上限")] private float _maxStorage;
    private float _currentResource;
    private ResourceManager _resourceManager;
    protected override void OnStart()
    {
        _resourceManager = ResourceManager.Instance;
    }

    protected override void OnFixedUpdate()
    {
        if (_currentResource < _maxStorage)
        {
            _currentResource += _generateGold * Time.fixedDeltaTime;
            _goldText.text = _currentResource.ToString("0");
        }
    }

    public override void OnClick()
    {
        _resourceManager.AddResources(_currentResource);
        _currentResource = 0;
    }
}
