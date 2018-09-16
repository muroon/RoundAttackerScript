using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace View.Character
{
    public class Archer : PlayerCharacter
    {
        [SerializeField] 
        GameObject bowObject; // it_archer_bow_01 gameobject

        List<Arrow> arrowPooling = new List<Arrow>();

        public ArcherAttackMotionState.MotionState MotionState = ArcherAttackMotionState.MotionState.None;

        bool IsRotatingForAttack
        {
            get
            {
                return MotionState == ArcherAttackMotionState.MotionState.Second;
            }
        }

        ArcherAttackMotionState CurrentMotionState
        {
            get
            {
                return ArcherAttackMotionState.GetMotionState(MotionState);
            }
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            ArcherAttackMotionState.Initialize(transform, simpleAnim);
            ArcherAttackMotionState.AttackEvent.Subscribe(forward => { ShootArrow(forward); }).AddTo(this);
            ArcherAttackMotionState.NextStateEvent.Subscribe(state => { MotionState = state; }).AddTo(this);
            ArcherAttackMotionState.CammeraFixedEvent.Subscribe(flag => { cameraInstance.FixFlag = flag; }).AddTo(this);
        }

        protected override bool CanMove()
        {
            return MotionState == ArcherAttackMotionState.MotionState.None;
        }

        public override void Attack()
        {
            if (CanAttack())
                MotionState = ArcherAttackMotionState.MotionState.First;
        }

        protected override bool CanAttack()
        {
            return base.CanAttack() && MotionState == ArcherAttackMotionState.MotionState.None;
        }

        protected override void Attacking()
        {
            CurrentMotionState.Execute();
        }

        void ShootArrow(Vector3 forward)
        {
            var arrow = arrowPooling.FirstOrDefault(_ => !_.gameObject.activeInHierarchy);

            if (arrow != default(Arrow) ) {
                arrow.gameObject.transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);
                arrow.gameObject.transform.rotation = Quaternion.identity;
                arrow.gameObject.SetActive(true);
            } else {
                var arrowObj = (GameObject)Resources.Load("Prefabs/arrow");
                var arrowObject = (GameObject)Instantiate(arrowObj, new Vector3(transform.position.x, 0.9f, transform.position.z), Quaternion.identity);
                arrow = arrowObject.GetComponent<Arrow>();
                arrowPooling.Add(arrow);
            }

            arrow.Initialize(forward, AttackPower);
        }

        void OnAttack()
        {
            if (!IsRotatingForAttack)
                return;

            CurrentMotionState.SetNext();
        }

        protected override void Die()
        {
            if (deadMotionFlag) return;
            base.Die();
            transform.localPosition += new Vector3(0, 0.06f, 0);
            bowObject.SetActive(false);
        }

        protected override void StartDeadAnimation()
        {
            simpleAnim.Play("Dead");
        }
    }
}