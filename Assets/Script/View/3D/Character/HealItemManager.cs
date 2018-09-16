using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace View.Character
{
    public class HealItemManager
    {

        float elapsedTime;

        bool isGenarated = false;

        List<HealItem> healItemPooling = new List<HealItem>();

        List<HealEffect> healEffectPooling = new List<HealEffect>();

        public List<StopEnemy> StopPointEnemies { get; set; }


        GameObject GetStop()
        {
            int cnt = 0;
            while (cnt <= 100)
            {
                var stopEnemy = GetRandomStop();
                if (stopEnemy != null && stopEnemy.GetCharacter() != null)
                {
                    cnt += 1;
                    continue;
                }

                // playerがそのstopPointにいない場合のみ
                return stopEnemy.gameObject;
            }
            return null;
        }

        StopEnemy GetRandomStop()
        {
            int stopIndex = (int)(Random.value * 10 % StopPointEnemies.Count);
            return StopPointEnemies.Count > stopIndex ? StopPointEnemies[stopIndex] : null;
        }

        public void AddElapsedTime(float time)
        {
            if (isGenarated)
                return;
            elapsedTime += time;
        }

        bool CanGenerate(float playerHpRate, float enemyHpRate)
        {
            return elapsedTime > 7.0f && playerHpRate < 1.0f && enemyHpRate < 1.0f;
        }

        public void TrySetRecoverHpItem(float playerHpRate, float enemyHpRate)
        {
            if (!CanGenerate(playerHpRate, enemyHpRate))
                return;

            var stopPoint = GetStop();
            if (stopPoint != null && !isGenarated)
            {
                isGenarated = true;
                var healItem = healItemPooling.FirstOrDefault(_ => !_.gameObject.activeInHierarchy);
                if (healItem == default(HealItem)) {
                    var item = (GameObject)Resources.Load("Prefabs/HealItem");
                    var itemObject = Object.Instantiate(item, stopPoint.transform.position, Quaternion.identity);
                    healItem = itemObject.GetComponent<HealItem>();
                    healItem.AfterHealedEvent.Subscribe(_ => ExecuteAfterHealed(stopPoint.transform.position));
                    healItemPooling.Add(healItem);
                } else {
                    healItem.gameObject.transform.position = stopPoint.transform.position;
                    healItem.AfterHealedEvent.Subscribe(_ => ExecuteAfterHealed(stopPoint.transform.position));
                    healItem.gameObject.SetActive(true);
                }
            }
        }

        void ExecuteAfterHealed(Vector3 pos)
        {
            GenerateAfterHealedEffect(pos);

            isGenarated = false;
            elapsedTime = 0f;
        }

        void GenerateAfterHealedEffect(Vector3 pos)
        {
            var healEffect = healEffectPooling.FirstOrDefault(_ => _ != null && _.gameObject != null && !_.gameObject.activeInHierarchy);

            if (healEffect == default(HealEffect)) {
                var effectObject = (GameObject)Resources.Load("Prefabs/Particles/Soap Bubbles");
                healEffect = Object.Instantiate(effectObject, pos, Quaternion.identity).GetComponent<HealEffect>();
                healEffectPooling.Add(healEffect);
            } else {
                healEffect.gameObject.transform.position = pos;
            }
            healEffect.Initialize();
        }
    }
}