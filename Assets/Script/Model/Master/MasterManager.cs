using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Master.Data;

namespace Model.Master
{
    public static class MasterManager
    {
        public static Stages Stages { get; private set; }

        public static PlayerCharacters PlayerCharacters { get; private set; }

        public static EnemyCharacters EnemyCharacters { get; private set; }

        /// <summary>
        /// マスタ読み込み開始
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void StartLoadData()
        {
            var data = Resources.Load<StageData>("Data/StageData");
            Stages = data.Stages;

            var playerData = Resources.Load<PlayerCharacterData>("Data/PlayerCharacterData");
            PlayerCharacters = playerData.PlayerCharacters;

            var enemyData = Resources.Load<EnemyCharacterData>("Data/EnemyCharacterData");
            EnemyCharacters = enemyData.EnemyCharacters;
        }
    }
}