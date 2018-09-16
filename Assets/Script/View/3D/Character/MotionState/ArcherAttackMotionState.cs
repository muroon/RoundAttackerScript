using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Character
{
    /// <summary>
    /// Archer 攻撃モーションState.
    /// </summary>
    public class ArcherAttackMotionState : MotionState
    {
        /// <summary>
        /// 全Stateクラスで共有する共通パラメータ
        /// </summary>
        public class MotionStateParameters
        {
            /// <summary>
            /// 攻撃開始前方位置ベクトル
            /// </summary>
            public Vector3 Forward;
        }

        /// <summary>
        /// Motion state.
        /// </summary>
        public enum MotionState
        {
            None,
            First,
            Second
        }

        /// <summary>
        /// The states. (MotionStateに沿ったMotionStateクラスを定義)
        /// </summary>
        public static Dictionary<MotionState, ArcherAttackMotionState> States = new Dictionary<MotionState, ArcherAttackMotionState>()
        {
            {MotionState.None, new ArcherAttackMotionStateNone()},
            {MotionState.First, new ArcherAttackMotionStateFirst()},
            {MotionState.Second, new ArcherAttackMotionStateSecond()},
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
        /// 攻撃開始前方位置ベクトル
        /// </summary>
        protected Vector3 forward {
            get { return parameters != null ? parameters.Forward : Vector3.zero; }
            set { parameters.Forward = value; }
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
        protected static Subject<Vector3> attackSubject = new Subject<Vector3>();
        public static IObservable<Vector3> AttackEvent
        {
            get { return attackSubject.AsObservable(); }
        }

        /// <summary>
        /// カメラ固定可否イベント
        /// </summary>
        protected static Subject<bool> cammeraFixedSubject = new Subject<bool>();
        public static IObservable<bool> CammeraFixedEvent
        {
            get { return cammeraFixedSubject.AsObservable(); }
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
        public static ArcherAttackMotionState GetMotionState(MotionState state)
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
    public class ArcherAttackMotionStateNone : ArcherAttackMotionState
    {
        public override void Execute()
        {
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.First);
        }
    }

    /// <summary>
    /// 攻撃モーション1クラス
    /// </summary>
    public class ArcherAttackMotionStateFirst : ArcherAttackMotionState
    {
        public override void Execute()
        {
            cammeraFixedSubject.OnNext(true);
            forward = characterTransform.forward;
            characterTransform.Rotate(0.0f, 90.0f, 0.0f);

            anim.GetState("Attack").speed = 1.5f;
            anim.Play("Attack");

            SetNext();
        }

        public override void SetNext()
        {
            nextStateSubject.OnNext(MotionState.Second);
        }
    }

    /// <summary>
    /// 攻撃モーション2クラス
    /// </summary>
    public class ArcherAttackMotionStateSecond : ArcherAttackMotionState
    {
        void Attack()
        {
            attackSubject.OnNext(forward);
            characterTransform.Rotate(0.0f, -90.0f, 0.0f);
            cammeraFixedSubject.OnNext(false);

            anim.Stop("Attack");
        }

        public override void SetNext()
        {
            Attack();
            nextStateSubject.OnNext(MotionState.None);
        }
    }

}