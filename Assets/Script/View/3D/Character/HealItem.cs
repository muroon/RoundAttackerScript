using UnityEngine;
using System.Collections;
using UniRx;

namespace View.Character
{
    public class HealItem : MonoBehaviour
    {
        public int recoverValue = 10;

        Subject<Unit> afterHealed = new Subject<Unit>();
        public IObservable<Unit> AfterHealedEvent
        {
            get { return afterHealed.AsObservable(); }
        }

        public void Heal(PlayerCharacter cheracter)
        {
            // Recover Hp
            cheracter.Heal(recoverValue);

            afterHealed.OnNext(Unit.Default);

            gameObject.SetActive(false);
        }
    }
}