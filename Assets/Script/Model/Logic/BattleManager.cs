using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Master;
using Model.Entity;

namespace Model.Logic
{
    public static class BattleManager
    {
        public static int StageId { get; private set; }

        public static PlayerCharacter Player { get; private set; }

        public static void InitializeStage()
        {
            StageId = 1;
        }

        public static void SetNextStage()
        {
            if (StageId + 1 > MasterManager.Stages.Count)
            {
                InitializeStage();
            }
            else
            {
                StageId += 1;
            }
        }

        public static void SetPlayerCharacter(int id)
        {
            Player = MasterManager.PlayerCharacters.GetValue(id);
        }

        public static BattleStageData GetBattleStageData()
        {
            var stage = MasterManager.Stages.GetValue(StageId);

            var enemy = MasterManager.EnemyCharacters.GetValue(stage.EnemyId);

            return new BattleStageData() { StageNum = StageId, Player = Player, Enemy = enemy };
        }
    }
}
