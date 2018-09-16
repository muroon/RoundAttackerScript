using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Scene;
using UniRx;
using UnityEngine.SceneManagement;
using StageState = View.Scene.StageButtonsView.StageState;
using Model.Logic;
using Model.Entity;

namespace Presenter.Scene
{
    public class BattlePresenter : PresenterBase
    {
        const string SceneName = "Battle";

        [SerializeField] BattleView view;

        protected override void Start()
        {
            // TODO:
            //if (BattleManager.StageId == 0)
            //    BattleManager.SetNextStage();
            
            //BattleManager.SetPlayerCharacter(1); // TODO:
            //BattleManager.SetPlayerCharacter(2); // TODO:

            view.StageEvent.Subscribe(state => MoveStage(state)).AddTo(this);
            view.Initialize(BattleManager.GetBattleStageData());
        }

        /// <summary>
        /// Scene遷移
        /// </summary>
        /// <param name="state">State.</param>
        void MoveStage(StageState state)
        {
            switch(state) {
                case StageState.Next:
                    BattleManager.SetNextStage();
                    SceneManager.LoadScene(SceneName);
                    break;
                case StageState.Restart:
                    SceneManager.LoadScene(SceneName);
                    break;
                case StageState.Exit:
                    SceneManager.LoadScene("Start");
                    break;
            }
        }
    }
}
