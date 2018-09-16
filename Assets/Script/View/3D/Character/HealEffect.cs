using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View.Character
{
    public class HealEffect : MonoBehaviour
    {
        float elapsedTime = 0f;

        public void Initialize()
        {
            elapsedTime = 0f;
            gameObject.SetActive(true);
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 10f)
                gameObject.SetActive(false);
        }
    }
}