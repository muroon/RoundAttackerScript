using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Entity;

/// <summary>
/// EnemyCharacter Data Scriptable Object(編集用)
/// </summary>
[CreateAssetMenu(fileName = "EnemyCharacterDataList")]
public class EnemyCharacterDataList : ScriptableObject
{
    public List<EnemyCharacter> EnemyCharacters = new List<EnemyCharacter>();
}
