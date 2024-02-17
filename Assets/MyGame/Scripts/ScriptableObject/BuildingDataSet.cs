using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable , CustomDataSet]
[CreateAssetMenu(fileName = "BuildingDataSet", menuName = "CreateBuildingDataList")]
public class BuildingDataSet : ScriptableObject
{
    [SerializeField, Header("施設リスト")]　public BuildingData[] Buildings;
}
