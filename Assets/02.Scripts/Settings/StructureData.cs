using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Structure", menuName = "new Structure")]//새로만들기창에 생성
public class StructureData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public GameObject[] dropPrefab;
}
