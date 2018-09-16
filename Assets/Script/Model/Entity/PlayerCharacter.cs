using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Model.Entity 
{
    [Serializable]
    public struct PlayerCharacter
    {
        public int Id;

        public string Name;

        public int MaxHp;

        public int AttackPower;

        public int DefensePower;

        public string Resource;
    }
}