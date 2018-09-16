using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Character
{
    /// <summary>
    /// Player 共通モーションState.
    /// </summary>
    public class PlayerCommonMotionState : MotionState
    {
        /// <summary>
        /// 全Stateクラスで共有する共通パラメータ
        /// </summary>
        public class MotionStateParameters {
            /// <summary>
            /// 移動中かどうか
            /// </summary>
            public bool MovingFlag = false;

            /// <summary>
            /// 移動方向
            /// </summary>
            public Character.MoveType MovingType = Character.MoveType.Left;
        }

        /// <summary>
        /// Motion state.
        /// </summary>
        public enum MotionState
        {
            None,
            Move,
            Death
        }

        /// <summary>
        /// The states. (MotionStateに沿ったMotionStateクラスを定義)
        /// </summary>
        public static Dictionary<MotionState, PlayerCommonMotionState> States = new Dictionary<MotionState, PlayerCommonMotionState>()
        {
            {MotionState.None, new PlayerCommonMotionStateNone()},
            {MotionState.Move, new PlayerCommonMotionStateMove()},
            {MotionState.Death, new PlayerCommonMotionStateDeath()},
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
        /// 移動中かどうか
        /// </summary>
        protected bool movingFlag {
            get { return parameters != null ? parameters.MovingFlag : false; }
            set { parameters.MovingFlag = value; }
        }

        /// <summary>
        /// 移動方向
        /// </summary>
        protected Character.MoveType movingType {
            get { return parameters.MovingType; }
            set { parameters.MovingType = value; }
        }

        /// <summary>
        /// 初期化済かどうか
        /// </summary>
        protected bool isInit = false;
        public bool IsInitialized {
            get { return isInit; }
        }

        /// <summary>
        /// StopPoint間の移動中かどうか
        /// </summary>
        public bool IsMoving
        {
            get { return movingFlag; }
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
        public static PlayerCommonMotionState GetMotionState(MotionState state)
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

            isInit = true;
        }

        /// <summary>
        /// 移動前後に体の向きを変える
        /// </summary>
        /// <param name="turnEnemy">If set to <c>true</c> turn enemy.</param>
        protected void TurnBody(bool turnEnemy)
        {
            int degree = (turnEnemy) ? 90 : -90;
            if (movingType == Character.MoveType.Right) degree *= -1;

            characterTransform.Rotate(0, (float)degree, 0);
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        /// <returns>The move.</returns>
        /// <param name="type">Type.</param>
        public void Move(Character.MoveType type) {
            movingType = type;

            if (movingFlag)
                return;

            if (type != Character.MoveType.Auto)
                TurnBody(false);

            movingFlag = true;

            nextStateSubject.OnNext(MotionState.Move);
        }
    }

    /// <summary>
    /// Defaultモーションクラス
    /// </summary>
    public class PlayerCommonMotionStateNone : PlayerCommonMotionState
    {
    }

    /// <summary>
    /// StopPoint間移動モーションクラス
    /// </summary>
    public class PlayerCommonMotionStateMove : PlayerCommonMotionState
    {
        const float walkAngleRate = 70.0f;

        public override void Execute()
        {
            if (movingFlag)
            {
                float angle = Time.deltaTime * walkAngleRate;
                float d = (2 * Mathf.PI) * (angle / 360);
                float h = 4f * Mathf.Cos(d);
                float v = 4f * Mathf.Sin(d);

                anim.GetState("MoveForward").speed = 1.5f;
                anim.Play("MoveForward");

                float alpha = angle;
                if (movingType == Character.MoveType.Right && alpha >= 0f)
                {
                    alpha *= -1f;
                }
                characterTransform.RotateAround(
                    new Vector3(0, 0, 0),
                    Vector3.up,
                    alpha
                );
            }
        }

        public override void SetNext()
        {
            if (!movingFlag)
                return;

            movingFlag = false;

            TurnBody(true);

            if (anim.IsPlaying("MoveForward"))
                anim.Stop("MoveForward");

            nextStateSubject.OnNext(MotionState.None);
        }
    }

    /// <summary>
    /// 死亡時モーションクラス
    /// </summary>
    public class PlayerCommonMotionStateDeath : PlayerCommonMotionState
    {
        public override void Execute()
        {
            if (!anim.IsPlaying("Dead")) {
                anim.Play("Dead");
            }
        }
    }
}
