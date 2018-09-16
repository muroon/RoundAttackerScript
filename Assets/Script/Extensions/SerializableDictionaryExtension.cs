using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;

public static class SerializableDictionaryExtension
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value.</returns>
    /// <param name="dic">Dic.</param>
    /// <param name="key">Key.</param>
    /// <typeparam name="TKey">The 1st type parameter.</typeparam>
    /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
    public static TValue GetValue<TKey, TValue>(this SerializableDictionary<TKey, TValue> dic, TKey key)
    {
        TValue value;
        if (!dic.TryGetValue(key, out value))
        {
            throw new ArgumentException(string.Format("{0} isn't included in {1}", key, typeof(TValue).Name));
        }
        return value;
    }
}