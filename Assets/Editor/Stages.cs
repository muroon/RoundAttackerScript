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
/// Stage Data Scriptable Object(保存用)
/// </summary>
[CreateAssetMenu(fileName = "StageData")]
public class StageData : ScriptableObject
{
    public Stages Stages = new Stages();
}
*/
/*
/// <summary>
/// Stage Data Scriptable Object(編集用)
/// </summary>
[CreateAssetMenu(fileName = "StageDataList")]
public class StageDataList : ScriptableObject
{
    public List<Stage> Stages = new List<Stage>();
}
*/
/*
/// <summary>
/// Stage Data Map
/// </summary>
[System.Serializable]
public class Stages : SerializableDictionary<int, Stage>
{
}

/// <summary>
/// Stage エンティティ
/// </summary>
[System.Serializable]
public struct Stage
{
    public int Id;

    public string Name;
}
*/


#if UNITY_EDITOR

/// <summary>
/// 各Assetの作成
/// </summary>
public class StageCreator : MonoBehaviour
{
    [MenuItem("GenerateData/Create Stage Data")]
    static void CreateStageDataAsset()
    {
        var userdata = ScriptableObject.CreateInstance<StageData>();

        AssetDatabase.CreateAsset(userdata, "Assets/Resources/Data/StageData.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("GenerateData/Create Stage List Data")]
    static void CreateStageListDataAsset()
    {
        var userdata = ScriptableObject.CreateInstance<StageDataList>();

        AssetDatabase.CreateAsset(userdata, "Assets/Resources/Data/StageDataList.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("GenerateData/Check Stage Data")]
    static void CheckStageDataAsset()
    {
        StageData data = Resources.Load<StageData>("Data/StageData");

        foreach (var user in data.Stages)
            Debug.LogFormat("Stage Id:{0}, EnemyId:{1}", user.Value.Id, user.Value.EnemyId);
    }
}

/// <summary>
/// StageDataListのインスペクター上の更新
/// </summary>
[CustomEditor(typeof(StageDataList))]
public class StageDataListScriptEditor : Editor
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
            StageDataList dataList = target as StageDataList;

            StageData data = Resources.Load<StageData>("Data/StageData");

            foreach (var stage in dataList.Stages) {
                if (data.Stages.ContainsKey(stage.Id)) {
                    data.Stages[stage.Id] = stage;
                } else {
                    data.Stages.Add(stage.Id, stage);
                }
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}

/// <summary>
/// 不使用
/// </summary>
[CustomPropertyDrawer(typeof(StageData))]
public class ExtendedSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{
}

#endif

