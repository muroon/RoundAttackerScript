using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;
using Model.Entity;
using Model.Master.Data;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
/// <summary>
/// PlayerCharacter Data Scriptable Object(保存用)
/// </summary>
[CreateAssetMenu(fileName = "PlayerCharacterData")]
public class PlayerCharacterData : ScriptableObject
{
    public PlayerCharacters PlayerCharacters = new PlayerCharacters();
}
*/
/*
/// <summary>
/// PlayerCharacter Data Scriptable Object(編集用)
/// </summary>
[CreateAssetMenu(fileName = "PlayerCharacterDataList")]
public class PlayerCharacterDataList : ScriptableObject
{
    public List<PlayerCharacter> PlayerCharacters = new List<PlayerCharacter>();
}
*/
/*
/// <summary>
/// PlayerCharacter Data Map
/// </summary>
[System.Serializable]
public class PlayerCharacters : SerializableDictionary<int, PlayerCharacter>
{
}

/// <summary>
/// PlayerCharacter エンティティ
/// </summary>
[System.Serializable]
public struct PlayerCharacter
{
    public int Id;

    public string Name;
}
*/


#if UNITY_EDITOR

/// <summary>
/// 各Assetの作成
/// </summary>
public class PlayerCharacterCreator : MonoBehaviour
{
    [MenuItem("GenerateData/Create PlayerCharacter Data")]
    static void CreatePlayerCharacterDataAsset()
    {
        var characterdata = ScriptableObject.CreateInstance<PlayerCharacterData>();

        AssetDatabase.CreateAsset(characterdata, "Assets/Resources/Data/PlayerCharacterData.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("GenerateData/Create PlayerCharacter List Data")]
    static void CreatePlayerCharacterListDataAsset()
    {
        var characterdata = ScriptableObject.CreateInstance<PlayerCharacterDataList>();

        AssetDatabase.CreateAsset(characterdata, "Assets/Resources/Data/PlayerCharacterDataList.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("GenerateData/Check PlayerCharacter Data")]
    static void CheckPlayerCharacterDataAsset()
    {
        PlayerCharacterData data = Resources.Load<PlayerCharacterData>("Data/PlayerCharacterData");

        foreach (var character in data.PlayerCharacters)
            Debug.LogFormat("PlayerCharacter Id:{0}, Name:{1}", character.Value.Id, character.Value.Name);
    }
}

/// <summary>
/// PlayerCharacterDataListのインスペクター上の更新
/// </summary>
[CustomEditor(typeof(PlayerCharacterDataList))]
public class PlayerCharacterDataListScriptEditor : Editor
{

    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        //ボタンを表示
        if (GUILayout.Button("Update"))
        {
            PlayerCharacterDataList dataList = target as PlayerCharacterDataList;

            PlayerCharacterData data = Resources.Load<PlayerCharacterData>("Data/PlayerCharacterData");

            foreach (var character in dataList.PlayerCharacters) {
                if (data.PlayerCharacters.ContainsKey(character.Id)) {
                    data.PlayerCharacters[character.Id] = character;
                } else {
                    data.PlayerCharacters.Add(character.Id, character);                    
                }

            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif

