using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;
using Model.Entity;
using System;

namespace Model.Master.Data
{
    [CreateAssetMenu(fileName = "StageData")]
    public class StageData : ScriptableObject
    {
        public Stages Stages = new Stages();
    }
}