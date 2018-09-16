using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace View.Scene
{
    public class StageButtonsView : MonoBehaviour
    {
        public enum StageState
        {
            None,
            Next,
            Restart,
            Exit
        }

        [SerializeField] List<ButtonStage> stageButtons = new List<ButtonStage>();

        ReactiveProperty<StageState> stageState = new ReactiveProperty<StageState>();

        /// <summary>
        /// Stage遷移イベント
        /// </summary>
        public IObservable<StageState> StageEvent
        {
            get { return stageState.AsObservable(); }
        }

        void Start() {

            foreach (var button in stageButtons)
                button.ClickEvent.Subscribe(state => { stageState.Value = state; }).AddTo(this);
        }
    }
}