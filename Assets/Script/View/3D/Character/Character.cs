using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Character
{
    public class Character : MonoBehaviour
    {

        public enum MoveType
        {
            Auto,
            Left,
            Right
        }

        public int Hp { get; protected set; }

        public int MaxHp { get; protected set; }

        public int AttackPower { get; protected set; }

        public int DefensePower { get; protected set; }

        public virtual bool IsAlive { get { return Hp > 0; } }

        Subject<Unit> deadSubject = new Subject<Unit>();
        public IObservable<Unit> DeadEvent
        {
            get { return deadSubject.AsObservable(); }
        }

        protected bool isInitialized = false;

        protected virtual void Die()
        {
            deadSubject.OnNext(Unit.Default);
        }

        public virtual void Attack()
        {
        }

        public virtual void Damage(int damage)
        {
            if (Hp > 0) Hp = Hp - damage + DefensePower;
            if (Hp < 0) Hp = 0;
        }

        public virtual void Heal(int point)
        {
        }

        public virtual void Move(MoveType type)
        {
        }

        public float GetHpRate()
        {
            double hpd = (double)Hp;
            double maxHpd = (double)MaxHp;

            return (float)System.Math.Round(hpd / maxHpd, 2);
        }
    }
}