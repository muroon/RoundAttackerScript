using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;
using Model.Entity;
using System;

namespace Model.Master.Data
{
    [Serializable]
    public class EnemyCharacters : SerializableDictionary<int, EnemyCharacter>
    {
    }
}