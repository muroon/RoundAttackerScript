using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoveType = View.Character.Character.MoveType;

namespace View.Scene
{
    public class CharacterStatusesView : MonoBehaviour
    {
        public enum PositionType {
            Center,
            Left,
            Right
        }

        readonly Dictionary<MoveType, PositionType> positionTypeSets = new Dictionary<MoveType, PositionType> {
            {MoveType.Left, PositionType.Left},
            {MoveType.Right, PositionType.Right},
        };

        readonly Dictionary<PositionType, Vector3> enemyBarPositions = new Dictionary<PositionType, Vector3> {
            {PositionType.Center, new Vector3(120f, 70f, 0f)},
            {PositionType.Left, new Vector3(180f, 70f, 0f)},
            {PositionType.Right, new Vector3(-180f, 70f, 0f)},
        };

        [SerializeField] Slider playerHpBar;

        [SerializeField] Slider enemyHpBar;

        public void SetPlayerHp(float hpRate)
        {
            playerHpBar.value = hpRate;
        }

        public void SetEnemyHp(float hpRate)
        {
            enemyHpBar.value = hpRate;
        }

        public void SetEnemyHpBarPositon(MoveType moveType)
        {
            if (!positionTypeSets.ContainsKey(moveType))
                throw new ArgumentException(string.Format("SetEnemyHpBarPositon. MoveType:{0} isn't allowed", moveType));
            SetEnemyHpBarPositon(positionTypeSets[moveType]);
        }

        public void SetEnemyHpBarPositon(PositionType type)
        {
            if (!enemyBarPositions.ContainsKey(type))
                throw new ArgumentException(string.Format("SetEnemyHpBarPositon. Position:{0} isn't included", type));

            enemyHpBar.transform.localPosition = enemyBarPositions[type];
        }
    }
}