using UnityEngine;
using System.Collections;

namespace View.Character
{
    public class GreeSpider : EnemyCharacter
    {

        int attackState = 0;

        protected override bool IsEnableStartMoving
        {
            get { return base.IsEnableStartMoving && attackState == 0; }
        }

        protected override void Update()
        {
            base.Update();

            Attacking();
        }

        public override void Attack()
        {
            attackState = 1;
        }

        void Attacking()
        {
            if (attackState == 0)
                return;

            if (attackState == 1)
            {
                if (!CanAttack()) ResetAttackState();
                // Move foward
                StartAttack();
            }

            // damage
            if (attackState == 2)
            {
                if (!CanAttack()) ResetAttackState();
                if (stopPoint != null && stopPoint.GetComponent<StopEnemy>().GetCharacter() != null)
                {
                    stopPoint.GetComponent<StopEnemy>().GetCharacter().SendMessage("Damage", AttackPower);
                }
                attackState = 3;
            }

            if (attackState == 3)
            {
                // Move back
                EndAttack();

                if (transform.position == prePosition)
                {
                    ResetAttackState();
                }
            }
        }

        void StartAttack()
        {

            // Move
            transform.Translate(Vector3.forward * 0.1f);
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Move", true);
            }
        }

        void EndAttack()
        {
            // Move back
            transform.position = Vector3.MoveTowards(transform.position, prePosition, 0.1f);
        }

        void ResetAttackState()
        {
            attackState = 0;

            stopPoint = null;

            transform.position = prePosition;
            prePosition = Vector3.zero;
        }


        void OnTriggerEnter(Collider col)
        {
            if (attackState == 1 && (col.tag.ToString() == "stopEnemy"))
            {
                //Debug.Log("transform.position:"+transform.position.ToString());
                //anim.SetBool("Attack", false);
                attackState = 2;
            }
        }
    }
}