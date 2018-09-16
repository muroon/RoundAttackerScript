using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace View.Character
{
    public class Knight : PlayerCharacter
    {
        public KnightAttackMotionState.MotionState MotionState = KnightAttackMotionState.MotionState.None;

        KnightAttackMotionState CurrentMotionState {
            get {
                return KnightAttackMotionState.GetMotionState(MotionState);
            }
        }

        bool IsMovingForwardForAttack {
            get {
                return MotionState == KnightAttackMotionState.MotionState.First;
            }
        }

        bool IsMovingBackAfterAttack {
            get {
                return MotionState == KnightAttackMotionState.MotionState.Third;
            }
        }

        Character AttackTarget { get; set; }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            KnightAttackMotionState.Initialize(transform, simpleAnim);
            KnightAttackMotionState.AttackEvent.Subscribe(_ => AttackTarget.Damage(AttackPower)).AddTo(this);
            KnightAttackMotionState.NextStateEvent.Subscribe(state => { MotionState = state; }).AddTo(this);
        }

        protected override void Attacking()
        {
            CurrentMotionState.Execute();
        }

        protected override bool CanMove()
        {
            return MotionState == KnightAttackMotionState.MotionState.None;
        }

        public override void Attack()
        {
            if (CanAttack())
                MotionState = KnightAttackMotionState.MotionState.First;
        }

        protected override bool CanAttack()
        {
            return base.CanAttack() && MotionState == KnightAttackMotionState.MotionState.None;
        }

        /// <summary>
        /// Ons the trigger enter.
        /// </summary>
        /// <param name="col">Col.</param>
        protected override void OnTriggerEnter(Collider col)
        {
            if (IsMovingBackAfterAttack && (col.tag == "stop1" || col.tag == "stop2"))
            {
                // StopPointへ移動中⇒移動終了
                CurrentMotionState.SetNext();
            }

            if (IsMovingForwardForAttack && col.tag == "enemy")
            {
                // 敵に向かって前進移動モーション停止
                // アタックモーション開始
                // 敵にダメージ
                // StopPointへ移動開始
                AttackTarget = col.gameObject.GetComponent<EnemyCharacter>();
                CurrentMotionState.SetNext();
            }

            base.OnTriggerEnter(col);
        }

        protected override void StartDeadAnimation()
        {
            simpleAnim.Play("Dead");
        }
    }
}