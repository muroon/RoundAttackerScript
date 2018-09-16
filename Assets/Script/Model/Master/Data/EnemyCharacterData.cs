using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;
using Model.Entity;
using System;

namespace Model.Master.Data
{
    [CreateAssetMenu(fileName = "EnemyCharacterData")]
    public class EnemyCharacterData : ScriptableObject
    {
        public EnemyCharacters EnemyCharacters = new EnemyCharacters();
    }
}