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
    public class StartPresenter : PresenterBase
    {
        [SerializeField] StartView view;

        protected override void Start()
        {
            BattleManager.InitializeStage();

            // PlayerCharacter選択イベント
            view.PlayerCharacterIdEvent.Subscribe(id => {
                BattleManager.SetPlayerCharacter(id);
            });

            // バトル開始イベント
            view.StartBattleEvent.Subscribe(_ => {
                SceneManager.LoadScene("Battle");
            });
        }
    }
}