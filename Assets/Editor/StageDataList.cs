using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Entity;

/// <summary>
/// Stage Data Scriptable Object(編集用)
/// </summary>
[CreateAssetMenu(fileName = "StageDataList")]
public class StageDataList : ScriptableObject
{
    public List<Stage> Stages = new List<Stage>();
}