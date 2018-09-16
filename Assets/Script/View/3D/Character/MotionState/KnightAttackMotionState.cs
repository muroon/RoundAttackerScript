using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Character
{
    /// <summary>
    /// Knight 攻撃モーションState
    /// </summary>
    public class KnightAttackMotionState : MotionState
    {
        /// <summary>
        /// 全Stateクラスで共有する共通パラメータ
        /// </summary>
        public class MotionStateParameters
        {
            /// <summary>
            /// 攻撃開始直前のPosition
            /// </summary>
            public Vector3 PrePositon = Vector3.zero;
        }

        /// <summary>
        /// Motion state.
        /// </summary>
        public enum MotionState
        {
            None,
            First,
            Second,
            Third,
            Force
        }

        /// <summary>
        /// The states. (MotionStateに沿ったMotionStateクラスを定義)
        /// </summary>
        public static Dictionary<MotionState, KnightAttackMotionState> States = new Dictionary<MotionState, KnightAttackMotionState>()
        {
            {MotionState.None, new KnightAttackMotionStateNone()},
            {MotionState.First, new KnightAttackMotionStateFirst()},
            {MotionState.Second, new KnightAttackMotionStateSecond()},
            {MotionState.Third, new KnightAttackMotionStateThird()},
            {MotionState.Force, new KnightAttackMotionStateForce()},
        };

        /// <summary>
        /// The character transform.
        /// </summary>
        protected Transform characterTransform;

        /// <summary>
        /// The animation.
        /// </summary>
        protected SimpleAnimation anim;

        /// <summary>
        /// The parameters.
        /// </summary>
        protected MotionStateParameters parameters;

        /// <summary>
        /// 攻撃開始直前のPosition
        /// </summary>
        protected Vector3 prePositon {
            get { return parameters != null ? parameters.PrePositon : Vector3.zero; }
            set { parameters.PrePositon = value; }
        }

        /// <summary>
        /// 次State遷移イベント
        /// </summary>
        protected static Subject<MotionState> nextStateSubject = new Subject<MotionState>();
        public static IObservable<MotionState> NextStateEvent
        {
            get { return nextStateSubject.AsObservable(); }
        }

        /// <summary>
        /// 攻撃通知イベント
        /// </summary>
        protected static Subject<Unit> attackSubject = new Subject<Unit>();
        public static IObservable<Unit> AttackEvent
        {
            get { return attackSubject.AsObservable(); }
        }

        /// <summary>
        /// 全Stateオブジェクトの初期化を実行
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="transform">Transform.</param>
        /// <param name="anim">Animation.</param>
        public static void Initialize(Transform transform, SimpleAnimation anim)
        {
            var parameters = new MotionStateParameters();
            foreach (var kv in States)
                kv.Value.Init(transform, anim, parameters);
        }

        /// <summary>
        /// MotionStateオブジェクトを返す
        /// </summary>
        /// <returns>The motion state.</returns>
        /// <param name="state">State.</param>
        public static KnightAttackMotionState GetMotionState(MotionState state)
        {
            if (!States.ContainsKey(state))
                throw new KeyNotFoundException(string.Format("MotionState Error. [{0} is not in States]", state));

            return States[state];
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="transform">Transform.</param>
        /// <param name="anim">Animation.</param>
        /// <param name="parameters">Parameters.</param>
        public void Init(Transform transform, SimpleAnimation anim, MotionStateParameters parameters)
        {
            characterTransform = transform;
            this.anim = anim;
            this.parameters = parameters;
        }
    }

    /// <summary>
    /// Defaultモーションクラス
    /// </summary>
    public class KnightAttackMotionStateNone : KnightAttackMotionState
    {
        public override void Execute()
        {
            prePositon = Vector3.zero;
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.First);
        }
    }

    /// <summary>
    /// 攻撃モーション1クラス
    /// </summary>
    public class KnightAttackMotionStateFirst : KnightAttackMotionState
    {
        public override void Execute()
        {
            if (prePositon == Vector3.zero)
            {
                prePositon = characterTransform.position;
            }

            // Moving
            characterTransform.Translate(Vector3.forward * 0.1f);

            anim.GetState("MoveForward").speed = 1.5f;
            anim.Play("MoveForward");
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.Second);
        }
    }

    /// <summary>
    /// 攻撃モーション2クラス
    /// </summary>
    public class KnightAttackMotionStateSecond : KnightAttackMotionState
    {
        bool isAttacked = false;

        public override void Execute()
        {
            if (anim.IsPlaying("MoveForward"))
                anim.Stop("MoveForward");

            anim.Play("Attack");

            if (!isAttacked)
            {
                isAttacked = true;
                attackSubject.OnNext(Unit.Default);
            }


            if (anim.IsPlaying("Attack"))
            {
                var aState = anim.GetState("Attack");

                if (aState.normalizedTime >= 1f)
                {
                    anim.Stop("Attack");
                    SetNext();
                    isAttacked = false;
                }
            }
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.Third);
        }
    }

    /// <summary>
    /// 攻撃モーション3クラス
    /// </summary>
    public class KnightAttackMotionStateThird : KnightAttackMotionState
    {
        public override void Execute()
        {
            anim.GetState("MoveBack").speed = 1.5f;
            anim.Play("MoveBack");
            characterTransform.position = Vector3.MoveTowards(characterTransform.position, prePositon, 0.1f);
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.Force);
        }
    }

    /// <summary>
    /// 攻撃モーション4クラス
    /// </summary>
    public class KnightAttackMotionStateForce : KnightAttackMotionState
    {
        public override void Execute()
        {
            anim.Stop("MoveBack");

            characterTransform.position = prePositon;

            SetNext();
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.None);
        }
    }
}