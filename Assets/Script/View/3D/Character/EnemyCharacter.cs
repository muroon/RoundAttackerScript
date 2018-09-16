using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace View.Character
{
    public class EnemyCharacter : Character
    {

        protected Animator anim;

        protected GameObject stopPoint = null; // TODO:削除予定

        public float rotateDelta = 10.0f;

        /// <summary>
        /// 攻撃動作前のposition
        /// </summary>
        protected Vector3 prePosition = Vector3.zero;

        protected bool deadMotionFlag = false;

        int movingState = 0;

        public List<StopEnemy> StopPointEnemies { get; set; }

        public override bool IsAlive
        {
            get { return Hp > 0 && !deadMotionFlag; }
        }

        protected virtual bool IsEnableStartMoving
        {
            get { return movingState == 0; }
        }

        // Use this for initialization
        protected virtual void Start()
        {
            // Animatorコンポーネントを取得する
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!isInitialized)
                return;

            if (deadMotionFlag)
            {
                return;
            }
            if (Hp <= 0)
            {
                Die();
                return;
            }

            Move(MoveType.Auto);

            if (movingState == 2)
                Moving().StartAsCoroutine();
        }

        public void SetParameter(Model.Entity.EnemyCharacter data)
        {
            MaxHp = data.MaxHp;
            Hp = MaxHp;
            AttackPower = data.AttackPower;
            DefensePower = data.DefensePower;
            isInitialized = true;
        }

        protected override void Die()
        {
            if (deadMotionFlag) return;

            base.Die();
            anim.SetBool("Death", true);
            deadMotionFlag = true;
        }

        /*
         * base class用
         * 攻撃可能かどうか？
         */
        protected bool CanAttack()
        {
            if (stopPoint == null) return false;
            return !stopPoint.GetComponent<StopEnemy>().isBlocked();
        }

        /*
         * base class用
         * StopPoint取得用共通メソッド
         */
        protected void GetTargetStopPoint()
        {
            stopPoint = GetRandomStop().gameObject;
        }

        StopEnemy GetRandomStop()
        {
            int stopIndex = (int)(Random.value * 10 % StopPointEnemies.Count);
            return StopPointEnemies.Count > stopIndex ? StopPointEnemies[stopIndex] : null;
        }

        public override void Move(MoveType type)
        {
            if (!IsEnableStartMoving) return;
            movingState = 1;

            GetTargetStopPoint();

            if (stopPoint == null)
            {
                movingState = 0;
                return;
            }

            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Move", true);
            }
            movingState = 2;
        }

        protected IEnumerator Moving()
        {
            Quaternion stopPointRotation = Quaternion.LookRotation(stopPoint.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, stopPointRotation, rotateDelta);

            if (transform.rotation == stopPointRotation)
            {
                movingState = 0;
                Attack();
                yield break;
            }
        }
    }
}