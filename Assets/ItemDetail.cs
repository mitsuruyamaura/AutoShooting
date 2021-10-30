using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDetail : MonoBehaviour
{
    private const string PLAYER = "Player";

    //public string itemName;
    public CONDITION_TYPE conditionType;
    public ITEM_TYPE itemType;
    //public float effectiveTime;
    //public float duration;

    private MoveSample playerController;
    private bool isTriggerItem;

    public Material[] itemMaterials;
    public MeshRenderer meshRenderer;
    public ItemBase itemBase;

    /// <summary>
    /// データベースからアイテムのデータを受け取り、このアイテムの情報としてセットをする
    /// </summary>
    /// <param name="itemData">データベースのアイテムデータ１つ</param>
    public void InitItem(GameData.DropItemData itemData) {
        // 各項目の設定
        itemBase.name = itemData.name;
        itemBase.itemType = itemData.itemType;
        itemBase.efficacyPoint = itemData.effectiveTime;
        itemBase.duration = itemData.option_1;

        // 画像をアイテムに合わせて変更
        meshRenderer.material = itemMaterials[itemData.no];

        // 各効果による内容を設定するためにコンディションを設定
        itemBase.conditionType = (CONDITION_TYPE)Enum.Parse(typeof(CONDITION_TYPE), itemData.name, true);
    }

    /// <summary>
    /// Playerにコンディションを追加
    /// </summary>
    //private void AddPlayerConditions() {
    //    switch (conditionType) {
    //        case CONDITION_TYPE.MOVE_SPEED:
    //            PlayerConditionSpeedUp speedUp;
    //            if (playerController.gameObject.TryGetComponent(out speedUp)) {
    //                // すでに同じコンディションが付与されているなら効果時間を延長
    //                speedUp.AddEffectiveTime(effectiveTime);
    //            } else {
    //                // 付与されていないならコンディションを追加
    //                speedUp = playerController.gameObject.AddComponent<PlayerConditionSpeedUp>().GetComponent<PlayerConditionSpeedUp>();
    //                speedUp.InitCondition(CONDITION_TYPE.MOVE_SPEED, effectiveTime, duration, playerController);
    //            }
    //            break;
    //        case CONDITION_TYPE.ATTACK_POWER:
    //            PlayerConditionAttackPowerUp attackPowerUp;
    //            if (playerController.gameObject.TryGetComponent(out attackPowerUp)) {
    //                attackPowerUp.AddEffectiveTime(effectiveTime);
    //            } else {
    //                attackPowerUp = playerController.gameObject.AddComponent<PlayerConditionAttackPowerUp>().GetComponent<PlayerConditionAttackPowerUp>();
    //                attackPowerUp.InitCondition(CONDITION_TYPE.ATTACK_POWER, effectiveTime, duration, playerController);
    //            }
    //            break;
    //        case CONDITION_TYPE.JUMP_POWER:
    //            PlayerConditionJumpPowerUp jumpPowerUp;
    //            if (playerController.gameObject.TryGetComponent(out jumpPowerUp)) {
    //                jumpPowerUp.AddEffectiveTime(effectiveTime);
    //            } else {
    //                jumpPowerUp = playerController.gameObject.AddComponent<PlayerConditionJumpPowerUp>().GetComponent<PlayerConditionJumpPowerUp>();
    //                jumpPowerUp.InitCondition(CONDITION_TYPE.JUMP_POWER, effectiveTime, duration, playerController);
    //            }
    //            break;

    //            // TODO 子クラスが実装できたら、各コンディションごとに設定追加
    //    }
    //}

    ///// <summary>
    ///// アイテムの効果決定
    ///// </summary>
    //private void SubmitItemEffect() {
    //    switch (itemType) {
    //        case ITEM_TYPE.INSTANT:
    //            // 即時効果
    //            InstantItemEffect();
    //            break;
    //        case ITEM_TYPE.CONDITION_CHANGE:
    //            // Playerにコンディションを付与する
    //            AddPlayerConditions();
    //            break;
    //        case ITEM_TYPE.TREASURE:
    //            // TODO リザルト用の宝箱のカウントを増やす

    //            break;
    //    }
    //}

    ///// <summary>
    ///// 即時効果のアイテムの効果発動と解決
    ///// </summary>
    //private void InstantItemEffect() {
    //    switch (conditionType) {
    //        case CONDITION_TYPE.RECOVER:
    //            // HP回復
    //            playerController.hp += (int)duration;

    //            // HPが最大値を超えないように制御
    //            playerController.hp = Mathf.Clamp(playerController.hp, 0, playerController.maxHp);
    //            Debug.Log(playerController.hp);
    //            break;
    //    }
    //}

    private void OnTriggerEnter(Collider col) {
        if (!col.gameObject.CompareTag(PLAYER)) {
            return;
        }

        if (isTriggerItem) {
            return;
        }
        isTriggerItem = true;

        col.gameObject.TryGetComponent(out playerController);  // 事前に変数を用意してあると型を省略できる

        if (playerController == null) {
            return;
        }

        //SubmitItemEffect();

        // アイテム取得情報の表示
        StartCoroutine(UIManager.instance.CreateItemInfo(itemBase.name));

        Destroy(gameObject);
    }
}
