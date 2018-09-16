using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;
using Model.Entity;
using System;

namespace Model.Master.Data
{
    [CreateAssetMenu(fileName = "PlayerCharacterData")]
    public class PlayerCharacterData : ScriptableObject
    {
        public PlayerCharacters PlayerCharacters = new PlayerCharacters();
    }
}