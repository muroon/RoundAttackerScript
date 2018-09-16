using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using View.Character;
using MoveType = View.Character.Character.MoveType;

namespace View.Scene
{
    public class ButtonMove : MonoBehaviour
    {
        [SerializeField] MoveType moveType;

        public IObservable<MoveType> ClickEvent
        {
            get { return GetComponent<Button>().onClick.AsObservable().Select(_ => { return moveType; }); }
        }
    }
}