using UnityEngine;
using System.Collections;

namespace View.Character
{
    public class StopEnemy : MonoBehaviour
    {

        Stop stop = null;

        // Use this for initialization
        void Start()
        {
            string num = gameObject.name.Replace("StopPointEnemy", "");
            GameObject stopPoint = GameObject.Find("StopPoint" + num);
            stop = stopPoint.GetComponent<Stop>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject GetCharacter()
        {
            return stop.GetCharacter();
        }

        public bool isBlocked()
        {
            return stop.isBlocked();
        }
    }
}