using UnityEngine;
using System.Collections;

namespace View.Character
{
    public class Arrow : MonoBehaviour
    {
        Vector3 forward;
        int damagePoint = 10;
        public int speed = 25;

        bool isDiminished = false;

        public void Initialize(Vector3 fowardVector, int point)
        {
            forward = fowardVector;
            damagePoint = point;
            isDiminished = false;
        }

        void Update()
        {
            if (forward != Vector3.zero)
            {
                transform.position += forward * Time.deltaTime * speed;
            }
        }


        void OnTriggerEnter( Collider col )
        {
            TrySendDamage(col);
        }

        void OnTriggerStay( Collider col )
        {
            TrySendDamage(col);
        }

        void TrySendDamage(Collider col)
        {
            if (col.tag != "enemy" || isDiminished)
                return;

            var target = col.gameObject;
            isDiminished = true;
            target.GetComponent<EnemyCharacter>().Damage(damagePoint);
            gameObject.SetActive(false);
            forward = Vector3.zero;
        }
    }
}