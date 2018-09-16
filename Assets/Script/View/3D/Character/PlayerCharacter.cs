//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;
using UniRx;

namespace View.Character
{
    // 必要なコンポーネントの列記
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerCharacter : Character
    {
        // キャラクターコントローラ（カプセルコライダ）の参照
        protected CapsuleCollider col;

        // キャラクターコントローラ（カプセルコライダ）の移動量
        protected Vector3 velocity;

        protected GameObject cameraObject;  // メインカメラへの参照
        protected ThirdPersonCamera cameraInstance;

        protected SimpleAnimation simpleAnim;


        /// <summary>
        /// StopPointタイプ(1 or 2)
        /// </summary>
        protected int stopPointType = 1;

        /// <summary>
        /// StopPointゲームオブジェクト
        /// </summary>
        protected GameObject stopPoint = null;

        protected bool deadMotionFlag = false;

        public PlayerCommonMotionState.MotionState CommonMotionState = PlayerCommonMotionState.MotionState.None;

        PlayerCommonMotionState CurrentCommonMotionState
        {
            get
            {
                return PlayerCommonMotionState.GetMotionState(CommonMotionState);
            }
        }

        /// <summary>
        /// 攻撃開始可否に通知
        /// </summary>
        Subject<bool> attackEnable = new Subject<bool>();
        public IObservable<bool> AttackEnableEvent
        {
            get { return attackEnable.AsObservable(); }
        }

        /// <summary>
        /// StopPoint間の移動中かどうか
        /// </summary>
        protected bool IsMovingBetweenStops {
            get { return CurrentCommonMotionState.IsMoving; }
        }

        // 初期化
        protected virtual void Start()
        {
            simpleAnim = GetComponent<SimpleAnimation>();

            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            col = GetComponent<CapsuleCollider>();

            //メインカメラを取得する
            cameraObject = GameObject.FindWithTag("MainCamera");

            attackEnable.OnNext(false);

            deadMotionFlag = false;

            PlayerCommonMotionState.Initialize(transform, simpleAnim);
            CurrentCommonMotionState.Move(MoveType.Auto);
        }

        protected virtual void Update()
        {
            if (!isInitialized)
                return;

            CurrentCommonMotionState.Execute();

            if (Hp <= 0)
            {
                Die();
                return;
            }

            Attacking();
        }

        public void SetParameter(Model.Entity.PlayerCharacter data)
        {
            MaxHp = data.MaxHp;
            Hp = MaxHp;
            AttackPower = data.AttackPower;
            DefensePower = data.DefensePower;
            isInitialized = true;
        }

        public void InitCommonMotion()
        {
            PlayerCommonMotionState.NextStateEvent.Subscribe(state => { CommonMotionState = state; }).AddTo(this);
            if (CurrentCommonMotionState.IsInitialized)
                CurrentCommonMotionState.Move(MoveType.Auto);
        }

        /// <summary>
        /// 現在立っているStopPointのTypeとは反対のTypeを取得
        /// </summary>
        /// <returns>The other stop point type.</returns>
        int GetOtherStopPointType()
        {
            return (stopPointType == 1) ? 2 : 1;
        }

        protected override void Die()
        {
            if (deadMotionFlag) return;

            base.Die();
            deadMotionFlag = true;

            CommonMotionState = PlayerCommonMotionState.MotionState.Death;
        }

        public override void Move(MoveType type)
        {
            if (!CanMove())
            {
                return;
            }

            if (stopPoint != null)
            {
                stopPoint.GetComponent<Stop>().RemoveCharacter();
                stopPoint = null;
            }

            CurrentCommonMotionState.Move(type);
        }

        protected virtual bool CanMove()
        {
            return true;
        }

        protected virtual bool CanAttack()
        {
            if (stopPoint == null) return false;
            return !stopPoint.GetComponent<Stop>().isBlocked();
        }

        protected virtual void Attacking()
        {
        }

        public override void Heal(int point)
        {
            Hp += point;
            if (Hp > MaxHp) Hp = MaxHp;
        }

        public void SetThirdPersonCamera(ThirdPersonCamera instance)
        {
            cameraInstance = instance;
        }

        /// <summary>
        /// Ons the trigger enter.
        /// </summary>
        /// <param name="col">Col.</param>
        protected virtual void OnTriggerEnter(Collider col)
        {
            if (IsMovingBetweenStops && col.tag == "stop" + GetOtherStopPointType().ToString())
            {
                stopPointType = GetOtherStopPointType();
                stopPoint = col.gameObject;
                if (stopPoint.GetComponent<Stop>() != null)
                {
                    stopPoint.GetComponent<Stop>().SetCharacter(gameObject);
                }

                attackEnable.OnNext(true); // 攻撃開始可能通知

                CurrentCommonMotionState.SetNext();
            }

            if (col.tag == "hprecover")
            {
                var healItem = col.gameObject.GetComponent<HealItem>();
                healItem.Heal(this);
            }
        }

        protected virtual void StartDeadAnimation()
        {
        }
    }
}