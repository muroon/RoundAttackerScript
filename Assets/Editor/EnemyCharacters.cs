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
/// EnemyCharacter Data Scriptable Object(保存用)
/// </summary>
[CreateAssetMenu(fileName = "EnemyCharacterData")]
public class EnemyCharacterData : ScriptableObject
{
    public EnemyCharacters EnemyCharacters = new EnemyCharacters();
}
*/
/*
/// <summary>
/// EnemyCharacter Data Scriptable Object(編集用)
/// </summary>
[CreateAssetMenu(fileName = "EnemyCharacterDataList")]
public class EnemyCharacterDataList : ScriptableObject
{
    public List<EnemyCharacter> EnemyCharacters = new List<EnemyCharacter>();
}
*/
/*
/// <summary>
/// EnemyCharacter Data Map
/// </summary>
[System.Serializable]
public class EnemyCharacters : SerializableDictionary<int, EnemyCharacter>
{
}

/// <summary>
/// EnemyCharacter エンティティ
/// </summary>
[System.Serializable]
public struct EnemyCharacter
{
    public int Id;

    public string Name;
}
*/


#if UNITY_EDITOR

/// <summary>
/// 各Assetの作成
/// </summary>
public class EnemyCharacterCreator : MonoBehaviour
{
    [MenuItem("GenerateData/Create EnemyCharacter Data")]
    static void CreateEnemyCharacterDataAsset()
    {
        var characterdata = ScriptableObject.CreateInstance<EnemyCharacterData>();

        AssetDatabase.CreateAsset(characterdata, "Assets/Resources/Data/EnemyCharacterData.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("GenerateData/Create EnemyCharacter List Data")]
    static void CreateEnemyCharacterListDataAsset()
    {
        var characterdata = ScriptableObject.CreateInstance<EnemyCharacterDataList>();

        AssetDatabase.CreateAsset(characterdata, "Assets/Resources/Data/EnemyCharacterDataList.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("GenerateData/Check EnemyCharacter Data")]
    static void CheckEnemyCharacterDataAsset()
    {
        EnemyCharacterData data = Resources.Load<EnemyCharacterData>("Data/EnemyCharacterData");

        foreach (var character in data.EnemyCharacters)
            Debug.LogFormat("EnemyCharacter Id:{0}, Name:{1}", character.Value.Id, character.Value.Name);
    }
}

/// <summary>
/// EnemyCharacterDataListのインスペクター上の更新
/// </summary>
[CustomEditor(typeof(EnemyCharacterDataList))]
public class EnemyCharacterDataListScriptEditor : Editor
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
            EnemyCharacterDataList dataList = target as EnemyCharacterDataList;

            EnemyCharacterData data = Resources.Load<EnemyCharacterData>("Data/EnemyCharacterData");

            foreach (var character in dataList.EnemyCharacters) {
                if (data.EnemyCharacters.ContainsKey(character.Id)) {
                    data.EnemyCharacters[character.Id] = character;
                } else {
                    data.EnemyCharacters.Add(character.Id, character);
                }
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif
