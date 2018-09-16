using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Entity;

/// <summary>
/// PlayerCharacter Data Scriptable Object(編集用)
/// </summary>
[CreateAssetMenu(fileName = "PlayerCharacterDataList")]
public class PlayerCharacterDataList : ScriptableObject
{
    public List<PlayerCharacter> PlayerCharacters = new List<PlayerCharacter>();
}