using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using UnityEngine.EventSystems;

/// <summary>
/// Enumerator拡張API
/// </summary>
public static class EnumeratorExtension
{
    /// <summary>
    /// Observable経由のコルーチン起動
    /// </summary>
    /// <param name="enumerator">IEnumerator関数</param>
    /// <returns>コルーチンインスタンス</returns>
    public static Coroutine StartAsCoroutine(this IEnumerator enumerator, System.Action callback = null)
    {
        return enumerator.ToObservable().StartAsCoroutine((Unit x) => { if (callback != null) callback(); });
    }
}
