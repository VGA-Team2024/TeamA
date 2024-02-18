using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable , CustomDataSet]
[CreateAssetMenu(fileName = "BuildingDataSet", menuName = "CreateBuildingDataList")]
public class BuildingDataSet : ScriptableObject
{
    [SerializeField, Header("施設リスト")]　public BuildingData[] Buildings;
}

[Serializable]
public class BuildingDataPair
{
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private BuildingData _buildingData;
    public GameObject BuildingPrefab => _buildingPrefab;
    public BuildingData BuildingData => _buildingData;
}
