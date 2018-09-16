using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using View.Character;

namespace View.Scene
{
    public class BattleView : ViewBase
    {
        const string MessageWin = "WIN!!";
        const string MessageLose = "LOSE!!";
        const string MessageStage = "STAGE {0}";

        [SerializeField] Button buttonLeft;

        [SerializeField] Button buttonRight;

        [SerializeField] List<ButtonMove> moveButtons = new List<ButtonMove>();

        [SerializeField] Button buttonAttack;

        [SerializeField] StageButtonsView stageView;

        [SerializeField] Text label;

        [SerializeField] CharacterStatusesView statusesView;

        [SerializeField] List<StopEnemy> stopPointEnemies = new List<StopEnemy>(); // TODO:

        [SerializeField] ThirdPersonCamera mainCamera;

        PlayerCharacter playerCharacter;

        EnemyCharacter enemyCharacter;

        HealItemManager healItemManager;

        bool isInitialized = false;

        /// <summary>
        /// Stage遷移イベント
        /// </summary>
        public IObservable<StageButtonsView.StageState> StageEvent
        {
            get { return stageView.StageEvent; }
        }

        public void Initialize(Model.Entity.BattleStageData stageData)
        {
            label.text = string.Format(MessageStage, stageData.StageNum);
            label.gameObject.SetActive(true);

            SetupPlayerCharacter(stageData.Player);
            SetupEmenyCharacter(stageData.Enemy);

            // 移動ボタンイベント
            foreach (var moveButton in moveButtons)
                moveButton.ClickEvent.Subscribe(type => {
                    playerCharacter.Move(type);
                    statusesView.SetEnemyHpBarPositon(type);
                }).AddTo(this);

            // Attackボタンイベント
            buttonAttack.onClick.AsObservable().Subscribe(_ => playerCharacter.Attack()).AddTo(this);

            // Attackボタンの表示可否
            playerCharacter.AttackEnableEvent.Subscribe(flag => {
                buttonAttack.gameObject.SetActive(flag);
                if (flag)
                    statusesView.SetEnemyHpBarPositon(CharacterStatusesView.PositionType.Center);
            }).AddTo(this);
            buttonAttack.gameObject.SetActive(false);

            statusesView.SetPlayerHp(1.0f);
            statusesView.SetEnemyHp(1.0f);

            // Stageボタン群表示
            stageView.gameObject.SetActive(false);

            // Character死亡イベント
            playerCharacter.DeadEvent.Subscribe(_ => {
                // Lose
                WaitAfterSeconds(0.5f, () => EndBattle(false)).StartAsCoroutine();
            }).AddTo(this);
            enemyCharacter.DeadEvent.Subscribe(_ => {
                // Win
                WaitAfterSeconds(0.5f, () => EndBattle(true)).StartAsCoroutine();
            }).AddTo(this);

            healItemManager = new HealItemManager();
            healItemManager.StopPointEnemies = stopPointEnemies;

            isInitialized = true;
        }

        void Update()
        {
            if (!isInitialized)
                return;

            if (label.gameObject.activeInHierarchy)
            {
                WaitAfterSeconds(1f, () => {
                    label.gameObject.SetActive(false);
                }).StartAsCoroutine();
            }

            SetHpBar().StartAsCoroutine();

            healItemManager.AddElapsedTime(Time.deltaTime);
            healItemManager.TrySetRecoverHpItem(playerCharacter.GetHpRate(), enemyCharacter.GetHpRate());
        }

        void SetupEmenyCharacter(Model.Entity.EnemyCharacter data)
        {
            Vector3 pos = Vector3.zero;
            var prefab = (GameObject)Resources.Load("Prefabs/Enemy/" + data.Resource);
            var obj = Instantiate(prefab, pos, Quaternion.identity);
            enemyCharacter = obj.GetComponent<EnemyCharacter>();
            enemyCharacter.SetParameter(data);
            enemyCharacter.StopPointEnemies = stopPointEnemies;
        }

        void SetupPlayerCharacter(Model.Entity.PlayerCharacter data)
        {
            Vector3 pos = new Vector3(-4.0f, 0.0f, 0.0f);
            var prefab = (GameObject)Resources.Load("Prefabs/Player/" + data.Resource);
            var obj = Instantiate(prefab, pos, Quaternion.identity);
            playerCharacter = obj.GetComponent<PlayerCharacter>();
            playerCharacter.SetParameter(data);
            mainCamera.SetTarget(playerCharacter.gameObject);
            playerCharacter.SetThirdPersonCamera(mainCamera);
            playerCharacter.InitCommonMotion();
        }

        IEnumerator SetHpBar()
        {
            yield return new WaitForSeconds(0.5f);
            statusesView.SetPlayerHp(playerCharacter.GetHpRate());
            statusesView.SetEnemyHp(enemyCharacter.GetHpRate());
        }

        IEnumerator WaitAfterSeconds(float sec, System.Action callback)
        {
            yield return new WaitForSeconds(sec);
            callback();
        }

        void EndBattle(bool isWin)
        {
            isInitialized = false;

            label.text = isWin ? MessageWin : MessageLose;
            label.gameObject.SetActive(true);

            stageView.gameObject.SetActive(true);

            buttonAttack.gameObject.SetActive(false);
        }
    }
}