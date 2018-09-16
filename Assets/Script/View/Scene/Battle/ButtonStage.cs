using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using StageState = View.Scene.StageButtonsView.StageState;

namespace View.Scene
{
    public class ButtonStage : MonoBehaviour
    {
        [SerializeField] StageState state;

        public IObservable<StageState> ClickEvent
        {
            get { return GetComponent<Button>().onClick.AsObservable().Select(_ => { return state; }); }
        }
    }
}
