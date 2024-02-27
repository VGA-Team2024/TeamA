using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSoldierManager : MonoBehaviour
{
    [SerializeField] Button _createButton;
    [SerializeField] Button _deathButton;
    [SerializeField] SoldierManager _soldierManager;
    [SerializeField] int _soldierCount = 1;
    
    private void Start()
    {
        //_createButton.onClick.AddListener(() => _soldierManager.AddSoldier(Vector3.zero, _soldierCount));
        _deathButton.onClick.AddListener(() => _soldierManager.RemoveSoldier(_soldierCount));
    }
}
