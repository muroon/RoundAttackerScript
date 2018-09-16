using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace View.Scene
{
    public class StartView : ViewBase
    {
        [SerializeField] GameObject playerObject;

        [SerializeField] ThirdPersonCamera mainCamera;

        [SerializeField] GameObject panelTitleObject;

        [SerializeField] GameObject panelSelectObject;

        [SerializeField ]GameObject panelStartObject;

        [SerializeField] Button startButton;

        [SerializeField] List<ButtonSelectCharacter> characterButtons;

        [SerializeField] Button confirmYesButton;

        [SerializeField] Button confirmNoButton;

        /// <summary>
        /// PlayerCharacter選択イベント
        /// </summary>
        Subject<int> idSubject = new Subject<int>();
        public IObservable<int> PlayerCharacterIdEvent
        {
            get { return idSubject.AsObservable(); }
        }

        /// <summary>
        /// バトル開始イベント
        /// </summary>
        Subject<Unit> startBattleSubject = new Subject<Unit>();
        public IObservable<Unit> StartBattleEvent
        {
            get { return startBattleSubject.AsObservable(); }
        }

        void Start()
        {
            mainCamera.SetTarget(playerObject);

            panelTitleObject.SetActive(true);
            panelSelectObject.SetActive(false);
            panelStartObject.SetActive(false);

            // startボタンイベント
            startButton.onClick.AsObservable().Subscribe(_ => SetPanelSelect());

            // PlayerCharacter選択ボタンイベント
            foreach (var charaButton in characterButtons)
                charaButton.IdEvent.Subscribe(id => {
                    idSubject.OnNext(id);
                    SetStartPanel();
                });

            // 最終確認ボタン
            confirmYesButton.onClick.AsObservable().Subscribe(_ => startBattleSubject.OnNext(Unit.Default));
            confirmNoButton.onClick.AsObservable().Subscribe(_ => SetPanelSelect());
        }

        /// <summary>
        /// PlayerCharacter選択パネル表示
        /// </summary>
        void SetPanelSelect()
        {
            panelTitleObject.SetActive(false);
            panelSelectObject.SetActive(true);
            panelStartObject.SetActive(false);
        }

        /// <summary>
        /// バトル開始確認パネル表示
        /// </summary>
        public void SetStartPanel()
        {
            panelSelectObject.SetActive(false);
            panelStartObject.SetActive(true);
        }
    }
}