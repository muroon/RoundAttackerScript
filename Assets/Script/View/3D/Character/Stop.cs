using UnityEngine;
using System.Collections;

namespace View.Character
{
    public class Stop : MonoBehaviour
    {

        public bool blockFlag = false;
        GameObject character = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCharacter(GameObject obj)
        {
            character = obj;
        }

        public GameObject GetCharacter()
        {
            return character;
        }

        public void RemoveCharacter()
        {
            character = null;
        }

        public bool isBlocked()
        {
            return blockFlag;
        }
    }
}